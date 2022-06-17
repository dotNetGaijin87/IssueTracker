using AutoMapper;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class CreateProjectCommandToProjectMap : Profile
{
    public CreateProjectCommandToProjectMap()
    {
        CreateMap<CreateProjectCommand, Project>()
            .ForMember(dest => dest.CreatedBy,
                cfg => cfg.MapFrom(src => src.UserCredentials.Name))
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.CompletionTime, opt => opt.Ignore())
            .ForMember(x => x.Issues, opt => opt.Ignore());
    }
}