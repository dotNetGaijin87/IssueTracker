using IssueTracker.Application.Interfaces;
using IssueTracker.Infrastructure.Services.AppDbContext;
using IssueTracker.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IssueTracker.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureExt(this IServiceCollection services, IConfiguration configuration)
    {
        // Add custom services
        services.AddScoped<IDbInitializer, DbInitializer>();


        // Add database
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IAppDbContext,AppDbContext>();


        return services;
    }
}

public class ApplicationEntryPoint { }

