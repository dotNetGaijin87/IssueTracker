using IssueTracker.Application.Utils;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace IssueTracker.Application.Services.UserRoleService;

public class UserCredentialsService : IUserCredentialsService
{
    private readonly HttpContext _httpContext;
    private readonly IConfiguration _config;

    public UserCredentialsService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _config = config;
    }

    public UserCredentials Get()
    {
        try
        {
            return new UserCredentials
            {
                Id = extractClaim("Auth0:User_id").Split("|").LastOrDefault(),
                Name = extractClaim("Auth0:Name"),
                Email = extractClaim("Auth0:Email"),
                Role = EnumUtils.StringToUserRole(extractClaim("Auth0:Roles"))
            };
        }
        catch (ExtractUserClaimException ex)
        {
            throw new ExtractUserClaimException("Extracting user credentials error", ex);
        }
    }

    private string extractClaim(string key)
    {
        return _httpContext.User.Claims
            .Where(x => !String.IsNullOrEmpty(x.Value))
            .Where(x => x.Type == _config[key])
            .Select(x => x.Value)
            .SingleOrDefault();
    }
}
