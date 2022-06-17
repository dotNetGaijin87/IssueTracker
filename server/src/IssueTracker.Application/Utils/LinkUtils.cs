using System.Linq.Expressions;

namespace IssueTracker.Application.Utils;
public static class LinkUtils
{
    public static IQueryable<TSource> IfPropertyNotNullApplyWhere<TSource>(this IQueryable<TSource> source, object condition, Expression<Func<TSource, bool>> predicate)
    {
        if (condition is not null)
            source = source.Where(predicate);


        return source;
    }
 
}