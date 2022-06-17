using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Models;

public class RequestBase
{
    public UserCredentials UserCredentials { get; set; } = new UserCredentials();
}
