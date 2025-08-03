

namespace Application.Features.Images.DTOs;
public class ImageDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Tag { get; set; }
}