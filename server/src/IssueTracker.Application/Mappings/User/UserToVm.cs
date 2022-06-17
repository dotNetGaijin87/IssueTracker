using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class UserToVm : Profile
{
    public UserToVm()
    {
        CreateMap<User, UserVm>()
            .ForMember(x => x.Name, opt => opt.Ignore())
            .ForMember(x => x.Projects, opt => opt.Ignore())
            .ForMember(x => x.Posts, opt => opt.Ignore());
    }
}