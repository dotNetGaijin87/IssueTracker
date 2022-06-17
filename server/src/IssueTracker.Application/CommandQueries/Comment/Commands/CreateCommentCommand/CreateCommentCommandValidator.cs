using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Content)
            .NotNull()
            .NotEmpty();    
    }
}
