using Application.Features.MyApp.Menus.DTOs;

namespace BongoEcom.Components.Layout.Admin;

public partial class NavMenu
{
    static List<NavigationMenuDto> FilterMenu(List<NavigationMenuDto> menus, string searchKey)
    {
        List<NavigationMenuDto> result = new List<NavigationMenuDto>();

        foreach (var menu in menus)
        {
            var filteredMenu = FilterNavigationMenuDto(menu, searchKey);
            if (filteredMenu != null)
            {
                result.Add(filteredMenu);
            }
        }

        return result;
    }

    static NavigationMenuDto? FilterNavigationMenuDto(NavigationMenuDto menu, string searchKey)
    {
        var filteredMenu = menu.Clone();

        foreach (var child in menu.Children)
        {
            var filteredChild = FilterNavigationMenuDto(child, searchKey);
            if (filteredChild != null)
            {
                filteredMenu.Children.Add(filteredChild);
            }
        }

        if (filteredMenu.Children.Count > 0 || menu.Title.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
        {
            return filteredMenu;
        }

        return null;
    }

    static void DisplayMenu(List<NavigationMenuDto> menus, int level = 0)
    {
        foreach (var menu in menus)
        {
            Console.WriteLine(new string(' ', level * 2) + menu.Title);
            DisplayMenu(menu.Children, level + 1);
        }
    }
}

