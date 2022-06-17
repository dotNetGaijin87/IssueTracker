namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;

public class GetIssueKanbanQueryException : Exception
{
    public GetIssueKanbanQueryException(string message, Exception innerException)
        : base(message, innerException)
    {  }
}

