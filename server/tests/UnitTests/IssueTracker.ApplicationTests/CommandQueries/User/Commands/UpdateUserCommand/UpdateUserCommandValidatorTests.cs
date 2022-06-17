using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
using IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using System.Collections.Generic;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateUserCommandValidatorTests
{
    [Fact]
    public void TestValidate_AdminRole_ValidationSuccess()
    {
        // ARRANGE
        var validator = new UpdateUserCommandValidator();
        var model = new UpdateUserCommand() { UserCredentials = new UserCredentials { Role = UserRole.admin } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Theory]
    [InlineData(UserRole.manager)]
    [InlineData(UserRole.employee)]
    public void TestValidate_WrongRole_ValidationError(UserRole role)
    {
        // ARRANGE
        var validator = new UpdateUserCommandValidator();
        var model = new UpdateUserCommand() { UserCredentials = new UserCredentials { Role = role } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Fact]
    public void TestValidate_FiledMaskIsValid_ValidationSuccess()
    {
        // ARRANGE
        var validator = new UpdateUserCommandValidator();
        var model = new UpdateUserCommand()
        {
            UserCredentials = new UserCredentials { Role = UserRole.admin },
            FieldMask = new List<string>() { "Id"}
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.FieldMask);
    }

    [Fact]
    public void TestValidate_FiledMaskIsNull_ValidationError()
    {
        // ARRANGE
        var validator = new UpdateUserCommandValidator();
        var model = new UpdateUserCommand() 
        {
            UserCredentials = new UserCredentials { Role = UserRole.admin },
            FieldMask = null
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.FieldMask);
    }

    [Fact]
    public void TestValidate_FiledMaskIsEmpty_ValidationError()
    {
        // ARRANGE
        var validator = new UpdateUserCommandValidator();
        var model = new UpdateUserCommand()
        {
            UserCredentials = new UserCredentials { Role = UserRole.admin },
            FieldMask = new List<string>()
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.FieldMask);
    }
}
