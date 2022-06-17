using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;

public class ListIssuesQueryHandler : IRequestHandler<ListIssuesQuery, IssueListVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ListIssuesQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IssueListVm> Handle(ListIssuesQuery query, CancellationToken ct)
    {
        try
        {
            var baseQuery = _appDbContext.Issues
                .AsNoTracking()
                .IfPropertyNotNullApplyWhere(query.ProjectId, x => x.Project.Id == query.ProjectId)
                .IfPropertyNotNullApplyWhere(query.OwnerId, x => x.Permissions.Any(x => x.UserId == query.OwnerId))
                .IfPropertyNotNullApplyWhere(query.Id, x => x.Id == query.Id)
                .IfPropertyNotNullApplyWhere(query.Type, x => x.Type == query.Type)
                .IfPropertyNotNullApplyWhere(query.Progress, x => x.Progress == query.Progress)
                .IfPropertyNotNullApplyWhere(query.Priority, x => x.Priority == query.Priority);
 
            int rowCount = baseQuery.Distinct().Count();
            if(rowCount == 0)
            {
               return new IssueListVm();
            }

            var issues = await baseQuery
                .OrderBy(x => x.Id)
                .Skip(query.PageSize * (query.Page - 1))
                .Take(query.PageSize)
                .Include(x => x.Project)
                .ToListAsync(ct);

            var issueListVm = new IssueListVm
            {
                PageCount = (rowCount / query.PageSize) + 1,
                Page = query.Page,
                Issues = issues.Select(x => _mapper.Map<Issue, IssueVm>(x)).ToList(), 
            };


            return issueListVm;
        }
        catch (Exception ex)
        {
            throw new ListIssuesQueryException($"Listing issues error.", ex);
        }
    }
}