using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, CommentVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetCommentQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<CommentVm> Handle(GetCommentQuery query, CancellationToken ct)
    {
        try
        {
            Comment comment =  await _appDbContext.Comments
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .SingleAsync();


            return _mapper.Map<Comment, CommentVm>(comment);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new GetCommentQueryException($"Comment \"{query.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new GetCommentQueryException($"Getting comment \"{query.Id}\" data error.", ex) ;
        }
    }
}