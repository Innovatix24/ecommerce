
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.Carts;
using Domain.Entities.Categories;
using Domain.Entities.Location;
using Domain.Entities.MyApp;
using Domain.Entities.Orders;
using Domain.Entities.Products;
using Infrastructure.DbContexts.Seeder;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Attribute = Domain.Entities.Products.Attribute;

namespace Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.SeedApplicationMenu();
        builder.SeedECommerceData();
        builder.SeedTestData();

        builder.Entity<Coupon>()
            .HasIndex(x => x.Code);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.ProductImage)
            .WithMany()
            .HasForeignKey(oi => oi.ProductImageId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<NavigationMenu> NavigationMenus { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CompanyInfo> CompanyInfoes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
    public DbSet<OrderHistory> OrderHistories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
    public DbSet<VisitorLog> VisitorLogs => Set<VisitorLog>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<Upazila> Upazilas => Set<Upazila>();
}
