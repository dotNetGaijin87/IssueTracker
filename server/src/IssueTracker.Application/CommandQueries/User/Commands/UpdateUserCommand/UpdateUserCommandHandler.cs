using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using IssueTracker.Application.ExceptionUtils;

namespace IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserVm> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        try
        {
            User user = await _appDbContext.Users
                .SingleAsync(x => x.Id == cmd.Id);

            user = DynamicUtils.UpdateObjectWithFieldMask(cmd, cmd.FieldMask, user);
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<User, UserVm>(user);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new UpdateUserCommandException($"User \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new UpdateUserCommandException($"Updating user \"{cmd.Id}\" error.", ex);
        }
    }
}