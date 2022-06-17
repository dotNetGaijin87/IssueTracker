using AutoMapper;
using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;
using System.Linq;

namespace IssueTracker.ApplicationTests.Helpers
{
    internal static class AutoMapperHelpers
    {

        internal static IMapper CreateAutoMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                // User
                opts.CreateMap<User, UserVm>()
                        .ForMember(x => x.Name, opt => opt.Ignore())
                        .ForMember(x => x.Projects, opt => opt.Ignore())
                        .ForMember(x => x.Posts, opt => opt.Ignore());


                // Project
                opts.CreateMap<CreateProjectCommand, Project>()
                        .ForMember(dest => dest.CreatedBy, 
                            cfg => cfg.MapFrom(src => src.UserCredentials.Name))
                        .ForMember(x => x.CreationTime, opt => opt.Ignore())
                        .ForMember(x => x.CompletionTime, opt => opt.Ignore())
                        .ForMember(x => x.Issues, opt => opt.Ignore());
                opts.CreateMap<Project, ProjectVm>();

 
                // Issue
                opts.CreateMap<CreateIssueCommand, Issue>()
                        .ForMember(dest => dest.CreatedBy,
                            cfg => cfg.MapFrom(src => src.UserCredentials.Name))
                        .ForMember(x => x.CreationTime, opt => opt.Ignore())
                        .ForMember(x => x.CompletionTime, opt => opt.Ignore())
                        .ForMember(x => x.Project, opt => opt.Ignore())
                        .ForMember(x => x.Permissions, opt => opt.Ignore())
                        .ForMember(x => x.Comments, opt => opt.Ignore());

                opts.CreateMap<Issue, KanbanCardVm>()
                        .ForMember(dest => dest.ProjectId, cfg => cfg.MapFrom(src => src.Project.Id))
                        .ForMember(dest => dest.Position, 
                            cfg => cfg.MapFrom(src => src.Permissions.Single().KanbanRowPosition));

                opts.CreateMap<Issue, IssueVm>()
                        .ForMember(dest => dest.Permission,
                            cfg =>
                            {
                                cfg.PreCondition(src => src.Permissions.Any());
                                cfg.MapFrom(src => src.Permissions.First());
                            })
                        .ForMember(x => x.CommentPageCount, opt => opt.Ignore());


                // Permissions
                opts.CreateMap<CreatePermissionCommand, Permission>()
                        .ForMember(x => x.User, opt => opt.Ignore())
                        .ForMember(x => x.Issue, opt => opt.Ignore())
                        .ForMember(x => x.KanbanRowPosition, opt => opt.Ignore())
                        .ForMember(x => x.GrantedBy, opt => opt.Ignore())
                        .ForMember(x => x.CreationTime, opt => opt.Ignore());

                opts.CreateMap<Permission, PermissionVm>();


                // Comments
                opts.CreateMap<Comment, CommentVm>();

                opts.CreateMap<CreateCommentCommand, Comment>()
                        .ForMember(dest => dest.UserId, cfg => cfg.MapFrom(src => src.UserCredentials.Name))
                        .ForMember(x => x.CreationTime, opt => opt.Ignore())
                        .ForMember(x => x.Issue, opt => opt.Ignore())
                        .ForMember(x => x.Id, opt => opt.Ignore())
                        .ForMember(x => x.User, opt => opt.Ignore());


            });


            return config.CreateMapper();
        }

    }
}
