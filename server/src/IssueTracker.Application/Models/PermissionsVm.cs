namespace IssueTracker.Application.Models;

public record PermissionsVm
{
    public int PageCount { get; set; }

    public int Page { get; internal set; } = 1;

    public List<PermissionVm> Permissions { get; set; }
}