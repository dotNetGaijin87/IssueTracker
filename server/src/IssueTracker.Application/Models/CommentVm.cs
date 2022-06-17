namespace IssueTracker.Application.Models;

public class CommentVm
{
    public string Id { get; set; }
    public string IssueId { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreationTime { get; set; }
}
