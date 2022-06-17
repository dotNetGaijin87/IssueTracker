using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;


public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteUserCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(DeleteUserCommand cmd, CancellationToken ct)
    {
        try
        {
            User user = await _appDbContext.Users.SingleAsync(x => x.Id == cmd.Id);
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();


            return Unit.Value;
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new DeleteUserCommandException($"User \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new DeleteUserCommandException($"Deleting user \"{cmd.Id}\" error.", ex);
        }
    }
}