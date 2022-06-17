using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();
    }
}
