using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;

public class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();
    }
}
