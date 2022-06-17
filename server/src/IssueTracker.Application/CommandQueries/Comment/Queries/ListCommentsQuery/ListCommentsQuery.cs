using IssueTracker.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;

public class ListCommentsQuery : RequestBase, IRequest<CommentListVm>
{
    public string IssueId { get; set; }

    public int PageSize { get; set; } = 5;

    public int Page { get; set; } = 1;

    public string Content { get; set; }

    public string CreatedBy { get; set; }
}
