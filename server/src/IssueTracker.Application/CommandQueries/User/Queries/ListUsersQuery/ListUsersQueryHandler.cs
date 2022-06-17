using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;


public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, UserListVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ListUsersQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserListVm> Handle(ListUsersQuery query, CancellationToken ct)
    {
        try
        {
            var baseQuery = _appDbContext.Users
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .IfPropertyNotNullApplyWhere(query.Id, x => x.Id.StartsWith(query.Id))
                .IfPropertyNotNullApplyWhere(query.Email, x => x.Email.StartsWith(query.Email));

            int rowCount = baseQuery.Count();
            if (rowCount == 0)
            {
                return new UserListVm();
            }

            var users = await baseQuery
                .OrderBy(x => x.Id)
                .Skip(query.PageSize * (query.Page - 1))
                .Take(query.PageSize)
                .ToListAsync(ct);

            var usersVm = new UserListVm
            {
                PageCount = (rowCount / query.PageSize) + 1,
                Page = query.Page,
                Users = users.Select(x => _mapper.Map<User, UserVm>(x)).ToList()
            };


            return usersVm;
        }
        catch (Exception ex)
        {
            throw new ListUsersQueryException("Listing users error.", ex);
        }
    }
}