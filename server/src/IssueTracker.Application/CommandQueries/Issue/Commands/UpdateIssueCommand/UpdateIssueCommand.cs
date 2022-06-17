using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;

public class UpdateIssueCommand : RequestBase, IRequest<IssueVm>
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public IssueType Type { get; set; }
    public IssueProgress Progress { get; set; }
    public IssuePriority Priority { get; set; }
    public List<string> FieldMask { get; set; } = new List<string>();
}
