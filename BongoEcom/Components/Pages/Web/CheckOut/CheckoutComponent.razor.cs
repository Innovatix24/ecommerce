
using Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BongoEcom.Components.Pages.Web.CheckOut;

public partial class CheckoutComponent
{
    bool loading = true;
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            loading = true;
            await LoadData();
            StateHasChanged();
            loading = false;
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private List<DeliveryChargeDto> deliveryCharges = new();
    private async Task LoadData()
    {
        var response = await _mediator.Send(new GetDeliveryChargesQuery());
        loading = false;
        if (response.IsSuccess)
        {
            deliveryCharges = response.Data ?? new();
            var delivery = deliveryCharges.FirstOrDefault(x => x.AreaType == "Outside Dhaka");
            if (delivery is null) return;

            DeliveryCharge = delivery.ChargeAmount;
            selectedDeliveryChargeId = delivery.Id;
            StateHasChanged();
        }
    }

    CouponDto? coupon;
    private async void ApplyCoupon()
    {
        var query = new GetCouponInfoQuery(order.Coupon ?? "");
        var response = await _mediator.Send(query);
        if (response.IsSuccess)
        {
            coupon = response.Data;
            Discount = coupon.DiscountValue;
            await alertService.ShowSuccessAsync("Coupon applied");
        }
        else
        {
            Discount = 0;
            await alertService.ShowErrorAsync(response.Error);
        }
        StateHasChanged();
    }

    private void HandleDelAddChange(ChangeEventArgs args)
    {
        if (args.Value == null) return;
        var value = Byte.Parse(args.Value.ToString());
        selectedDeliveryChargeId = value;
        var delivery = deliveryCharges.First(x => x.Id == value);
        DeliveryCharge = delivery.ChargeAmount;
        Message = value.ToString();
    }

    private string Validate()
    {
        if (string.IsNullOrEmpty(order.FullName))
        {
            return "Customer Name is required";
        }
        if (string.IsNullOrEmpty(order.PhoneNumber))
        {
            return "Mobile No is required";
        }
        if (string.IsNullOrEmpty(order.Address))
        {
            return "Address is required";
        }
        return "";
    }

    private async Task SubmitOrder()
    {
        var error = Validate();
        if (!string.IsNullOrEmpty(error))
        {
            await alertService.ShowErrorAsync(error);
            return;
        }

        order.DeliveryCharge = DeliveryCharge;
        order.Total = CartService.GetTotalPrice();
        if (coupon is not null)
        {
            order.CouponId = coupon.Id;
            order.Discount = Discount;
        }
        
        var confirmed = await _jsRuntime.InvokeAsync<bool>("confirm", "Are you sure to place order?");

        if (!confirmed)
        {
            return;
        }

        UIService.ShowLoader();
        var result = await _mediator.Send(order);
        UIService.HideLoader();

        if (result.IsSuccess)
        {
            CartService.ClearCart();
            Navigation.NavigateTo($"/order-confirmation/{result.Data}");
        }
        else
        {
            Message = result.Error ?? "Something went wrong while placing the order.";
        }
    }
}
