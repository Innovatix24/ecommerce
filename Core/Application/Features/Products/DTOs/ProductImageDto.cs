
namespace Application.Features.Products.DTOs;

public class ProductImageDto
{
    public int Id { get; set; }
    public string Url { get; set; } = "";
    public string Tag { get; set; } = "";
    public byte DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public object ProductId { get; internal set; }
}
