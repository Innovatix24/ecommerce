

using Domain.Entities.Products;

namespace Domain.Entities.Inventories;

public class SKU : Entity<int>
{
    public short ProductId { get; set; }
    public string SKUCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int StockQuantity { get; set; }
    public int? ReorderLevel { get; set; }
    public string? Barcode { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<SKUAttributeValue> AttributeValues { get; set; } = new List<SKUAttributeValue>();
    public ICollection<StockLedger> StockLedgers { get; set; } = new List<StockLedger>();
}


public class SKUAttributeValue
{
    public int Id { get; set; }
    public int SKUId { get; set; }
    public SKU SKU { get; set; }
    public int AttributeId { get; set; }
    public int AttributeValueId { get; set; }
}