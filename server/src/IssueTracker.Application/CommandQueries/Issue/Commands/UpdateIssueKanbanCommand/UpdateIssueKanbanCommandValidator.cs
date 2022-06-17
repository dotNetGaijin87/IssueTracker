using FluentValidation;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;

public class UpdateIssueKanbanCommandValidator : AbstractValidator<UpdateIssueKanbanCommand>
{
    public UpdateIssueKanbanCommandValidator()
    {
        RuleFor(v => v.IssueId)
            .NotNull()
            .NotEmpty();

        RuleFor(cmd => cmd.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager || r == UserRole.employee)
            .WithMessage(x => $"User is not authorized to modify the kanban.");

    }
}
