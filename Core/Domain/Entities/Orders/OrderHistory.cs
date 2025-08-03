

namespace Domain.Entities.Orders;

public class OrderHistory : Entity<int>
{
    public string Note { get; set; } = "";
    public byte Status { get; set; }
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}