using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}
