using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;

public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, ProjectListVm>
{
    private readonly IAppDbContext _appDbContext;

    public ListProjectsQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ProjectListVm> Handle(ListProjectsQuery query, CancellationToken ct)
    {
        try
        {
            var baseQuery = _appDbContext.Projects
                .AsNoTracking()
                .IfPropertyNotNullApplyWhere(query.Id, x => x.Id.StartsWith(query.Id))
                .IfPropertyNotNullApplyWhere(query.CreatedBy, x => x.CreatedBy.StartsWith(query.CreatedBy))
                .IfPropertyNotNullApplyWhere(query.Progress, x => x.Progress == query.Progress);

            int rowCount = baseQuery.Distinct().Count();
            if (rowCount == 0)
            {
                return new ProjectListVm();
            }

            var projects = await baseQuery
                .Select(x => new ProjectVm
                { 
                    Id              = x.Id,
                    Summary         = x.Summary,
                    CreatedBy       = x.CreatedBy,
                    OwnedBy         = x.OwnedBy,
                    Progress        = x.Progress,
                    CreationTime    = x.CreationTime,
                    CompletionTime  = x.CompletionTime,
                })
                .OrderBy(x => x.Id)
                .Skip(query.PageSize * (query.Page - 1))
                .Take(query.PageSize)
                .ToListAsync(ct);

            var projectListVm = new ProjectListVm
            {
                PageCount = (rowCount + query.PageSize - 1)/ query.PageSize,
                Page = query.Page,
                Projects = projects
            };


            return projectListVm;
        }
        catch (Exception ex)
        {
            throw new ListProjectsQueryException($"Listing projects error.", ex);
        }
    }
}