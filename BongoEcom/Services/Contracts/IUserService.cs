using Microsoft.JSInterop;
using System.Security.Claims;

namespace BongoEcom.Services.Contracts;

public interface IUserService
{
    //string GetUserId();
    Task<string> GetUserIdAsync();
}

public class UserService : IUserService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor)
    {
        _jsRuntime = jsRuntime;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetUserIdAsync()
    {
        try
        {
            var id = string.Empty;
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.Identity.Name
                    ?? "authenticated";
                return id;
            }

            id = await _jsRuntime.InvokeAsync<string>("getBrowserId");
            return id;
        }
        catch(Exception ex)
        {
            return "anonymous";
        }
    }

    public string GetUserId()
    {
        return GetUserIdAsync().GetAwaiter().GetResult();
    }
}