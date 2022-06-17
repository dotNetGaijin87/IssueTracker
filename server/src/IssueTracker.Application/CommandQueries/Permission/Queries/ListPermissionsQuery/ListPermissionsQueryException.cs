namespace IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;

public class ListPermissionsQueryException : Exception
{
    public ListPermissionsQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

