using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public class IssueVm
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public IssueType Type { get; set; } = IssueType.Bug;
    public IssueProgress Progress { get; set; } = IssueProgress.OnHold;
    public IssuePriority Priority { get; set; } = IssuePriority.Low;
    public PermissionVm Permission { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public int CommentPageCount { get; set; }
    public IEnumerable<CommentVm> Comments { get; set; } = new List<CommentVm>();
}

