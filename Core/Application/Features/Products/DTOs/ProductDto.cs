

namespace Application.Features.Products.DTOs;

public class ProductDto
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal RegularPrice { get; set; }
    public decimal SalePrice { get; set; }
    public short CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public bool InStock { get; set; }
    public string StockStatus => InStock ? "In Stock" : "Stock out";
    public string FeatureImageUrl => Images.FirstOrDefault() == null? "" : Images.FirstOrDefault()!.Url;
    public List<ProductImageDto> Images { get; set; } = new();
    public List<ProductSpecificationDto> Specifications { get; set; } = new();
    public List<ProductAttributeDto> Attributes { get; set; } = new();
    public string Code { get; internal set; }
    public string NameBangla { get; set; }
    public bool AddedToCart { get; set; }
    public decimal Discount
    {
        get
        {
            if (RegularPrice == 0 || RegularPrice <= SalePrice) return 0;
            var discount = Math.Round((((RegularPrice - SalePrice) / RegularPrice) * 100), 2);
            return discount;
        }
    }
}
