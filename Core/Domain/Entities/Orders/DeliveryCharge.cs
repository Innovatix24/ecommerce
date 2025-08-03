
namespace Domain.Entities.Orders;
public enum AreaType
{
    InsideDhaka,
    OutsideDhaka,
    International
}
public class DeliveryCharge : Entity<byte>
{
    public string AreaType { get; set; } 
    public decimal ChargeAmount { get; set; }
    public decimal? FreeShippingThreshold { get; set; }
}