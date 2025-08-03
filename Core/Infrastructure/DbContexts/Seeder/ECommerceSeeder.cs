
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts.Seeder;

public static class ECommerceSeeder
{
    public static void SeedECommerceData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveryCharge>().HasData(
            new DeliveryCharge { Id = 1, AreaType = "Inside Dhaka", ChargeAmount = 60, FreeShippingThreshold = 1500 },
            new DeliveryCharge { Id = 2, AreaType = "Outside Dhaka", ChargeAmount = 120, FreeShippingThreshold = 2000 }
        );
    }
}