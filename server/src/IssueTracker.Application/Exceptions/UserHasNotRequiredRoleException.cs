using System.Net;

namespace IssueTracker.Application.Shared.Exceptions;

public class UserHasNotRequiredRoleException : ExceptionWithStatusCode
{
    public UserHasNotRequiredRoleException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.Forbidden;
    }
}