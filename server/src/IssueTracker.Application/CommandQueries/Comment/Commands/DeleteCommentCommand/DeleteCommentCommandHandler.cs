using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteCommentCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(DeleteCommentCommand cmd, CancellationToken ct)
    {
        try
        {
            Comment comment = await _appDbContext.Comments
                .SingleAsync(x => x.Id == cmd.Id);

            _appDbContext.Comments.Remove(comment);

            await _appDbContext.SaveChangesAsync();


            return Unit.Value;
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new DeleteCommentCommandException($"Comment \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new DeleteCommentCommandException($"Deleting comment \"{cmd.Id}\" error.", ex);
        }
    }
}
