using AutoMapper;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;


public class UpdateIssueKanbanCommandHandler : IRequestHandler<UpdateIssueKanbanCommand, IEnumerable<KanbanCardVm>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdateIssueKanbanCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<KanbanCardVm>> Handle(UpdateIssueKanbanCommand cmd, CancellationToken ct)
    {
        try
        {
            var issues = await _appDbContext.Issues
                .Where(x => cmd.Permissions.Select(x => x.IssueId).Contains(x.Id))
                .Include(x =>  x.Permissions.Where(x => x.UserId == cmd.UserCredentials.Name))
                .ToListAsync();

            foreach (Issue issue in issues)
            {
                for (int i = 0; i < issue.Permissions.Count(); i++)
                {
                    if (issue.Id == cmd.IssueId)
                        issue.Progress = cmd.Progress;

                    Permission newPermission = cmd.Permissions.Where(x => x.IssueId == issue.Id).Single();

                    issue.Permissions[i].IsPinnedToKanban = newPermission.IsPinnedToKanban;
                    issue.Permissions[i].KanbanRowPosition = newPermission.KanbanRowPosition;
                }
            }

            _appDbContext.Issues.UpdateRange(issues);
            await _appDbContext.SaveChangesAsync();


            return issues.Select(x => _mapper.Map<Issue, KanbanCardVm>(x))
                    .ToList();
        }
        catch (Exception ex)
        {
            throw new UpdateIssueKanbanCommandException($"Updating kanban for user \"{cmd.UserCredentials.Name}\" error.", ex);
        }
    }
}
