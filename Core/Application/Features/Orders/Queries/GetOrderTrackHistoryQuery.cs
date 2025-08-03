

using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Application.Features.Orders.Queries;

public class OrderHistoryDto
{
    public int Id { get; set; } 
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte Status { get; set; }
    public OrderStatus StatusEnum => (OrderStatus)Status;

    public string GetNote()
    {
        if (StatusEnum == OrderStatus.Pending)
        {
            return "Your order has been received";
        }
        else if (StatusEnum == OrderStatus.Processing)
        {
            return "Your order is being processed";
        }
        else if (StatusEnum == OrderStatus.Shipped)
        {
            return "Your order is has been shipped";
        }
        else if (StatusEnum == OrderStatus.Delivered)
        {
            return "Your order is has been Delivered";
        }
        else if (StatusEnum == OrderStatus.Cancelled)
        {
            return "Your order is has been Cancelled";
        }
        else if (StatusEnum == OrderStatus.Returned)
        {
            return "Your order is has been Returned";
        }
        return "";
    }
}

public record GetOrderTrackHistoryQuery(long OrderNo) : IRequest<Result<List<OrderHistoryDto>>>;


public class GetOrderTrackHistoryQueryHandler : IRequestHandler<GetOrderTrackHistoryQuery, Result<List<OrderHistoryDto>>>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public GetOrderTrackHistoryQueryHandler(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    } 

    public async Task<Result<List<OrderHistoryDto>>> Handle(GetOrderTrackHistoryQuery request, CancellationToken cancellationToken)
    {
        var _context = await _contextFactory.CreateDbContextAsync();
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo);

        if (order is null)
        {
            return Result<List<OrderHistoryDto>>.Failure("No order found");
        }

        var orders = await _context.OrderHistories
            .Where(x => x.OrderId == order.Id)
            .Select(order => new OrderHistoryDto
            {
                Id = order.Id,
                Note = order.Note,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
            })
            .ToListAsync(cancellationToken);

        return Result<List<OrderHistoryDto>>.Success(orders);
    }
}
