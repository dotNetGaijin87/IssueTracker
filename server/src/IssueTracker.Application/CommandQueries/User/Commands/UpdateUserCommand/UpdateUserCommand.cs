using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;

public class UpdateUserCommand : RequestBase, IRequest<UserVm>
{
    public string Id { get; set; }
    public bool IsActivated { get; set; }

    public List<string> FieldMask { get; set; } = new List<string>();
}
