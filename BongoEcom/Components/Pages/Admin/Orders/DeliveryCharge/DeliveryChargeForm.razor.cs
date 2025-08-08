using Application.Features.Categories.DTOs;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection.Metadata;

namespace BongoEcom.Components.Pages.Admin.Orders.DeliveryCharge;

public partial class DeliveryChargeForm : IDialogContentComponent
{
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public DeliveryChargeDto? Content { get; set; } = new();

    public async void HandleValidSubmit()
    {
        var command = new CreateDeliveryChargeCommand()
        {
            Id = Content.Id,
            AreaType = Content.AreaType,
            ChargeAmount = Content.ChargeAmount,
            FreeShippingThreshold = Content.FreeShippingThreshold,
        };
        var response = await _mediator.Send(command);
        if (response.IsSuccess)
        {
            await Dialog.CloseAsync("Saved successfully");
            await UIService.ShowSuccessAsync("Saved successfully");
        }
    }
    public void CancelAsync()
    {

    }
}
