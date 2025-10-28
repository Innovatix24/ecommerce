
using BongoEcom;
using BongoEcom.Components;
using BongoEcom.Services.Contracts;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
//{
//    FontManager.RegisterFont(
//        File.OpenRead("/usr/share/fonts/truetype/siyam-rupali/SiyamRupali.ttf")
//    );
//}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

//builder.Host.UseSerilog();

builder.AddBongoEcom()
    .AddInfrastructure();

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BongoEcom_";
});

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await IdentitySeeder.SeedSuperAdminAsync(services);
//}

app.Run();
