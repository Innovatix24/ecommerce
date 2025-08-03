
using Domain.Entities.Categories;

namespace Domain.Entities.Products;

public class Product : Entity<short>
{
    public string Name { get; set; } = string.Empty;
    public string NameBangla { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public decimal RegularPrice { get; set; }
    public int Stock { get; set; }
    public string Tag { get; set; } = "New";
    public short CategoryId { get; set; }
    public bool IsActive { get; set; }
    public bool InStock { get; set; }
    public Category Category { get; set; } = default!;
    ICollection<ProductImage> Images { get; set; }
    ICollection<ProductSpecification> Specifications { get; set; }
}

