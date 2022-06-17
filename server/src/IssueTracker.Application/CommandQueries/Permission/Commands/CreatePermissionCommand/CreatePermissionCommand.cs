using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;

public class CreatePermissionCommand : RequestBase, IRequest<PermissionVm>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
    public bool IsPinnedToKanban { get; set; } = true;
    public IssuePermission IssuePermission { get; set; }
}
