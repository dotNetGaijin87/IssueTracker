using AutoMapper;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<CommentVm> Handle(CreateCommentCommand cmd, CancellationToken ct)
    {
        try
        {
            Comment comment = _mapper.Map<CreateCommentCommand, Comment>(cmd);
            _appDbContext.Comments.Add(comment);
            await _appDbContext.SaveChangesAsync();

            Comment commentAfterSave = await _appDbContext.Comments
                .Where(x => x.Id == comment.Id)
                .SingleAsync();


            return _mapper.Map<Comment, CommentVm>(commentAfterSave);
        }
        catch (Exception ex)
        {
            throw new CreateCommentCommandException($"Creating a comment by user \"{cmd.UserCredentials.Name}\" for issue \"{cmd.IssueId}\" error.", ex);
        }
    }
}
