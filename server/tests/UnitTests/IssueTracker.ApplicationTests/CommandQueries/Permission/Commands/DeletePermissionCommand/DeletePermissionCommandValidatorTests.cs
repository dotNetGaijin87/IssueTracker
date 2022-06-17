using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeletePermissionCommandValidatorTests
{
    [Fact]
    public void TestValidate_ValidUserId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new DeletePermissionCommandValidator();
        var model = new DeletePermissionCommand() { UserId = "User_1" };

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
        var validator = new DeletePermissionCommandValidator();
        var model = new DeletePermissionCommand() { UserId = userId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void TestValidate_ValidIssueId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new DeletePermissionCommandValidator();
        var model = new DeletePermissionCommand() { IssueId = "Issue_1" };

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
        var validator = new DeletePermissionCommandValidator();
        var model = new DeletePermissionCommand() { IssueId = issueId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.IssueId);
    }
}

