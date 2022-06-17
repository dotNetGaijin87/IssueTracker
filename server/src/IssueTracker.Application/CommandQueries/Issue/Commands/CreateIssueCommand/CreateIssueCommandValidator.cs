using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;

public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty()
            .Length(3, 50)
                .WithMessage(p => $"Your input is outside allowable issue id length range (3, 50) .");

        RuleFor(p => p.Summary)
            .NotNull()
            .NotEmpty()
            .Length(10, 100);
    }
}
