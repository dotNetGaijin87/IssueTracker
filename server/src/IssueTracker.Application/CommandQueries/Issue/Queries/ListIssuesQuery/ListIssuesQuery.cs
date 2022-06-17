using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;

[Authorize]
public class ListIssuesQuery : RequestBase, IRequest<IssueListVm>
{
    public string ProjectId { get; set; }
    public string Id { get; set; }
    public string OwnerId { get; set; }
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 1;
    public string CreatedBy { get; set; }
    public IssueType? Type { get; set; }
    public IssueProgress? Progress { get; set; }
    public IssuePriority? Priority { get; set; }
}
