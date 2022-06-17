namespace IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;

public class ListCommentsQueryException : Exception
{
    public ListCommentsQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

