
using Application.Features.Attributes.DTOs;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Enums;

namespace Application.Features.Orders.Queries;

public record GetOrderDetailsByIdQuery(int OrderId) : IRequest<Result<OrderDto>>;

public class CustomerDto
{
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public short UserId { get; set; }
}
public class OrderDto
{
    public int Id { get; set; }
    public long OrderNo { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte Status { get; set; }
    public string StatusStr => ((OrderStatus)Status).ToString();
    public decimal SubTotal { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount => SubTotal + DeliveryCharge - Discount;
    public string TotalStr => TotalAmount.ToString("0.00 Tk");
    public string CustomerName { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
    public string PaymentMethodStr => PaymentMethod.ToString();
    public PaymentMethod PaymentMethod { get; set; }
    public string CustomerEmail { get; set; } = default!;
    public string DeliveryAddress { get; set; } = default!;
    public string? Note { get; set; }
    public CustomerDto Customer { get; set; } = new();
    public CouponDto? Coupon { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string Attributes { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string AttributeHtml { 
        get
        {
            var str = "";
            var items = JsonConvert.DeserializeObject<List<ItemAttribute>>(Attributes);
            if (items is null) return str;

            foreach (var item in items)
            {
                str += "<b>" + item.Key + "</b> : ";
                str += item.Value + ", ";
            }
            str = str.Trim();
            str = str.Trim(',');
            return str;
        } 
    }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;

    public string ProductCode { get; internal set; }
}


public class GetOrderDetailsByIdQueryHandler : IRequestHandler<GetOrderDetailsByIdQuery, Result<OrderDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOrderDetailsByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(x => x.Product)
             .Include(o => o.Items)
                .ThenInclude(item => item.ProductImage)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                return Result<OrderDto>.Failure($"Order with ID {request.OrderId} not found.");

            var coupon = await _context.Coupons.FirstOrDefaultAsync(x=> x.Id == order.CouponId);

            var dto = new OrderDto
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Discount = order.Discount,
                SubTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice),
                CustomerName = order.Customer.FullName,
                CustomerPhone = order.Customer.PhoneNumber ?? "",
                CustomerEmail = order.Customer.Email ?? "",
                DeliveryAddress = order.Customer.Address ?? "",
                DeliveryCharge = order.DeliveryCharge,
                PaymentMethod = (PaymentMethod)order.PaymentMethod,
                Note = order.Note,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductCode = i.Product.Code,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Attributes = i.Attributes,
                    ImageUrl = i.ProductImage.Url,
                }).ToList()
            };

            if(coupon is not null)
            {
                dto.Coupon = new CouponDto
                {
                    Id = coupon.Id,
                    Code = coupon.Code,
                    DiscountType = (DiscountType)coupon.DiscountType,
                    DiscountValue = coupon.DiscountValue,
                };
            }

            return Result<OrderDto>.Success(dto);
        }
        catch(Exception ex)
        {
            var item = new OrderDto();
            return Result<OrderDto>.Success(item); ;
        }
    }
}


public record GetOrderDetailsByOrderNoQuery(long OrderNo) : IRequest<Result<OrderDto>>;

public class GetOrderDetailsByOrderNoQueryHandler : IRequestHandler<GetOrderDetailsByOrderNoQuery, Result<OrderDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOrderDetailsByOrderNoQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderDetailsByOrderNoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(x => x.Product)
             .Include(o => o.Items)
                .ThenInclude(item => item.ProductImage)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderNo == request.OrderNo, cancellationToken);

            if (order == null)
                return Result<OrderDto>.Failure($"Order with No {request.OrderNo} not found.");

            var dto = new OrderDto
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
                    ProductCode = i.Product.Code,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Attributes = i.Attributes,
                    ImageUrl = i.ProductImage.Url,
                }).ToList()
            };

            return Result<OrderDto>.Success(dto);
        }
        catch (Exception ex)
        {
            var item = new OrderDto();
            return Result<OrderDto>.Success(item); ;
        }
    }
}
