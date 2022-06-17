namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;

public class UpdateIssueKanbanCommandException : Exception
{
    public UpdateIssueKanbanCommandException(string message, Exception innerException)
        : base(message, innerException)
    {  }
}

