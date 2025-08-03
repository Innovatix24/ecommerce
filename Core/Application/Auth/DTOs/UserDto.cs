

namespace Application.Auth.DTOs;

public class UserDto
{
    public short Id { get; set; } 
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
}
