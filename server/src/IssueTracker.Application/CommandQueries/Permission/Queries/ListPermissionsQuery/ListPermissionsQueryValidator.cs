using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;
public class ListPermissionsQueryValidator : AbstractValidator<ListPermissionsQuery>
{
    public ListPermissionsQueryValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .InclusiveBetween(1, 20);
    }
}
