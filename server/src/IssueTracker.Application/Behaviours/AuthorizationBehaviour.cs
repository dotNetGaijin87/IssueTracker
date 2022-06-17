using IssueTracker.Application.Attributes;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Application.Models;
using IssueTracker.Application.Shared.Exceptions;
using MediatR;
using System.Net;
using System.Reflection;

namespace IssueTracker.Application.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : RequestBase, IRequest<TResponse>
{
    private readonly IAuthService _authService;
    private readonly IUserCredentialsService _userCredentialsService;

    public AuthorizationBehaviour(IAuthService identityService, IUserCredentialsService userRoleService)
    {
        _authService = identityService;
        _userCredentialsService = userRoleService;
}

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    { 
        var authAttributes = request.GetType().GetCustomAttributes<AuthorizeUserAttribute>();
        request.UserCredentials = _userCredentialsService.Get();

        if (authAttributes.Any() && !_authService.IsAuthenticated())
        {
            throw new UserNotAuthenticatedException("User authentication error.");
        }

        if (authAttributes.Any() && !_authService.HasRequiredRole(authAttributes, request.UserCredentials.Role))
        {
            throw new UserHasNotRequiredRoleException("User is not authorized to access the resource.");
        }


        return await next();
    }
}
