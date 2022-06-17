using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class ProjectToVm : Profile
{
    public ProjectToVm()
    {
        CreateMap<Project, ProjectVm>();
    }
}