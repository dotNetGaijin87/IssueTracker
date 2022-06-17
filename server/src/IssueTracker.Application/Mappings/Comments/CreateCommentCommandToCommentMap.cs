using AutoMapper;
using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings.Comments;
public class CreateCommentCommandToCommentMap : Profile
{
    public CreateCommentCommandToCommentMap()
    {
        CreateMap<CreateCommentCommand, Comment>()
            .ForMember(dest => dest.UserId, cfg => cfg.MapFrom(src => src.UserCredentials.Name))
            .ForMember(x => x.CreationTime, opt => opt.Ignore())
            .ForMember(x => x.Issue, opt => opt.Ignore())
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.User, opt => opt.Ignore());
    }

}