using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;


public class ListPermissionsQueryHandler : IRequestHandler<ListPermissionsQuery, PermissionsVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ListPermissionsQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PermissionsVm> Handle(ListPermissionsQuery query, CancellationToken ct)
    {
        try
        {
            var baseQuery = _appDbContext.Permissions
                .AsNoTracking()
                .IfPropertyNotNullApplyWhere(query.UserId, x => x.UserId == query.UserId)
                .IfPropertyNotNullApplyWhere(query.IssueId, x => x.IssueId == query.IssueId);

            int rowCount = baseQuery.Distinct().Count();

            var userIssues = await baseQuery
                .OrderBy(x => x.IssueId)
                .Skip(query.PageSize * (query.Page - 1))
                .Take(query.PageSize)
                .ToListAsync(ct);

            var userIssueListVm = new PermissionsVm
            {
                PageCount = (rowCount / query.PageSize) + 1,
                Page = query.Page,
                Permissions = userIssues.Select(x => _mapper.Map<Permission, PermissionVm>(x)).ToList(),
            };


            return userIssueListVm;
        }
        catch (Exception ex)
        {
            throw new ListPermissionsQueryException($"Listing permissions for user \"{query.UserId}\" for issue \"{query.IssueId}\" error.", ex);
        }
    }
}