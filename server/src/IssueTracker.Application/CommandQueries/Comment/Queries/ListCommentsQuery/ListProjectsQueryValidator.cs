using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;
public class ListCommentsQueryValidator : AbstractValidator<ListCommentsQuery>
{
    public ListCommentsQueryValidator()
    {
        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Page)
            .GreaterThan(0);

        RuleFor(p => p.PageSize)
            .InclusiveBetween(1, 20);
    }
}
