


using Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Customer : Entity<short>
{

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = default!;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public short UserId { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
