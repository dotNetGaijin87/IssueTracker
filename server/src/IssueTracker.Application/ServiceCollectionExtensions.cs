using FluentValidation;
using IssueTracker.Application.Behaviours;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Application.Services.DateTimeSnapshot;
using IssueTracker.Application.Services.IdentityService;
using IssueTracker.Application.Services.UserRoleService;
using IssueTracker.Application.Shared.Behaviours;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IssueTracker.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationExt(this IServiceCollection services, IConfiguration configuration)
    {
        // Add custom services
        services.AddTransient<IDateTimeSnapshot, DateTimeSnapshot>()
                .AddTransient<IAuthService,  AuthService>()
                .AddTransient<IUserCredentialsService, UserCredentialsService>()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Add third party libraries
        services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddAutoMapper(Assembly.GetExecutingAssembly());
        AssemblyScanner.FindValidatorsInAssembly(typeof(ApplicationEntryPoint).Assembly)
                .ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));


        return services;
    }
}

public class ApplicationEntryPoint { }

