using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Users.Commands.CreateUserSafelyCommand;

public class CreateUserSafelyCommand : RequestBase, IRequest<UserVm>
{
}
