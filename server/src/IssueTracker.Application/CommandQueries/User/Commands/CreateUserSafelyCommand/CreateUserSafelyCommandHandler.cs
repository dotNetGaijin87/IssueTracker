using AutoMapper;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Users.Commands.CreateUserSafelyCommand;

public class CreateUserSafelyCommandHandler : IRequestHandler<CreateUserSafelyCommand, UserVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CreateUserSafelyCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserVm> Handle(CreateUserSafelyCommand cmd, CancellationToken ct)
    {
        try
        {
            User currentUser = await _appDbContext.Users
                .SingleOrDefaultAsync(x => x.Id == cmd.UserCredentials.Name);

            var newUser = new User();
            if (currentUser is not null)
            {
                newUser = currentUser;
                _appDbContext.Users.Update(newUser);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                newUser = new User
                {
                    Id          = cmd.UserCredentials.Name,
                    IsActivated = cmd.UserCredentials.Role == UserRole.admin,
                    Role        = cmd.UserCredentials.Role,
                    Email       = cmd.UserCredentials.Email,
                };
                _appDbContext.Users.Add(newUser);
                await _appDbContext.SaveChangesAsync();
            }


            return _mapper.Map<User, UserVm>(newUser);
        }
        catch (Exception ex)
        {
            throw new CreateUserSafelyCommandException($"Creating user {cmd.UserCredentials.Name} error.", ex);
        }
    }
}