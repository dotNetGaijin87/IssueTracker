using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;

public class CreateIssueCommand : RequestBase, IRequest<IssueVm>
{
    public string Id { get; set; }
    public string ProjectId { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public List<string> ResponsibleBy { get; set; } = new List<string>();
    public IssueType Type { get; set; }
    public IssueProgress Progress { get; set; }
    public IssuePriority Priority { get; set; }
}
