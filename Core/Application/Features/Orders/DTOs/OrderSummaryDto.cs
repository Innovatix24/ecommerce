

namespace Application.Features.Orders.DTOs;

public class OrderSummaryDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string DelivaryAddress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public byte Status { get; set; }
}