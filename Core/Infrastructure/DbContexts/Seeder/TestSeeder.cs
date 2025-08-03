
using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts.Seeder;

public static class TestSeeder
{
    public static void SeedTestData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    Description = "Devices and gadgets",
                },
                new Category
                {
                    Id = 2,
                    Name = "Clothing",
                    Description = "Apparel for all ages",
                },
                new Category
                {
                    Id = 3,
                    Name = "Home & Kitchen",
                    Description = "Household items and appliances",
                },
                new Category
                {
                    Id = 4,
                    Name = "Books",
                    Description = "Physical and digital media",
                },
                new Category
                {
                    Id = 5,
                    Name = "Sports & Outdoors",
                    Description = "Equipment for active lifestyle",
                },
                new Category
                {
                    Id = 6,
                    Name = "Beauty & Personal Care",
                    Description = "Cosmetics and hygiene products",
                },
                new Category
                {
                    Id = 7,
                    Name = "Toys & Games",
                    Description = "Entertainment for all ages",
                },
                new Category
                {
                    Id = 8,
                    Name = "Automotive",
                    Description = "Vehicle parts and accessories",
                },
                new Category
                {
                    Id = 9,
                    Name = "Office Supplies",
                    Description = "Workplace essentials",
                },
                new Category
                {
                    Id = 10,
                    Name = "Groceries",
                    Description = "Food and household consumables",
                }
            );
    }
}