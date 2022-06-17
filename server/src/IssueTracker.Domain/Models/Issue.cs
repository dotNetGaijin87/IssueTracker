using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Domain.Models;

public record Issue
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public IssueType Type { get; set; } = IssueType.Bug;
    public IssueProgress Progress { get; set; } = IssueProgress.OnHold;
    public IssuePriority Priority { get; set; } = IssuePriority.Low;
    public string CreatedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public string ProjectId { get; set; }
    public Project Project { get; set; }
    public IList<Permission> Permissions { get; set; } = new List<Permission>();
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
}
