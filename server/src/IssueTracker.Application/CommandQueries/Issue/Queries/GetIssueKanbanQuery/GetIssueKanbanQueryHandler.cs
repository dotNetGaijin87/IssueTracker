using AutoMapper;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;


public class GetIssueKanbanQueryHandler : IRequestHandler<GetIssueKanbanQuery, IEnumerable<KanbanCardVm>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetIssueKanbanQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<KanbanCardVm>> Handle(GetIssueKanbanQuery query, CancellationToken ct)
    {
        try
        {
            var issues = await _appDbContext.Issues
                .Where(x => x.Permissions
                                .Any(x => x.UserId == query.UserCredentials.Name && x.IsPinnedToKanban == true))
                .Include(x => x.Permissions.Where(x => x.UserId == query.UserCredentials.Name))
                .Include(x => x.Project)
                .ToListAsync();

 
            return issues.Select(x => _mapper.Map<Issue, KanbanCardVm>(x))
                    .ToList();
        }
        catch (Exception ex)
        {
            throw new GetIssueKanbanQueryException($"Getting kanban for user  \"{query.UserCredentials.Name}\" error.", ex);
        }
    }
}
