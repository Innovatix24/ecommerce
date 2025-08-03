

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries;

public record GetAllOrdersQuery : IRequest<Result<List<OrderDto>>>;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<List<OrderDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetAllOrdersQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Select(order => new OrderDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                SubTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice),
                CustomerName = order.Customer.FullName,
                CustomerPhone = order.Customer.PhoneNumber ?? "",
                CustomerEmail = order.Customer.Email ?? "",
                DeliveryAddress = order.Customer.Address ?? "",
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
