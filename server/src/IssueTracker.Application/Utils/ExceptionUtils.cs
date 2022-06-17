using Microsoft.Data.SqlClient;

namespace IssueTracker.Application.ExceptionUtils;
public static class ExceptionUtils
{
    public static bool IsDuplicateKeyConstraintViolationError(this Exception ex)
    {
        return ex is SqlException sqlex && sqlex.Number == 2627;
    }
    
    public static bool IsSequenceContainsNoElementsError(this Exception ex)
    {
        return ex is InvalidOperationException && ex.HResult == -2146233079;
    }
}