using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserVm> Handle(GetUserQuery query, CancellationToken ct)
    {
        try
        {
            User user = await _appDbContext.Users
                .AsNoTracking()
                .SingleAsync(x => x.Id == query.Id);


            return _mapper.Map<User, UserVm>(user);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new GetUserQueryException($"User \"{query.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new GetUserQueryException($"Getting user \"{query.Id}\" data error.", ex);
        }
    }
}