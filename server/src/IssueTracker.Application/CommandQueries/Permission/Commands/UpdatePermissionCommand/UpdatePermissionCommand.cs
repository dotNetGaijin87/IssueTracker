using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;

public class UpdatePermissionCommand : RequestBase, IRequest<PermissionVm>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
    public bool IsPinnedToKanban { get; set; }
    public IssuePermission IssuePermission { get; set; }
}
