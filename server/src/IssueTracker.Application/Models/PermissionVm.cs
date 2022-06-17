using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public class PermissionVm
{
    public string UserId { get; set; }

    public string IssueId { get; set; }
    public bool IsPinnedToKanban { get; set; }
    public int KanbanRowPosition { get; set; }

    public IssuePermission IssuePermission { get; set; } = IssuePermission.None;
}