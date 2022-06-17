using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CreatePermissionCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<PermissionVm> Handle(CreatePermissionCommand cmd, CancellationToken ct)
    {
        try
        {
            Permission permission = _mapper.Map<CreatePermissionCommand, Permission>(cmd);
            _appDbContext.Permissions.Add(permission);
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Permission, PermissionVm>(permission);
        }
        catch (Exception ex) when (ex.InnerException.IsDuplicateKeyConstraintViolationError())
        {
            throw new CreatePermissionCommandException($"Permission with the same ids \"{cmd.UserId}\" \"{cmd.IssueId}\" already exists.", ex, HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            throw new CreatePermissionCommandException($"Creating permission for the user \"{cmd.UserId}\" for issue \"{cmd.IssueId}\" error.", ex);
        }
    }
}
