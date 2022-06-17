using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;

public class GetIssueQueryHandler : IRequestHandler<GetIssueQuery, IssueVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetIssueQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IssueVm> Handle(GetIssueQuery query, CancellationToken ct)
    {
        try
        {
            Issue issue =  await _appDbContext.Issues
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .Include(x => x.Permissions.Where(x => x.UserId == query.UserCredentials.Name))
                .SingleAsync();


            return _mapper.Map<Issue, IssueVm>(issue);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new GetIssueQueryException($"Issue \"{query.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new GetIssueQueryException($"Getting issue \"{query.Id}\" data error.", ex) ;
        }
    }
}