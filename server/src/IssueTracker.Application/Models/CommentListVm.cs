namespace IssueTracker.Application.Models;

public record CommentListVm
{
    public int PageCount { get; set; }

    public int Page { get; internal set; } = 1;

    public List<CommentVm> Comments { get; set; }
}