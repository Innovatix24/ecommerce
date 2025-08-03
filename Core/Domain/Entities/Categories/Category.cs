using Domain.Entities.Products;

namespace Domain.Entities.Categories;

public class Category : Entity<short>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public short ParentId { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

