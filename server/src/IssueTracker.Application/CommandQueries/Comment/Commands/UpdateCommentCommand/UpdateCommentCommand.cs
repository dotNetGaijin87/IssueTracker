using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;

public class UpdateCommentCommand : RequestBase, IRequest<CommentVm>
{
    public string Id { get; set; }
    public string Content { get; set; }
}
