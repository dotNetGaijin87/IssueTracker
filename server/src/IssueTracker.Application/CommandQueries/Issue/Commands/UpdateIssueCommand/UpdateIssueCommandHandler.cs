using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using IssueTracker.Application.ExceptionUtils;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;


public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, IssueVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdateIssueCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IssueVm> Handle(UpdateIssueCommand cmd, CancellationToken ct)
    {
        try
        {
            Issue issue = await _appDbContext.Issues
                .SingleAsync(x => x.Id  == cmd.Id);

            issue = DynamicUtils.UpdateObjectWithFieldMask(cmd, cmd.FieldMask, issue);
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Issue, IssueVm>(issue);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new UpdateIssueCommandException($"Issue \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new UpdateIssueCommandException($"Updating issue \"{cmd.Id}\" error.", ex);
        }
    }
}
