namespace IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;

public class ListProjectsQueryException : Exception
{
    public ListProjectsQueryException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

