using FluentValidation;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    IAppDbContext _appDbContext;

    public DeleteProjectCommandValidator(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty();

        // Projects can be deleted only by administrators or managers who created them
        RuleFor(p => p.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager)
                .WithMessage(x => $"User is not authorized to delete the project.");

        RuleFor(x => x)
            .MustAsync(async (p, ct) =>
            {
                Project project = await _appDbContext.Projects.Where(x => x.Id == p.Id).SingleOrDefaultAsync();
                if (project is null)
                    throw new ValidationException("Project not found");

                return project.CreatedBy == p.UserCredentials.Name;
            }).When(p => p.UserCredentials.Role == UserRole.manager)
            .WithMessage(p => $"User is not authorized to delete the project.");

    }
}





 