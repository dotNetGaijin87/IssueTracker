using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;

public class DeletePermissionCommand : RequestBase, IRequest<Unit>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
}
