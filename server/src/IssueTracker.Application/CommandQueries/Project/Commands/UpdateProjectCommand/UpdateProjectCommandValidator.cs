using FluentValidation;
using IssueTracker.Application.Interfaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IAppDbContext _appDbContext;
    public UpdateProjectCommandValidator(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;


        RuleFor(p => p.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(p => p.Summary)
            .NotNull()
            .NotEmpty()
            .Length(10, 100);

        // Project can be updated only by administrators or a manager who created the project
        RuleFor(x => x.UserCredentials.Role)
            .Must(r => r == UserRole.admin || r == UserRole.manager)
            .WithMessage(x => $"User is not authorized to update the project.");

        RuleFor(x => x)
            .MustAsync(async (p, ct) =>
            {
                Project project = await _appDbContext.Projects.Where(x => x.Id == p.Id).SingleOrDefaultAsync();
                if (project is null)
                    throw new ValidationException("Project not found");

                return project.CreatedBy == p.UserCredentials.Name;
            }).When(p => p.UserCredentials.Role == UserRole.manager)
            .WithMessage(p => $"User is not authorized to update the project.");
    }
}
