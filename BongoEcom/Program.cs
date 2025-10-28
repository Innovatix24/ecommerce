
using BongoEcom;
using BongoEcom.Components;
using BongoEcom.Services.Contracts;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
//{
//    FontManager.RegisterFont(
//        File.OpenRead("/usr/share/fonts/truetype/siyam-rupali/SiyamRupali.ttf")
//    );
//}

builder.AddBongoEcom()
    .AddInfrastructure();

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "BongoEcom_";
});

//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = "BongoEcom.Session";
//    options.IdleTimeout = TimeSpan.FromHours(2);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//});

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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await IdentitySeeder.SeedSuperAdminAsync(services);
//}

app.Run();
