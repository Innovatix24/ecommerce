

namespace Application.Features.Categories.DTOs;

public class CategoryDto
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InputType { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
