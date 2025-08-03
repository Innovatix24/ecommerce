

using Application.Features.Coupons.Queries;

namespace BongoEcom.Components.Pages.Admin.Coupons;

public partial class ManageCouponPage
{
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadData();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadData()
    {
        var query = new GetCouponsQuery();
        var response = await _mediator.Send(query);
        if (response.IsSuccess)
        {
            coupons = response.Data ?? new();
            StateHasChanged();
        }
    }
}
