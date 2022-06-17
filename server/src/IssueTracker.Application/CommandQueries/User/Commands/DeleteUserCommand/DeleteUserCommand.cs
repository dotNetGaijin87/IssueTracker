using IssueTracker.Application.Attributes;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;

public class DeleteUserCommand : RequestBase,IRequest<Unit>
{
    public string Id { get; set; }
}
