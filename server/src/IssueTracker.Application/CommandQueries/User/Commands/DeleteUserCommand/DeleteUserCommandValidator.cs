using FluentValidation;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(p => p.UserCredentials.Role)
            .Equal(UserRole.admin)
                .WithMessage(x => $"User is not authorized to delete another user.");


        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
