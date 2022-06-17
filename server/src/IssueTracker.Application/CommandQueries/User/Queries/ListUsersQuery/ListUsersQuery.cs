using IssueTracker.Application.Attributes;
using IssueTracker.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;

[AuthorizeUser]
public class ListUsersQuery : RequestBase, IRequest<UserListVm>
{
    public string Id { get; set; }
    public string Email { get; set; }
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 1;
}
