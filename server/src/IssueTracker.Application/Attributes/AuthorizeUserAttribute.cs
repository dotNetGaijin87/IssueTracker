using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthorizeUserAttribute : Attribute
    {
        public AuthorizeUserAttribute(params UserRole[] roles) : base()
        {
            Roles = roles?.ToList();
        }

        public List<UserRole> Roles { get; init; }

    }
}
