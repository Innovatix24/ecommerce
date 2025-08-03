

using Shared.Enums;

namespace Application.Features.Orders.DTOs;

public class OrderHistoryDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public List<StatusChangeLogDto> StatusChangeLogs { get; set; } = new();
}
public class StatusChangeLogDto
{
    public DateTime ChangedAt { get; set; }
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string? Note { get; set; }
}