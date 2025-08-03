
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class CompanyInfo : Entity<byte>
{
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = default!;
    public string LogoUrl { get; set; } = default!;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(15)]
    public string? MobileNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }
}
