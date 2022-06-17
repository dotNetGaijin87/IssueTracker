using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;

public class GetCommentQuery : RequestBase, IRequest<CommentVm>
{
    public string Id { get; set; }
}
