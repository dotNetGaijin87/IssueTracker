using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public class KanbanCardVm
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public int Position { get; set; }
    public string Summary { get; set; }
    public IssueType Type { get; set; } = IssueType.Bug;
    public IssueProgress Progress { get; set; } = IssueProgress.OnHold;
    public IssuePriority Priority { get; set; } = IssuePriority.Low;
}

