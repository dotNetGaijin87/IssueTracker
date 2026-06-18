using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeleteProjectCommandValidatorTests
{
    [Fact]
    public async Task TestValidate_AdminRole_ValidationSuccess()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new DeleteProjectCommandValidator(dbContext);
        var model = new DeleteProjectCommand() { UserCredentials = new UserCredentials { Role = UserRole.admin } };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }


    [Fact]
    public async Task TestValidate_UserWhoCreatedTheProjectWithManagerRole_ValidationSuccess()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project { Id = "ProjectId", CreatedBy = "manager", Summary ="summary" });
        dbContext.SaveChanges();

        var validator = new DeleteProjectCommandValidator(dbContext);
        var model = new DeleteProjectCommand
        { 
             Id = "ProjectId",
            UserCredentials = new UserCredentials { Role = UserRole.manager, Name ="manager" } 
        };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x);
    }


    [Theory]
    [InlineData(UserRole.employee)]
    public async Task TestValidate_UnauthorizedRole_ValidationError(UserRole role)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new DeleteProjectCommandValidator(dbContext);
        var model = new DeleteProjectCommand() { UserCredentials = new UserCredentials { Role = role }  };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserCredentials.Role);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InValidId_ValidationError(string id)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new DeleteProjectCommandValidator(dbContext);
        var model = new DeleteProjectCommand() { Id = id };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}

