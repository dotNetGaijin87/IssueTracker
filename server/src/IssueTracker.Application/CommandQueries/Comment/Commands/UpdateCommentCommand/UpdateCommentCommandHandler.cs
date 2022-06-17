using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<CommentVm> Handle(UpdateCommentCommand cmd, CancellationToken ct)
    {
        try
        {
            Comment comment = await _appDbContext.Comments
                .SingleAsync(x => x.Id == cmd.Id);

            comment.Content = cmd.Content;
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Comment, CommentVm>(comment);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new UpdateCommentCommandException($"Comment \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new UpdateCommentCommandException($"Updating the comment \"{cmd.Id }\" error.", ex);
        }
    }
}
