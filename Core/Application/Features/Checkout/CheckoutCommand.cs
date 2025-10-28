

using Application.Features.Attributes.DTOs;
using Domain.Entities;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System.Text.Json.Serialization;

namespace Application.Features.Checkout;
public class CheckoutOrderCommand : IRequest<Result<int>>
{
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public string? Coupon { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal Total { get; set; }
    public decimal Discount { get; set; } = 0;
    public decimal GTotal => Total + DeliveryCharge - Discount;
    public List<CheckoutOrderItemDto> Items { get; set; } = new();
    public short UserId { get; internal set; }
    public short CouponId { get; set; }
}

public class CheckoutOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string AttributeStr { get; set; }
    public string AttributeJson { get; set; }
    public List<ItemAttribute> Attributes { get; set; } = new();
    public int ImageId { get; set; }
    public int SKUId { get; set; }
    public string ImageUrl { get; set; } = "";
}


public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, Result<int>>
{
    private readonly ApplicationDbContext _context;

    public CheckoutOrderHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<int>> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Create customer
            var customer = new Customer
            {
                UserId = request.UserId,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Address
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            var orderCount = _context.Orders.Where(x => x.CreatedAt.Date == DateTime.Now.Date).Count();
            // Create order
            var order = new Order
            {
                OrderNo = GetOrderNo(orderCount),
                CustomerId = customer.Id,
                CreatedAt = DateTime.UtcNow,
                Status = (byte)OrderStatus.Pending,
                Note = request.Note,
                PaymentMethod = (byte)request.PaymentMethod,
                DeliveryCharge = request.DeliveryCharge,
                Total = request.Total,
                Discount = request.Discount,
                CouponId = request.CouponId,
                Items = new List<OrderItem>()
            };

            // Add order items
            foreach (var item in request.Items)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId, cancellationToken);

                if (product == null)
                    return Result<int>.Failure($"Product with ID {item.ProductId} not found.");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    SKUId = item.SKUId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Attributes = item.AttributeJson,
                    ProductImageId = item.ImageId,
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            if(order.CouponId > 0)
            {
                var coupon = await _context.Coupons.FirstOrDefaultAsync(x=> x.Id == order.CouponId);
                if(coupon is not null)
                {
                    coupon.UsageCount++;
                    _context.Coupons.Update(coupon);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            var log = new OrderHistory
            {
                OrderId = order.Id,
                Status = (byte)OrderStatus.Pending,
                Note = "",
                CreatedAt = DateTime.UtcNow
            };
            _context.OrderHistories.Add(log);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Result<int>.Success(order.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result<int>.Failure("Checkout failed: " + ex.Message);
        }
    }

    private long GetOrderNo(int orderCount)
    {
        var dateStr = DateTime.Now.ToString("yyMMdd");
        Int64.TryParse(dateStr, out long result);
        return (result * 10000) + (orderCount + 1);
    }
}
