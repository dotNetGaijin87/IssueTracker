using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;

public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery, PermissionVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetPermissionQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PermissionVm> Handle(GetPermissionQuery query, CancellationToken ct)
    {
        try
        {
            Permission permission =  await _appDbContext.Permissions
                .SingleAsync(x => x.UserId == query.UserId && x.IssueId == query.IssueId);


            return _mapper.Map<Permission, PermissionVm>(permission);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new GetPermissionQueryException($"Permission not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new GetPermissionQueryException($"Getting permission for the user \"{query.UserId}\" for issue \"{query.IssueId}\" error.", ex) ;
        }
    }
}