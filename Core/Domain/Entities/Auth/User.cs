namespace Domain.Entities.Auth;

public class User : Entity<short>
{
    public required string FullName { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public required string PasswordHash { get; set; }
    public byte RoleId { get; set; }
    public Role Role { get; set; }
}