using Hellang.Middleware.ProblemDetails;
using IssueTracker.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IssueTracker.ServicesExtensions;

public static class AuthenticationExt
{
    public static AuthenticationBuilder AddAuthenticationExt(this IServiceCollection services, ConfigurationManager configManager)
    {
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configManager["Auth0:Authority"];
            options.Audience = configManager["Auth0:Audience"];
        });
    }
}


