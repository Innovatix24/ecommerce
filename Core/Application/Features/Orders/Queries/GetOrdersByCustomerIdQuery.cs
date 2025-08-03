
using Microsoft.EntityFrameworkCore;
namespace Application.Features.Orders.Queries;

public class GetOrdersByCustomerIdQuery : IRequest<Result<List<OrderDto>>>
{
    public int CustomerId { get; set; }
}
public class GetOrdersByCustomerIdHandler : IRequestHandler<GetOrdersByCustomerIdQuery, Result<List<OrderDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetOrdersByCustomerIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Where(x => x.CustomerId == request.CustomerId)
            .Select(order => new OrderDto
            {
                Id = order.Id,
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
        catch (Exception)
        {
            return Result<List<OrderDto>>.Failure("Failed to fetch orders.");
        }
    }
}
