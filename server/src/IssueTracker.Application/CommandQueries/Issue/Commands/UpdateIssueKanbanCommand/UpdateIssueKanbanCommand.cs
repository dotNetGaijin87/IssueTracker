using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;

public class UpdateIssueKanbanCommand : RequestBase, IRequest<IEnumerable<KanbanCardVm>>
{
    public string IssueId { get; set; }
    public string UserId { get; set; }
    public bool IsPinnedToKanban { get; set; } = true;
    public List<Permission> Permissions { get; set; } = new List<Permission>();
    public IssueProgress Progress { get; set; }
    public IssuePermission IssuePermission { get; set; }
}
