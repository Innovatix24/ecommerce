

using Infrastructure.DbContexts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Infrastructure;

public static class RegisterServices
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        AddDatabase(builder);
        AddIdentity(builder);
        
        return builder;
    }

    private static void AddDatabase(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        }, ServiceLifetime.Scoped);

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString));
    }
    //private static void AddDatabase(WebApplicationBuilder builder)
    //{
    //    var connectionString = builder.Configuration.GetConnectionString("Default")
    //        ?? throw new InvalidOperationException("Connection string 'Default' not found.");

    //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //        options.UseSqlServer(connectionString));

    //    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    //    builder.Services.AddTransient<IDbConnection>(_ => new SqlConnection(connectionString));
    //}
    private static void AddIdentity(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
