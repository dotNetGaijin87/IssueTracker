using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using System.Net;
using IssueTracker.Application.ExceptionUtils;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, IssueVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CreateIssueCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IssueVm> Handle(CreateIssueCommand cmd, CancellationToken ct)
    {
        try
        {
            string requesterName = cmd.UserCredentials.Name;
            var newIssue = _mapper.Map<CreateIssueCommand, Issue>(cmd);
            var permissions = new List<Permission>
            {
                Permission.CreateWithCanDeletePermission(cmd.Id, requesterName, requesterName)
            };
            foreach (var userId in cmd.ResponsibleBy.Where(x => x != requesterName))
            {
                permissions.Add(Permission.CreateWithCanModifyPermission(cmd.Id, userId, requesterName));
            }
            newIssue.Permissions = permissions;

            _appDbContext.Issues.Add(newIssue);
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Issue, IssueVm>(newIssue);
        }
        catch (Exception ex) when (ex.InnerException.IsDuplicateKeyConstraintViolationError())
        {
            throw new CreateIssueCommandException($"Issue with the same id \"{cmd.Id}\" already exists.", ex, HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            throw new CreateIssueCommandException($"Creating issue \"{cmd.Id}\" error.", ex);
        }
    }
}
