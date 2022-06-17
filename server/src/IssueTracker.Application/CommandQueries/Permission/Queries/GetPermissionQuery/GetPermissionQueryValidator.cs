using FluentValidation;

namespace IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;
public class GetPermissionQueryValidator : AbstractValidator<GetPermissionQuery>
{
    public GetPermissionQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.IssueId)
            .NotNull()
            .NotEmpty();
    }
}
