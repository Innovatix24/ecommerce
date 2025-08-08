using Application.Features.Categories.DTOs;
using Application.Features.Orders.Queries;
using BongoEcom.Components.Pages.Admin.Categories;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom.Components.Pages.Admin.Orders.DeliveryCharge;
public partial class ManageDelivaryChargePage
{

    public async void CreateItem()
    {
        OpenModal("Create Category");
    }

    public async void EditItem(DeliveryChargeDto item)
    {
        OpenModal("Edit Category", item);
    }
    public async void OpenModal(string title, DeliveryChargeDto? item = null)
    {
        DialogParameters parameters = new()
        {
            Title = title,
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<DeliveryChargeForm>(item, parameters);
        DialogResult? result = await dialog.Result;
        if (result.Data is not null)
        {
            await LoadData();
        }
    }

    private async Task LoadData()
    {
        loading = true;
        var response = await _mediator.Send(new GetDeliveryChargesQuery());
        loading = false;
        if (response.IsSuccess)
        {
            deliveryCharges = response.Data ?? new();
            StateHasChanged();
        }
    }
}
