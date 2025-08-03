

namespace Domain.Entities.Products;

public class ProductVariant : Entity<short>
{
    public short ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Product Product { get; set; } = null!;
    public ICollection<VariantAttribute> Attributes { get; set; } = new List<VariantAttribute>();
}
