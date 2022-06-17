using IssueTracker.Application.Interfaces;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using static IssueTracker.IntegrationTests.Helpers.ServiceStubs;

namespace IssueTracker.ApplicationTests.Helpers
{
    public static class WebHost
    {
        private static readonly object _locker = new object();
        private static readonly ConcurrentDictionary<string, WebApplicationFactory<Program>> Hosts =
            new ConcurrentDictionary<string, WebApplicationFactory<Program>>(StringComparer.OrdinalIgnoreCase);

        public static WebApplicationFactory<Program> GetSingletonHost(string callingClass)
        {
            lock (_locker)
            {
                if (!Hosts.ContainsKey(callingClass))
                {
                    Hosts.TryAdd(callingClass, CreateNewHost(callingClass));
                }

                return Hosts[callingClass];
            }
        }

        private static WebApplicationFactory<Program> CreateNewHost(string callingClass)
        {
            return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserCredentialsService, StubUserCredentialsService>();
                    services.AddScoped<IAuthService, StubAuthService>();

                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    services.Remove(descriptor);

                    string connection = GetIntegrationTestConnectionString(callingClass, services);

                    services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlServer(connection))
                            .AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

                    services.AddScoped<IAppDbContext, AppDbContext>();
                });
            });
        }

        private static string GetIntegrationTestConnectionString(string callingClass, IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connection = config.GetConnectionString("IntegratioTestConnection");
            connection = connection.Replace("_Test", $"_{callingClass}_Test");


            if (Regex.Matches(connection, "_Test").Count != 1)
                throw new Exception("No properly defined connection string for integration tests");
            return connection;
        }
    }
}
