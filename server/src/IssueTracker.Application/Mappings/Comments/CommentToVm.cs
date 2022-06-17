using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings.Comments;

public class CommentToVm : Profile
{
    public CommentToVm()
    {
        CreateMap<Comment, CommentVm>();
    }
}