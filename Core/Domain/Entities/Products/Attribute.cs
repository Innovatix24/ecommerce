

namespace Domain.Entities.Products;

public class Attribute : Entity<short>
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public string InputType { get; set; }
    public bool IsVariantable { get; set; } = true;
    public ICollection<AttributeValue> Values { get; set; } = new List<AttributeValue>();
}
