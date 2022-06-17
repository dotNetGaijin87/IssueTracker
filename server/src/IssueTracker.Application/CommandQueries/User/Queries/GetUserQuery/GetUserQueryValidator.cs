using FluentValidation;
 

namespace IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
