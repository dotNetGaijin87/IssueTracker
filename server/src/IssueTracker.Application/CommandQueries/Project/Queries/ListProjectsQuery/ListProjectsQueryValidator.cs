using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;
public class ListProjectsQueryValidator : AbstractValidator<ListProjectsQuery>
{
    public ListProjectsQueryValidator()
    {
        RuleFor(p => p.Page)
            .GreaterThan(0);

        RuleFor(p => p.PageSize)
            .InclusiveBetween(1, 20);
    }
}
