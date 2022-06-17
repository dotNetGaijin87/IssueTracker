using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;

public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
{
    private readonly IAppDbContext _appDbContext;

    public DeletePermissionCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(DeletePermissionCommand cmd, CancellationToken ct)
    {
        try
        {
            Permission permission = await _appDbContext.Permissions
                .SingleAsync(x => x.UserId == cmd.UserId && x.IssueId == cmd.IssueId);
            _appDbContext.Permissions.Remove(permission);
            await _appDbContext.SaveChangesAsync();


            return Unit.Value;
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new DeletePermissionCommandException($"Permission not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new DeletePermissionCommandException($"Deleting permission for the user \"{cmd.UserId}\" for issue \"{cmd.IssueId}\" error.", ex);
        }
    }
}
