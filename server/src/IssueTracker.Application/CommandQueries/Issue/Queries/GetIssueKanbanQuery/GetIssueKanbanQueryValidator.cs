using FluentValidation;
using IssueTracker.Domain.Models.Enums;

namespace IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;

public class GetIssueKanbanQueryValidator : AbstractValidator<GetIssueKanbanQuery>
{
    public GetIssueKanbanQueryValidator()
    {
 
    }
}
