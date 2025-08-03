
namespace Domain.Entities.Products;

public class ProductImage : Entity<int>
{
    public string Url { get; set; } = "";
    public string Tag { get; set; } = "";
    public byte DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public short ProductId { get; set; }
    public Product Product { get; set; }
}
