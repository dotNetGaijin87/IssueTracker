using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
{
    public GetProjectQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty();
    }
}