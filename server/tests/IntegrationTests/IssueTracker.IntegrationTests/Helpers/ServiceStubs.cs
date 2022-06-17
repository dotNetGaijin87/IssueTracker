using IssueTracker.Application.Attributes;
using IssueTracker.Application.Intrefaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using System.Collections.Generic;

namespace IssueTracker.IntegrationTests.Helpers
{
    public static class ServiceStubs
    {
        public class StubUserCredentialsService : IUserCredentialsService
        {
            public UserCredentials Get()
            {
                return new UserCredentials
                {
                    Id = "User_5",
                    Name = "User_5",
                    Email = "www.user_5@email.com",
                    Role = UserRole.admin
                };
            }
        }

        public class StubAuthService : IAuthService
        {
            public bool HasRequiredRole(IEnumerable<AuthorizeUserAttribute> attributes, UserRole role)
            {
                return true;
            }

            public bool IsAuthenticated()
            {
                return true;
            }
        }

    }
}
