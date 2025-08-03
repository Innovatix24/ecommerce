

using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public short UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Url { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte NotificationTypeValue { get; set; }

    [NotMapped]
    public NotificationType NotificationType
    {
        get => (NotificationType)NotificationTypeValue;
        set => NotificationTypeValue = (byte)value;
    }
}