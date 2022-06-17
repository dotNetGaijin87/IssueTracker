using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class IssueToKanbanCardVm : Profile
{
    public IssueToKanbanCardVm()
    {
        CreateMap<Issue, KanbanCardVm>()
            .ForMember(dest => dest.ProjectId, cfg => cfg.MapFrom(src => src.Project.Id))
            .ForMember(dest => dest.Position,
                cfg => cfg.MapFrom(src => src.Permissions.Single().KanbanRowPosition));
    }
}