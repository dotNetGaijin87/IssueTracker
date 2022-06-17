using IssueTracker.Application.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;

public class CreateCommentCommand : RequestBase, IRequest<CommentVm>
{
    public string UserId { get; set; }
    public string IssueId { get; set; }
    public string Content { get; set; }
}
