
namespace Domain.Entities;

public class Image : Entity<int>
{
    public string Url { get; set; } = string.Empty;
    public byte OwnerType { get; set; }
    public int OwnerId { get; set; }
    public string? Tag { get; set; }
}