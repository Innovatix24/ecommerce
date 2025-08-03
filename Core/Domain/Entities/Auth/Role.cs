
namespace Domain.Entities.Auth;

public class Role : Entity<byte>
{
    public required string Name { get; set; }
    public ICollection<User> Users { get; set; }
}
