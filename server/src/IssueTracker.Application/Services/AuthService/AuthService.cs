using IssueTracker.Application.Attributes;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Application.Services.IdentityService;

public class AuthService : IAuthService
{
    private readonly HttpContext _httpContext;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public bool IsAuthenticated()
    {
        if (!_httpContext.User.Identity.IsAuthenticated)
            return false;


        return true;
    }

    public bool HasRequiredRole(IEnumerable<AuthorizeUserAttribute> attributes, UserRole role)
    {
        // Filter out those attributes which do not have roles defined
        if (!attributes.SelectMany(x => x.Roles).Any())
        {
            return true;
        }

        var isAuthorized = attributes.Any(x => x.Roles.Any(y => y == role));


        return isAuthorized;
    }
}
