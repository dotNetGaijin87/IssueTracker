using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ProjectVm> Handle(CreateProjectCommand cmd, CancellationToken ct)
    {
        try
        {
            var createProject = _mapper.Map<CreateProjectCommand, Project>(cmd);
            _appDbContext.Projects.Add(createProject);
            await _appDbContext.SaveChangesAsync();
            var project = await _appDbContext.Projects.SingleAsync(p => p.Id == cmd.Id);


            return _mapper.Map<Project, ProjectVm>(project);
        }
        catch (Exception ex) when (ex.InnerException.IsDuplicateKeyConstraintViolationError())
        {
            throw new CreateProjectCommandException($"Project with the same id \"{cmd.Id}\" already exists.", ex, HttpStatusCode.Conflict);
        }
        catch (Exception ex)
        {
            throw new CreateProjectCommandException($"Creating project \"{cmd.Id}\" error.", ex);
        }
    }
}
