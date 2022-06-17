using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Domain.Models;

public record User
{
    public string Id { get; set; }
    public bool IsActivated { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    public UserRole Role { get; set; }
    public DateTime RegisteredOn { get; set; }
    public DateTime LastLoggedOn { get; set; }
    public IList<Permission> Issues { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
}
