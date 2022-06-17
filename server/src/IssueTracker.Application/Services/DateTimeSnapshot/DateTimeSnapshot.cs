using IssueTracker.Application.Intrefaces;

namespace IssueTracker.Application.Services.DateTimeSnapshot;

public class DateTimeSnapshot : IDateTimeSnapshot
{
    public DateTime Now
    {
        get 
        { 
            return DateTime.Now; 
        }
    }
}
