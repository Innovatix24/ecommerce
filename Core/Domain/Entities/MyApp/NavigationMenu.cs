
namespace Domain.Entities.MyApp;
public class NavigationMenu : Entity<short>
{
    public string? Url { get; set; }
    public required string Title { get; set; }
    public required string IconName { get; set; }
    public short ParentId { get; set; }
    public bool IsActive { get; set; } = true;
}