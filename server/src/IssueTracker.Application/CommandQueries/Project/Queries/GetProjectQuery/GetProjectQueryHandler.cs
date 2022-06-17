using AutoMapper;
using IssueTracker.Application.ExceptionUtils;
using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectVm>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetProjectQueryHandler(IAppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ProjectVm> Handle(GetProjectQuery cmd, CancellationToken ct)
    {
        try
        {
            Project project =  await _appDbContext.Projects
                .AsNoTracking()
                .Where(x => x.Id == cmd.Id)
                .SingleAsync();


            return _mapper.Map<Project, ProjectVm>(project);
        }
        catch (Exception ex) when (ex.IsSequenceContainsNoElementsError())
        {
            throw new GetProjectQueryException($"Project \"{cmd.Id}\" not found.", ex, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            throw new GetProjectQueryException($"Getting project \"{cmd.Id}\" error.", ex) ;
        }
    }
}