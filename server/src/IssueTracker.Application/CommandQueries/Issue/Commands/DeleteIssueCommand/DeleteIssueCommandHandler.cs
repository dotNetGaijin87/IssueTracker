using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;


public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, Unit>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteIssueCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Unit> Handle(DeleteIssueCommand cmd, CancellationToken ct)
    {
        try
        {      
            Issue issue = await _appDbContext.Issues.SingleAsync(x => x.Id == cmd.Id, ct);
            _appDbContext.Issues.Remove(issue);
            await _appDbContext.SaveChangesAsync();


            return Unit.Value;
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new DeleteIssueCommandException($"Issue \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new DeleteIssueCommandException($"Deleting issue \"{cmd.Id}\" error.", ex);
        }
    }
}
