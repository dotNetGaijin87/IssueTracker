using AutoMapper;
using IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class CreatePermissionCommandToUserIssue : Profile
{
    public CreatePermissionCommandToUserIssue()
    {
        CreateMap<CreatePermissionCommand, Permission>()
            .ForMember(x => x.User, opt => opt.Ignore())
            .ForMember(x => x.Issue, opt => opt.Ignore())
            .ForMember(x => x.KanbanRowPosition, opt => opt.Ignore())
            .ForMember(x => x.GrantedBy, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());
    }
}