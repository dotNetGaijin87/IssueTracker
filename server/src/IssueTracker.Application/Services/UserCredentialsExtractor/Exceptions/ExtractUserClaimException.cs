namespace IssueTracker.Application.Services.UserRoleService;

public class ExtractUserClaimException : Exception
{
    public ExtractUserClaimException(string message)
    : base(message)
    {
;
    }
    public ExtractUserClaimException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

