using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class CreatePermissionCommandValidatorTests
{
    [Fact]
    public void TestValidate_ValidUserId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new CreatePermissionCommandValidator();
        var model = new CreatePermissionCommand() { UserId = "User_1" };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InValidUserId_ValidationError(string userId)
    {
        // ARRANGE
        var validator = new CreatePermissionCommandValidator();
        var model = new CreatePermissionCommand() { UserId = userId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void TestValidate_ValidIssueId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new CreatePermissionCommandValidator();
        var model = new CreatePermissionCommand() { IssueId = "Issue_1" };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InValidIssueId_ValidationError(string issueId)
    {
        // ARRANGE
        var validator = new CreatePermissionCommandValidator();
        var model = new CreatePermissionCommand() { IssueId = issueId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.IssueId);
    }
}

