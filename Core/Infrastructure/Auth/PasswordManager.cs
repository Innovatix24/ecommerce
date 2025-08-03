
namespace Infrastructure.Services.Auth;

using System.Security.Cryptography;
using System.Text;

public class PasswordManager
{
    public static bool CheckPassword(string password, string actualPassword)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == actualPassword;
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        for (var i = 0; i < hashBytes.Length; i++)
        {
            builder.Append(hashBytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}