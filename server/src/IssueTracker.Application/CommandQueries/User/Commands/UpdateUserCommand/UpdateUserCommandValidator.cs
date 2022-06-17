using FluentValidation;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.UserCredentials.Role)
            .Equal(UserRole.admin)
                .WithMessage(x => $"User is not authorized to update another user.");

        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.FieldMask)
            .NotNull()
            .NotEmpty()
            .Must(x => x != null ? x.Count() > 0 : false); 
    }
}
