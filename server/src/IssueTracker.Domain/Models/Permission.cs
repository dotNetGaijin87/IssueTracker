using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Domain.Models;

public record Permission
{
    public string UserId { get; set; }
    public User User { get; set; }
    public string IssueId { get; set; }
    public Issue Issue { get; set; }
    public bool IsPinnedToKanban { get; set; }
    public int KanbanRowPosition { get; set; }
    public string GrantedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public IssuePermission IssuePermission { get; set; } = IssuePermission.None;
    public static Permission CreateWithCanModifyPermission(string issueId, string grantedTo,string grantedBy)
    {
        return GetPermission(issueId, grantedTo, grantedBy, IssuePermission.CanModify);
    }

    public static Permission CreateWithCanDeletePermission(string issueId, string grantedTo, string grantedBy)
    {
        return GetPermission(issueId, grantedTo, grantedBy, IssuePermission.CanDelete);
    }

    private static Permission GetPermission(string issueId, string grantedTo, string grantedBy, IssuePermission permission)
    {
        return new Permission
        {
            IssueId = issueId,
            UserId = grantedTo,
            GrantedBy = grantedBy,
            IssuePermission = permission,
            IsPinnedToKanban = true         
        };
    }
}