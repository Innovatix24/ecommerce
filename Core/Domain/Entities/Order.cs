using Domain.Common;
using Domain.Entities.Products;
using Shared.Enums;

namespace Domain.Entities;

public class Order : AuditableEntity<int> 
{
    public short CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public byte Status { get; set; } = (byte)OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public Customer Customer { get; set; } = default!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public string? Note { get; set; }
    public byte PaymentMethod { get; set; }
    public decimal DeliveryCharge { get; set; }
    public decimal Total { get; set; }
    public long OrderNo { get; set; }
    public decimal Discount { get; set; }
    public short CouponId { get; set; }
}

public class OrderItem : Entity<int>
{
    public int OrderId { get; set; }
    public short ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Order Order { get; set; } = default!;
    public Product Product { get; set; } = default!;
    public ProductImage ProductImage { get; set; } = default!;
    public string Attributes { get; set; } = "";
    public int ProductImageId { get; set; }
}

