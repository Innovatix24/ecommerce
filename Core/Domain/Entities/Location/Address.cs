

using Domain.Entities.Auth;

namespace Domain.Entities.Location;

public class Address : Entity<int>
{
    public int UserId { get; set; }
    public string Line1 { get; set; } = string.Empty;
    public string Line2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public User User { get; set; } = default!;
}