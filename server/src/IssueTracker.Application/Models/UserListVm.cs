namespace IssueTracker.Application.Models;

public class UserListVm
{
    public int PageCount { get; set; }

    public int Page { get; set; } = 1;

    public List<UserVm> Users { get; set; } = new List<UserVm>();
}