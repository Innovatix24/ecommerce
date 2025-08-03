
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using QuestPDF.Fluent;
using Shared.Enums;
using BongoEcom.PDF;

namespace BongoEcom.Components.Pages.Admin.Orders;

public partial class ManageOrdersPage
{
    private List<OrderDto> Orders = new();
    private List<OrderDto> OrdersFiltered = new();
    private List<OrderStatus> StatusList = new();
    private bool IsLoading = true;
    private OrderStatus SelectedValue;
    private OrderStatus SelectedOption;

    private OrderStatus FilterStatus ;

    DataGridRowSize rowSize = DataGridRowSize.Medium;
    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    protected override async Task OnInitializedAsync()
    {
        await LoadOrders();
        foreach (var item in Enum.GetValues<OrderStatus>())
        {
            StatusList.Add(item);
        }
    }

    private async Task LoadOrders()
    {
        IsLoading = true;
        StateHasChanged();

        var result = await _mediator.Send(new GetAllOrdersQuery());
        if (result.IsSuccess)
        {
            Orders = result.Data ?? new();
            
        }
        foreach (var order in Orders)
        {
            order.Status = order.Status;
        }

        OrdersFiltered = Orders;
        IsLoading = false;
        StateHasChanged();
    }


    private void HandleStatusFilterChanged(OrderStatus status)
    {
        FilterStatus = status;
        OrdersFiltered = Orders.Where(x => x.Status == (byte)status).ToList();
    }

    private void HandleSearch(string filter)
    {
        filter = filter.Trim().ToLower();
        if (string.IsNullOrEmpty(filter))
        {
            OrdersFiltered = Orders;
        }
        else
        {
            OrdersFiltered = Orders.Where(x => x.CustomerName.ToLower().Contains(filter)
            || x.CustomerPhone.Contains(filter)
            || x.OrderNo.ToString().Contains(filter)
            ).ToList();
        }
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
        }
        else
        {
            await ShowErrorAsync(result.Error);
        }
    }

    public void GoToDetails(OrderDto order)
    {
        Navigation.NavigateTo($"admin/order/{order.Id}/details");
    }

    private async void PrintOrder(OrderDto order)
    {
        var query = new GetOrderDetailsByIdQuery(order.Id);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            await companyService.Load();
            if (result.Data is null) return;
            var document = new OrderPdfDocument(result.Data, _env, companyService.CompanyInfo);
            var bytes = document.GeneratePdf();
            var base64Str = Convert.ToBase64String(bytes);
            await JS.InvokeVoidAsync("openPdfInNewTab", base64Str);
        }
    }

    public async void ManageOrder(OrderDto order)
    {
        DialogParameters parameters = new()
        {
            Title = $"Update status",
            Width = "300px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<ManageOrderComponent>(order, parameters);
        DialogResult? result = await dialog.Result;
        if(result.Data is not null)
        {
            await LoadOrders();
        }
    }

    private void HanldeStatusChanged(ChangeEventArgs e)
    {

    }

    private async Task ShowSuccessAsync()
    {
        var dialog = await DialogService.ShowSuccessAsync("The action was a success");
        var result = await dialog.Result;
    }

    private async Task ShowWarningAsync()
    {
        var dialog = await DialogService.ShowWarningAsync("This is your final warning");
        var result = await dialog.Result;
    }

    private async Task ShowErrorAsync(string error)
    {
        var dialog = await DialogService.ShowErrorAsync(error);
        var result = await dialog.Result;
    }
    private async Task<bool> ShowConfirmationAsync()
    {
        var dialog = await DialogService.ShowConfirmationAsync("Are you <strong>sure</strong> you want to delete this item? <br /><br />This will also remove any linked items");
        var result = await dialog.Result;
        return result.Cancelled;
    }
}
