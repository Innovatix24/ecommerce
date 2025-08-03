using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DbContexts.Seeder;

public static class IdentitySeeder
{
    public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1. Define roles
        string[] roles = ["SuperAdmin", "Admin", "Customer"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Create SuperAdmin
        string email = "systemadmin@gmail.com";
        string password = "Innovatix@2025";

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
            var user = new ApplicationUser
            {
                UserId = 1,
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Assign all roles
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
            else
            {
                throw new Exception("Super admin creation failed: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
