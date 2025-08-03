using Application;
using Blazr.RenderState.Server;
using BongoEcom.Components.Account;
using BongoEcom.Services;
using Infrastructure.Auth;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BongoEcom;

public static class RegisterServices
{
    public static WebApplicationBuilder AddBongoEcom(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.AddBlazrRenderStateServerServices();
        builder.Services.AddFluentUIComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
        });
        builder.Services.AddScoped<AuthTokenService>();
        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<CompanyInfoService>();
        builder.Services.AddScoped<UIHelperService>();
        builder.Services.AddScoped<SweetAlertService>();
        builder.Services.AddScoped<ProductFilterService>();
        builder.Services.AddScoped<CategoryStateService>();
        builder.Services.AddScoped<AuthService>();
        
        return builder;
    }
}
