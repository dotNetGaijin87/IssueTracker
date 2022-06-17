using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class IssueToVm : Profile
{
    public IssueToVm()
    {
        CreateMap<Issue, IssueVm>()
            .ForMember(dest => dest.Permission,
                cfg =>
                {
                    cfg.PreCondition(src => src.Permissions.Any());
                    cfg.MapFrom(src => src.Permissions.First());
                })
            .ForMember(x => x.CommentPageCount, opt => opt.Ignore());
    }
}