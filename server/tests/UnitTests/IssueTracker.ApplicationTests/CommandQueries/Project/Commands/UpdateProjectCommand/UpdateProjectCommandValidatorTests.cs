using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateProjectCommandValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InvalidId_ValidationError(string projectId)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand() { Id = projectId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData("01234567890123456789")]
    [InlineData("abcdefghijklm")]
    public void TestValidate_ValidSummary_ValidationSuccess(string summary)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand() { Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Summary);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456789")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891")]
    public void TestValidate_InvalidSummary_ValidationError(string summary)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand() { Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Summary);
    }

    [Fact]
    public void TestValidate_AdminRole_ValidationSuccess()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand() { UserCredentials = new UserCredentials { Role = UserRole.admin } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Fact]
    public void TestValidate_UserWhoCreatedTheProjectWithManagerRole_ValidationSuccess()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project { Id = "ProjectId", CreatedBy = "manager", Summary = "summary" });
        dbContext.SaveChanges();

        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand
        {
            Id = "ProjectId",
            UserCredentials = new UserCredentials { Role = UserRole.manager, Name = "manager" }
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void TestValidate_UserWithManagerRoleWhoDidNotCreateTheProject_ValidationError()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "otherManager", Summary = "summary" });
        dbContext.SaveChanges();
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand
        {
            Id = "Project_1",
            UserCredentials = new UserCredentials { Role = UserRole.manager, Name = "manager" }
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void TestValidate_RoleEmployee_ValidationError()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateProjectCommandValidator(dbContext);
        var model = new UpdateProjectCommand() { UserCredentials = new UserCredentials { Role = UserRole.employee } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

}

