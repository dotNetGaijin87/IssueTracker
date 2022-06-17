using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;

public class CreateProjectCommand : RequestBase, IRequest<ProjectVm>
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string OwnedBy { get; set; }
    public ProjectProgress Progress { get; set; }
}
