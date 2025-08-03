
using Domain.Entities.Categories;

namespace Domain.Entities.Products;

public class CategoryAttribute : Entity<short>
{
    public short CategoryId { get; set; }
    public short AttributeId { get; set; }
    public Category Category { get; set; } = null!;
    public Attribute Attribute { get; set; } = null!;
}