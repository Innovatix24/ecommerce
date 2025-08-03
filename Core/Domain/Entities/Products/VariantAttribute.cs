
namespace Domain.Entities.Products;
public class VariantAttribute : Entity<int>
{
    public short VariantId { get; set; } 
    public string AttributeName { get; set; } = "";
    public string AttributeValue { get; set; } = "";
}
