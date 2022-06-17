using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;

public class DeleteIssueCommand : RequestBase, IRequest<Unit>
{
    public string Id { get; set; }
}
