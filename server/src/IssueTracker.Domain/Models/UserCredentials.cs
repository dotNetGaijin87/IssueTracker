using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Domain.Models
{
    public record UserCredentials
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; } = UserRole.employee;
    }
}
