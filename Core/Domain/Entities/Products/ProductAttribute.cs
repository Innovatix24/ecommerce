
namespace Domain.Entities.Products;

public class ProductAttribute : Entity<short>
{
    public short ProductId { get; set; }
    public short AttributeId { get; set; }
    public Attribute Attribute { get; set; }
    public List<ProductAttributeValue> Values { get; set; } = new();
}

public class ProductAttributeValue : Entity<int>
{
    public short ProductAttributeId { get; set; }
    public ProductAttribute ProductAttribute { get; set; } = default!;
    public string Value { get; set; } = string.Empty;
}
