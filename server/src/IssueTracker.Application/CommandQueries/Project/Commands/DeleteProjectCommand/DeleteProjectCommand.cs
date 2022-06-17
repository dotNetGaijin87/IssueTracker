using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;

public class DeleteProjectCommand : RequestBase, IRequest<Unit>
{
    public string Id { get; set; }
}
