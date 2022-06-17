using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, PermissionVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdatePermissionCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PermissionVm> Handle(UpdatePermissionCommand cmd, CancellationToken ct)
    {
        try
        {
            Permission permission = await _appDbContext.Permissions
                .SingleAsync(x => x.UserId == cmd.UserId && x.IssueId == cmd.IssueId);

            permission.IssuePermission = cmd.IssuePermission;
            permission.IsPinnedToKanban = cmd.IsPinnedToKanban;
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Permission, PermissionVm>(permission);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new UpdatePermissionCommandException($"Permission not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new UpdatePermissionCommandException($"Updating permission for the user \"{cmd.UserId}\" for issue \"{cmd.IssueId}\" error.", ex);
        }
    }
}
