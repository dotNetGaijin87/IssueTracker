using IssueTracker.Application.Attributes;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Interfaces;

public interface IAuthService
{
    bool IsAuthenticated();
    bool HasRequiredRole(IEnumerable<AuthorizeUserAttribute> attributes, UserRole role);
}