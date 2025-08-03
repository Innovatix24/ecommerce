

using Shared.Enums;

namespace Application.Features.Orders.DTOs;

public class OrderDto2
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemDto2> Items { get; set; } = new();
}

public class OrderItemDto2
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}