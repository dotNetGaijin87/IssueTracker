using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public class UserProjectVm
{
    public string Id { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public ProjectProgress ProjectProgress { get; set; } = ProjectProgress.Open;
    public UserProjectClaim Permission { get; set; } = UserProjectClaim.None;
}