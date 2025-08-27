using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Site;

public class Setting
{
    [Key]
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string TValue { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
