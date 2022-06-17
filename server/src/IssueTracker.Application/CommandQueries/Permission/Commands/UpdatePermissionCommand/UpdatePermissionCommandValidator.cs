using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();
    }
}
