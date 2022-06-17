using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Content)
            .NotNull()
            .NotEmpty();
    }
}
