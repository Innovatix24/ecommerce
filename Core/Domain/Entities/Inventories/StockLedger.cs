
namespace Domain.Entities.Inventories;
public class StockLedger
{
    public int Id { get; set; }
    public int SKUId { get; set; }
    public SKU SKU { get; set; }
    public string ChangeType { get; set; } = string.Empty; // e.g. "SALE", "RETURN", "PURCHASE"
    public int Quantity { get; set; }
    public string? ReferenceId { get; set; } // e.g. OrderId, PurchaseId
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}