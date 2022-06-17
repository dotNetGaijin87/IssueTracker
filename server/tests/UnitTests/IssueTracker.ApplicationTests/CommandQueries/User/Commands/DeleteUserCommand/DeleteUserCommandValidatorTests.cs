using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeleteUserCommandValidatorTests
{
    [Fact]
    public void TestValidate_AdminRole_ValidationSuccess()
    {
        // ARRANGE
        var validator = new DeleteUserCommandValidator();
        var model = new DeleteUserCommand() 
        { 
            Id = "User_1",
            UserCredentials = new UserCredentials 
            { 
                Role = UserRole.admin 
            } 
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Theory]
    [InlineData(UserRole.manager)]
    [InlineData(UserRole.employee)]
    public void TestValidate_NotAUthorizedRole_ValidationError(UserRole role)
    {
        // ARRANGE
        var validator = new DeleteUserCommandValidator();
        var model = new DeleteUserCommand()
        {
            Id = "User_1",
            UserCredentials = new UserCredentials
            {
                Role = role
            }
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserCredentials.Role);
    }

    [Fact]
    public void TestValidate_ValidUserId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new DeleteUserCommandValidator();
        var model = new DeleteUserCommand() { Id = "User_1" };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
 
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InValidUserId_ValidationError(string userId)
    {
        // ARRANGE
        var validator = new DeleteUserCommandValidator();
        var model = new DeleteUserCommand() { Id = userId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}

