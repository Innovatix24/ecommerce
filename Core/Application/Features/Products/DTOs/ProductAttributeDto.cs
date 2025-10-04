

namespace Application.Features.Products.DTOs;

public class ProductAttributeDto
{
    public short Id { get; set; }
    public short AttributeId { get; set; }
    public string Name { get; set; } = "";
    public List<ProductAttributeValueDto> Values { get; set; } = new();
}

public class ProductAttributeValueDto
{
    public int Id { get; set; }
    public short AttributeId { get; set; }
    public string Value { get; set; } = "";
    public bool IsSelected { get; set; }
}