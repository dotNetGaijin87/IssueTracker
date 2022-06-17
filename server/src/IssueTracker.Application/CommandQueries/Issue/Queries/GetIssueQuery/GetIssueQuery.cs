using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;

public class GetIssueQuery : RequestBase, IRequest<IssueVm>
{
    public string Id { get; set; }
}
