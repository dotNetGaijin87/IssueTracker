using FluentValidation;
using IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;
public class ListUsersQueryValidator : AbstractValidator<ListUsersQuery>
{
    public ListUsersQueryValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .InclusiveBetween(1, 20);
    }
}
