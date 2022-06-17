using IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Models;

public record UserVm
{
    public string Id { get; set; }
    public bool IsActivated { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    public UserRole Role { get; set; }
    public DateTime RegisteredOn { get; set; }
    public DateTime LastLoggedOn { get; set; }
    public IList<UserProjectVm> Projects { get; set; }
    public IList<Permission> Issues { get; set; }
    public IEnumerable<Comment> Posts { get; set; }
}