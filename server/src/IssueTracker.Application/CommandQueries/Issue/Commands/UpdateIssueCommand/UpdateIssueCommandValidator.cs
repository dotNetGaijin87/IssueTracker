using FluentValidation;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;

public class UpdateIssueCommandValidator : AbstractValidator<UpdateIssueCommand>
{
    private readonly IAppDbContext _appDbContext;
    public UpdateIssueCommandValidator(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;


        RuleFor(cmd => cmd.Id)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);

        RuleFor(cmd => cmd.Summary)
            .NotNull()
            .NotEmpty()
            .Length(10, 100)
                .When(cmd => cmd.FieldMask.Contains("Summary"));

        // Issue can be modified only by administrators, managers or employees who have correct rights  
        RuleFor(cmd => cmd.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager || r == UserRole.employee)
            .WithMessage(x => $"User is not authorized to modify the issue.");

        When(x => x.UserCredentials.Role == UserRole.employee, () => {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    bool canModify = await _appDbContext.Permissions
                        .Where(x => x.IssueId == cmd.Id &&
                                    x.UserId == cmd.UserCredentials.Name &&
                                    (x.IssuePermission == IssuePermission.CanModify || 
                                    x.IssuePermission == IssuePermission.CanDelete)).AnyAsync();

                    return canModify;
                }).WithMessage(p => $"User is not authorized to modify the issue.");
        });
    }
}
