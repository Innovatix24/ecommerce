using Application.Auth.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BongoEcom.Services;
public class AuthUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool Authenticated { get; set; }
    public short UserId { get; set; }
}

public interface IAuthUserService
{
    AuthUser? User { get; set; }
    bool Authenticated { get; set; }
    void LoadUserAuth();
}

public class AuthUserService : IAuthUserService
{
    public AuthUser? User { get; set; }
    public bool Authenticated { get; set; }

    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthUserService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async void LoadUserAuth()
    {
        var state = await _authStateProvider.GetAuthenticationStateAsync();
        var user = state.User;
        var userAuthInfo = new AuthUser
        {
            Authenticated = user.Identity?.IsAuthenticated ?? false
        };

        if (userAuthInfo.Authenticated)
        {
            userAuthInfo.Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            userAuthInfo.UserName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userIdStr = user.FindFirst("UserId")?.Value;
            short.TryParse(userIdStr, out short userId);
            userAuthInfo.UserId = userId;
            userAuthInfo.Email = user.FindFirst(ClaimTypes.Email)?.Value;
        }

        User =  userAuthInfo;
    }
}


public class AuthService
{
    public event Action OnChange;
    public UserDto? AuthUser { get; set; }
    public UserSession? UserSession { get; set; }

    private readonly ProtectedSessionStorage _sessionStorage;

    public AuthService(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task LoadAuthSession()
    {
        try
        {
            var userSession = await _sessionStorage.GetAsync<UserSession>("UserSession");
            var session = userSession.Success ? userSession.Value : null;

            if (session == null)
            {
                UserSession = null;
            }
            else
            {
                UserSession = session;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public async void Login(UserDto? user)
    {
        AuthUser = user;

        var userSession = new UserSession
        {
            UserId = user.Id,
            UserName = user.UserName,
            Role = user.Role
        };

        await _sessionStorage.SetAsync("UserSession", userSession);
        await LoadAuthSession();

        NotifyStateChanged();
    }

    public bool Authenticated()
    {
        return AuthUser != null;
    }
    public async Task<bool> IsAdmin()
    {
        await LoadAuthSession();
        return UserSession != null && UserSession.Role == "Admin";
    }

    public async void Logout()
    {
        await _sessionStorage.DeleteAsync("UserSession");
        AuthUser = null;
        LoadAuthSession();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
public class UserSession
{
    public short UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}