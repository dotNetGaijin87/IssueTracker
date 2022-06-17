using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;

public class ListCommentsQueryHandler : IRequestHandler<ListCommentsQuery, CommentListVm>
{
    private readonly IAppDbContext _appDbContext;

    public ListCommentsQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CommentListVm> Handle(ListCommentsQuery query, CancellationToken ct)
    {
        try
        {
            var baseQuery = _appDbContext.Comments
                .AsNoTracking()
                .Where(x => x.IssueId == query.IssueId)
                .IfPropertyNotNullApplyWhere(query.CreatedBy, x => x.UserId.StartsWith(query.CreatedBy))
                .IfPropertyNotNullApplyWhere(query.Content, x => x.Content.StartsWith(query.IssueId));

            int rowCount = baseQuery.Distinct().Count();

            var comments = await baseQuery
                .Select(x => new CommentVm
                {
                    Id = x.Id,
                    IssueId = x.IssueId,
                    UserId = x.UserId,
                    CreationTime = x.CreationTime,
                    Content = x.Content,
                })
                .OrderByDescending(x => x.CreationTime)
                .Skip(query.PageSize * (query.Page - 1))
                .Take(query.PageSize)
                .ToListAsync(ct);

            var commentListVm = new CommentListVm
            {
                PageCount = (rowCount + query.PageSize - 1)/ query.PageSize,
                Page = query.Page,
                Comments = comments
            };


            return commentListVm;
        }
        catch (Exception ex)
        {
            throw new ListCommentsQueryException( $"Listing comments for issue \"{query.IssueId}\" error.", ex);
        }
    }
}