using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;

[Authorize]
public class ListPermissionsQuery : RequestBase, IRequest<PermissionsVm>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 1;
}
