
namespace Application.Features.Attributes.DTOs;

public class AttributeDto
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string InputType { get; set; } = string.Empty;
}
