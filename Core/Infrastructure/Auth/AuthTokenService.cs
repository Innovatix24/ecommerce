

using Domain.Entities.Auth;

namespace Infrastructure.Auth;

public class AuthTokenService
{
    public string GenerateToken(User user)
    {
        return Guid.NewGuid().ToString();
    }
}