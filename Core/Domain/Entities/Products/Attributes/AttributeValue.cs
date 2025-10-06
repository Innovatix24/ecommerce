namespace Domain.Entities.Products.Attributes;

public class AttributeValue : Entity<short>
{
    public short AttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }

    public Attribute Attribute { get; set; } = null!;
}