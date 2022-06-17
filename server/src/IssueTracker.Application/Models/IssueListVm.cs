namespace IssueTracker.Application.Models;

public record IssueListVm
{
    public int PageCount { get; set; }

    public int Page { get; internal set; } = 1;

    public List<IssueVm> Issues { get; set; } = new List<IssueVm>();
}