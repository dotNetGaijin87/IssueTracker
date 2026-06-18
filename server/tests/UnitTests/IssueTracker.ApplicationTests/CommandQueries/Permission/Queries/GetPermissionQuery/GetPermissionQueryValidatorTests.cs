using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;
using IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetPermissionQueryValidatorTests
{
    [Fact]
    public async Task TestValidate_ValidUserId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new GetPermissionQueryValidator();
        var model = new GetPermissionQuery() { UserId = "User_1" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InValidUserId_ValidationError(string userId)
    {
        // ARRANGE
        var validator = new GetPermissionQueryValidator();
        var model = new GetPermissionQuery() { UserId = userId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task TestValidate_ValidIssueId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new GetPermissionQueryValidator();
        var model = new GetPermissionQuery() { IssueId = "Issue_1" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InValidIssueId_ValidationError(string issueId)
    {
        // ARRANGE
        var validator = new GetPermissionQueryValidator();
        var model = new GetPermissionQuery() { IssueId = issueId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.IssueId);
    }
}

