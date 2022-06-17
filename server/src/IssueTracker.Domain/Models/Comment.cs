namespace IssueTracker.Domain.Models;

public record Comment
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Content { get; set; }
    public DateTime CreationTime { get; set; }
    public string IssueId { get; set; }
    public Issue Issue { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
