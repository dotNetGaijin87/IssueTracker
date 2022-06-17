using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;

public class GetPermissionQuery : RequestBase, IRequest<PermissionVm>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
}
