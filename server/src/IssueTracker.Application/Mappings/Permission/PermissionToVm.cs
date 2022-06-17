using AutoMapper;
using IssueTracker.Application.Models;
using IssueTracker.Domain.Models;

namespace IssueTracker.Application.Mappings;
public class PermissionToVm : Profile
{
    public PermissionToVm()
    {
        CreateMap<Permission, PermissionVm>();
    }
}