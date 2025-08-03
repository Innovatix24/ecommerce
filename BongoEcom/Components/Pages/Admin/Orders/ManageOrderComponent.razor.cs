using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Enums;

namespace BongoEcom.Components.Pages.Admin.Orders;

public partial class ManageOrderComponent : IDialogContentComponent
{
    private string Name = "";
    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public OrderDto? Content { get; set; }

    private List<OrderStatus> StatusList = new();

    bool IsLoading = true;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        
        foreach (var item in Enum.GetValues<OrderStatus>())
        {
            StatusList.Add(item);
        }

        IsLoading = false;
    }

    private async void AddItemAsync()
    {
        if (Content == null)
        {
            Content = new();
        }

        await Dialog.CloseAsync(Content);
    }

    private async void CancelAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    private async Task UpdateOrderStatusAsync(OrderDto order)
    {
        var cancelled = await ShowConfirmationAsync();
        if (cancelled) return;

        var command = new UpdateOrderStatusCommand() { OrderId = order.Id, NewStatus = (OrderStatus)order.Status };
        var result = await _mediator.Send(command);
        if (result.IsSuccess)
        {
            order.Status = order.Status;
            await ShowSuccessAsync();
            await Dialog.CloseAsync(Content);
        }
        else
        {
            await ShowErrorAsync(result.Error);
        }
    }

    private async Task ShowErrorAsync(string error)
    {
        var dialog = await dialogueService.ShowErrorAsync(error);
        var result = await dialog.Result;
    }
    private async Task<bool> ShowConfirmationAsync()
    {
        var dialog = await dialogueService.ShowConfirmationAsync("Are you <strong>sure</strong> you want to delete this item? <br /><br />This will also remove any linked items");
        var result = await dialog.Result;
        return result.Cancelled;
    }

    private async Task ShowSuccessAsync()
    {
        var dialog = await dialogueService.ShowSuccessAsync("The action was a success");
        var result = await dialog.Result;
    }
}
