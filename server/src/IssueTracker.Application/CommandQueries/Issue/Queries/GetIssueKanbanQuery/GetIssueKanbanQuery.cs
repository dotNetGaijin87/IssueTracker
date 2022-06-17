using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;

public class GetIssueKanbanQuery : RequestBase, IRequest<IEnumerable<KanbanCardVm>>
{
}
