using IssueTracker.Application.Models;
using MediatR;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentCommand : RequestBase, IRequest<Unit>
{
    public string Id { get; set; }
}
