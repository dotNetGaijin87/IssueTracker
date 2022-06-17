using AutoMapper;
using IssueTracker.Application.Utils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using IssueTracker.Application.ExceptionUtils;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;


public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ProjectVm> Handle(UpdateProjectCommand cmd, CancellationToken ct)
    {
        try
        {
            Project project = await _appDbContext.Projects
                .SingleAsync(x => x.Id == cmd.Id);

            project = DynamicUtils.UpdateObjectWithFieldMask(cmd, cmd.FieldMask, project);
            await _appDbContext.SaveChangesAsync();


            return _mapper.Map<Project, ProjectVm>(project);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new UpdateProjectCommandException($"Project \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new UpdateProjectCommandException($"Updating project \"{cmd.Id}\" error.", ex);
        }
    }
}
