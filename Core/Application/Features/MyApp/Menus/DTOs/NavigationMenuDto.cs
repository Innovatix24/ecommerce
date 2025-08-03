
namespace Application.Features.MyApp.Menus.DTOs;
public class NavigationMenuDto
{
    public required short Id { get; set; }
    public string? Url { get; set; }
    public required string Title { get; set; }
    public required string IconName { get; set; }
    public short ParentId { get; set; }
    public List<NavigationMenuDto> Children { get; set; } = new();

    public NavigationMenuDto? Search(object target, string searchText, List<NavigationMenuDto> parents)
    {
        parents.Add(this);
        if (Title.ToString().ToLower().Contains(searchText))
        {
            return this;
        }
        if (Children is not null)
        {
            foreach (NavigationMenuDto child in Children)
            {
                NavigationMenuDto? result = child.Search(target, searchText, new List<NavigationMenuDto>(parents));
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    public NavigationMenuDto Clone()
    {
        return new NavigationMenuDto
        {
            Id = this.Id,
            Url = this.Url,
            Title = this.Title,
            IconName = this.IconName,
            ParentId = this.ParentId,
            Children = new(),
        };
    }
}
