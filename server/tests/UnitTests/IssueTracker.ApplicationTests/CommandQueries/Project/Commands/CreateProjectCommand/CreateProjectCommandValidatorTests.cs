using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class CreateProjectCommandValidatorTests
{
    [Theory]
    [InlineData(UserRole.manager)]
    [InlineData(UserRole.admin)]
    public void TestValidate_ValidRole_ValidationSuccess(UserRole role)
    {
        // ARRANGE
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { UserCredentials = new UserCredentials { Role = role } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Fact]
    public void TestValidate_RoleEmployee_ValidationError()
    {
        // ARRANGE
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { UserCredentials = new UserCredentials { Role = UserRole.employee }  };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("abcdefgh")]
    [InlineData("01234567890123456789012345678901234567890123456789")]
    public void TestValidate_ValidId_ValidationSuccess(string id)
    {
        // ARRANGE
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { Id = id };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("012345678901234567890123456789012345678901234567890")]
    public void TestValidate_InValidId_ValidationError(string id)
    {
        // ARRANGE
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { Id = id };

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
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { Summary = summary };

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
        var validator = new CreateProjectCommandValidator();
        var model = new CreateProjectCommand() { Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Summary);
    }
}

