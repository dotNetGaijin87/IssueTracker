using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;

[Authorize]
public class ListProjectsQuery : RequestBase, IRequest<ProjectListVm>
{
    public string Id { get; set; }

    public int PageSize { get; set; } = 10;

    public int Page { get; set; } = 1;

    public string CreatedBy { get; set; }

    public ProjectProgress? Progress { get; set; }
}
