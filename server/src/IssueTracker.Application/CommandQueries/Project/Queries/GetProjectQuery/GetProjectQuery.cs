using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;

public class GetProjectQuery : RequestBase, IRequest<ProjectVm>
{
    public string Id { get; set; }
}
