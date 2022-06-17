using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;
public class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
{
    public GetCommentQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty();
    }
}