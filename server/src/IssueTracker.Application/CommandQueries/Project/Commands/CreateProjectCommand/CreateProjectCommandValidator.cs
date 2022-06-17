using FluentValidation;
using IssueTracker.Application.Extensions;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(p => p.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager)
                .WithMessage(x => $"User is not authorized to create the project.");

        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty()
            .Length(3, 50)
                .WithMessage(p => $"Your input is outside allowable project id length range (3, 50) .");

        RuleFor(p => p.Summary)
            .NotNull()
            .NotEmpty()
            .Length(10, 100);
    }
}
