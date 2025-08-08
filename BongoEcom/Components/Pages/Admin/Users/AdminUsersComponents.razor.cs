using Application.Auth.DTOs;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Application.Features.Users.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared.Enums;
using BongoEcom.Components.Pages.Admin.Orders;
using Microsoft.AspNetCore.Identity;

namespace BongoEcom.Components.Pages.Admin.Users;

public partial class AdminUsersComponents
{
    private List<UserDto> Users = new();
    private List<OrderStatus> StatusList = new();
    private bool IsLoading = true;
    private OrderStatus SelectedValue;
    private OrderStatus SelectedOption;

    DataGridRowSize rowSize = DataGridRowSize.Small;
    PaginationState pagination = new PaginationState { ItemsPerPage = 10 };

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        LoadUsers();
        IsLoading = false;

        foreach (var item in Enum.GetValues<OrderStatus>())
        {
            StatusList.Add(item);
        }
    }

    //private void LoadUsers()
    //{
    //    var result = await _mediator.Send(new GetAllUsersQuery());
    //    if (result.IsSuccess)
    //    {
    //        Users = result.Data ?? new();
    //    }
    //}

    private void LoadUsers()
    {
        var users = userManager.Users.Select(c => new UserDto
        {
            Id = c.UserId,
            UserName = c.UserName,
            FullName = c.UserName,
            Email = c.Email ?? "",
        }).ToList();
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

    public async void ManageOrder(OrderDto order)
    {
        DialogParameters parameters = new()
        {
            Title = $"Manage Order",
            PrimaryAction = "Yes",
            PrimaryActionEnabled = false,
            SecondaryAction = "No",
            Width = "450px",
            TrapFocus = false,
            Modal = false,
            PreventScroll = true
        };

        IDialogReference dialog = await DialogService.ShowDialogAsync<ManageOrderComponent>(order, parameters);
        DialogResult? result = await dialog.Result;
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
