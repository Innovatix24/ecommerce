

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace Application.Features.Orders.Commands;

public class CreateOrderCommand : IRequest<Result<int>>
{
    public short CustomerId { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public CreateOrderHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var orderCount = _context.Orders.Where(x=> x.CreatedAt.Date == DateTime.Now.Date).Count();
            var order = new Order
            {
                OrderNo = GetOrderNo(orderCount),
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Status = (byte)OrderStatus.Pending,
                ShippingAddress = request.ShippingAddress,
                Items = new List<OrderItem>()
            };

            foreach (var item in request.Items)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

                if (product == null)
                    return Result<int>.Failure($"Product with ID {item.ProductId} not found.");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result<int>.Success(order.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<int>.Failure("Order could not be created.");
        }
    }

    private long GetOrderNo(int orderCount)
    {
        var dateStr = DateTime.Now.ToString("yyMMdd");
        Int64.TryParse(dateStr, out long result);
        return result + 1000 + orderCount + 1;
    }
}
