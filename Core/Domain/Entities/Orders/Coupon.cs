

using Domain.Entities.Auth;
using System;

namespace Domain.Entities.Orders;

public enum DiscountType { Fixed, Percent }

public class Coupon : Entity<short>
{
    public string Code { get; set; } = string.Empty;
    public byte DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal? MinimumPurchaseAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public int UsageCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
}
public class CouponUsage : Entity<int>
{
    public int CouponId { get; set; }
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;
}
