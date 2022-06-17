using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteProjectCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(DeleteProjectCommand cmd, CancellationToken ct)
    {
        try
        {
            Project project = await _appDbContext.Projects.SingleAsync(x => x.Id == cmd.Id);
            _appDbContext.Projects.Remove(project);
            await _appDbContext.SaveChangesAsync();


            return Unit.Value;
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new DeleteProjectCommandException($"Project \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new DeleteProjectCommandException($"Deleting project \"{cmd.Id}\" error.", ex);
        }
    }
}
