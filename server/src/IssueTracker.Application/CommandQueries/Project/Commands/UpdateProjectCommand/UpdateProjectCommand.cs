using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;

public class UpdateProjectCommand : RequestBase, IRequest<ProjectVm>
{
    public string Id { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public ProjectProgress Progress { get; set; }

    public List<string> FieldMask { get; set; } = new List<string>();
}
