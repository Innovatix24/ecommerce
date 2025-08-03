

namespace Domain.Entities.Products;

public class AttributeValue : Entity<short>
{
    public short AttributeId { get; set; }

    public string Value { get; set; } = string.Empty;

    public string? DisplayValue { get; set; }

    public Attribute Attribute { get; set; } = null!;
}