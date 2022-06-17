using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.Utils
{
    public static class EnumUtils
    {
        public static UserRole StringToUserRole(string value)
        {
            return (UserRole)Enum.Parse(typeof(UserRole), value, ignoreCase: true);
        }
    }
}
