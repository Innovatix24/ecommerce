
namespace Domain.Entities.Products;

public class ProductSpecification : Entity<int>
{
    public short ProductId { get; set; }
    public Product Product { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}