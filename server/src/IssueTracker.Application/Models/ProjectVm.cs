using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public class ProjectVm
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string OwnedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public ProjectProgress Progress { get; set; }
    public IEnumerable<IssueVm> Issues { get; set; }
}

