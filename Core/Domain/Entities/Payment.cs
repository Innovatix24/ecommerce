
using Domain.Common;

namespace Domain.Entities;

public class Payment : AuditableEntity<int>
{
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } = "CreditCard";
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public Order Order { get; set; } = default!;
}
