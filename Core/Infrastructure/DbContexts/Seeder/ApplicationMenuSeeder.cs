
using Domain.Entities.MyApp;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts.Seeder;

public static class ApplicationMenuSeeder
{
    public static void SeedApplicationMenu(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NavigationMenu>().HasData(
            new NavigationMenu { Id = 1, Title = "Dashboard", IconName = "Home", Url = "/admin/dashboard", ParentId = 0 },

            new NavigationMenu { Id = 2, Title = "Manage Product", IconName = "Home", Url = null, ParentId = 0 },
            new NavigationMenu { Id = 3, Title = "Categories", IconName = "Home", Url = "/admin/categories", ParentId = 2 },
            new NavigationMenu { Id = 4, Title = "Products", IconName = "Home", Url = "/admin/products", ParentId = 2 },
            new NavigationMenu { Id = 5, Title = "Attributes", IconName = "Home", Url = "/admin/attributes", ParentId = 2 },

            new NavigationMenu { Id = 6, Title = "Sales Settings", IconName = "Home", Url = null, ParentId = 0 },
            new NavigationMenu { Id = 7, Title = "Coupons", IconName = "Home", Url = "/admin/coupons", ParentId = 6 },
            new NavigationMenu { Id = 8, Title = "Delivery Charge", IconName = "Home", Url = "/admin/delivery-charges", ParentId = 6 },

            new NavigationMenu { Id = 9, Title = "Orders", IconName = "Home", Url = "/admin/orders", ParentId = 0 },
            new NavigationMenu { Id = 10, Title = "Customers", IconName = "Home", Url = "/admin/customers", ParentId = 0 },
            new NavigationMenu { Id = 11, Title = "Users", IconName = "Home", Url = "/admin/users", ParentId = 0 },
            new NavigationMenu { Id = 12, Title = "Notifications", IconName = "Home", Url = "/admin/notifications", ParentId = 0 },

            new NavigationMenu { Id = 13, Title = "Site Settings", IconName = "Home", Url = null, ParentId = 0 },
            new NavigationMenu { Id = 14, Title = "Company Info", IconName = "Home", Url = "/admin/company-info", ParentId = 13 },
            new NavigationMenu { Id = 15, Title = "Social Links", IconName = "Home", Url = "/admin/social-links", ParentId = 13 }
        );
    }
}
