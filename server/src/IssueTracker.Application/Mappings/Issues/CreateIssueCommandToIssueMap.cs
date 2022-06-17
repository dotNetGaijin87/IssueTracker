using AutoMapper;
using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class CreateIssueCommandToIssueMap : Profile
{
    public CreateIssueCommandToIssueMap()
    {
        CreateMap<CreateIssueCommand, Issue>()
            .ForMember(dest => dest.CreatedBy,
                cfg => cfg.MapFrom(src => src.UserCredentials.Name))
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.CompletionTime, opt => opt.Ignore())
            .ForMember(x => x.Project, opt => opt.Ignore())
            .ForMember(x => x.Permissions, opt => opt.Ignore())
            .ForMember(x => x.Comments, opt => opt.Ignore());
    }
}