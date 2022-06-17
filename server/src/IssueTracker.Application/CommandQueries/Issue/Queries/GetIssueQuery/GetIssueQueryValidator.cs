using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;
public class GetIssueQueryValidator : AbstractValidator<GetIssueQuery>
{
    public GetIssueQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
