using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Domain.Models;

public record Project 
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public string OwnedBy { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public ProjectProgress Progress { get; set; } = ProjectProgress.Open;
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
