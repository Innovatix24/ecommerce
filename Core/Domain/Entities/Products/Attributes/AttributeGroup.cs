
namespace Domain.Entities.Products.Attributes;
public class AttributeGroup : Entity<short>
{
    public string Name { get; set; }
    public List<Attribute> Attributes { get; set; } = new();
}
