using Application.Features.Orders.Queries;
using Domain.Entities;

namespace BongoEcom.Components.Pages.Admin.Orders;

public partial class AdminOrderDetailsPage
{
    private OrderDto? Order;
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var query = new GetOrderDetailsByIdQuery(OrderId);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            Order = result.Data;
        }
        isLoading = false;
    }
}
