using Application.Features.Orders.Queries;
namespace BongoEcom.Components.Pages.Web.CheckOut;

public partial class TrackOrderComponent
{
    string OrderNo = "";
    OrderDto? _order;
    List<OrderHistoryDto> orderHistories = new();
    private async void GetOrder()
    {
        long.TryParse(OrderNo, out long parsed);
        var query = new GetOrderDetailsByOrderNoQuery(parsed);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            _order = result.Data;
        }
    }

    bool init = true;
    bool loading = false;
    private async void GetOrderHistory()
    {
        long.TryParse(OrderNo, out long parsed);
        if (parsed == 0 || loading) return;

        init = false;
        loading = true;

        UIService.ShowLoader();
        var query = new GetOrderTrackHistoryQuery(parsed);
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            orderHistories = result.Data ?? new();
        }
        loading = false;
        UIService.HideLoader();
        StateHasChanged();
    }
}
