using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;
public class ListIssuesQueryValidator : AbstractValidator<ListIssuesQuery>
{
    public ListIssuesQueryValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .InclusiveBetween(1, 20);
    }
}
