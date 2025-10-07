
namespace Application.Features.Attributes.DTOs;

public class AttributeDto
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string InputType { get; set; } = string.Empty;
    public short GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public List<AttributeValueDto> Values { get; set; } = new();
}

public class AttributeValueDto
{
    public short Id { get; set; }
    public string Value { get; set; }
    public short DisplayOrder { get; set; }
    public bool IsSelected { get; set; }
}
