
using Application.Features.Orders.Queries;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Dashboard.Queries;

public record GetRecentOrdersQuery : IRequest<Result<List<OrderDto>>>;

internal class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, Result<List<OrderDto>>>
{
    IDbContextFactory<ApplicationDbContext> _contextFactory;
    public GetRecentOrdersQueryHandler(IDbContextFactory<ApplicationDbContext> factory)
    {
        _contextFactory = factory;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
    {
        var _context = _contextFactory.CreateDbContext();
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .Select(order => new OrderDto
            {
                Id = order.Id,
                OrderNo= order.OrderNo,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                SubTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice),
                CustomerName = order.Customer.FullName,
                CustomerPhone = order.Customer.PhoneNumber ?? "",
                CustomerEmail = order.Customer.Email ?? "",
                DeliveryAddress = order.ShippingAddress,
                DeliveryCharge = order.DeliveryCharge,
                Note = order.Note,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return Result<List<OrderDto>>.Success(orders);
    }
}
