using FluentValidation;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;

public class DeleteIssueCommandValidator : AbstractValidator<DeleteIssueCommand>
{
    private readonly IAppDbContext _appDbContext;
    public DeleteIssueCommandValidator(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;


        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();

        // Issue can be deleted only by administrators, managers or employees who have correct rights  
        RuleFor(x => x.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager || r == UserRole.employee)
            .WithMessage(x => $"User is not authorized to delete the issue.");

        When(x => x.UserCredentials.Role == UserRole.employee, () => { 
            RuleFor(p => p)
                .MustAsync(async (p, ct) =>
                {
                    bool canDelete = await _appDbContext.Permissions
                        .Where(x => x.IssueId == p.Id &&
                                    x.UserId == p.UserCredentials.Name &&
                                    x.IssuePermission == IssuePermission.CanDelete).AnyAsync();

                    return canDelete;
                }).WithMessage(p => $"User is not authorized to delete the issue."); 
        });
    }
}
