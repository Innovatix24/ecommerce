namespace Domain.Entities.Products.Attributes;

public class Attribute : Entity<short>
{
    public short GroupId { get; set; }
    public AttributeGroup Group { get; set; }
    public string Name { get; set; }
    public bool IsVariantable { get; set; } = true;
    public ICollection<AttributeValue> Values { get; set; } = new List<AttributeValue>();
}
