using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;

public class GetUserQuery : RequestBase, IRequest<UserVm>
{
    public string Id { get; set; }
}
