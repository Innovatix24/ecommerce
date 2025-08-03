using Application.Auth.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BongoEcom.Services;

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