namespace IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;

public class ListUsersQueryException : Exception
{
    public ListUsersQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

