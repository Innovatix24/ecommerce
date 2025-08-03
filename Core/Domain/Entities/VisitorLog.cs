
namespace Domain.Entities;

public class VisitorLog : Entity<int>
{
    public string? IPAddress { get; set; }
    public string? PageVisited { get; set; }
    public string? UserAgent { get; set; }
    public DateTime VisitTime { get; set; }
}
