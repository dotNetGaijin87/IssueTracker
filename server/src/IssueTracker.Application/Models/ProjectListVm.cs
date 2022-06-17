namespace IssueTracker.Application.Models;

public record ProjectListVm
{
    public int PageCount { get; set; }

    public int Page { get; internal set; } = 1;

    public List<ProjectVm> Projects { get; set; } = new List<ProjectVm>();
}