using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class CreateCommentCommandValidatorTests
{
    [Fact]
    public async Task TestValidate_ValidIssueId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new CreateCommentCommandValidator();
        var model = new CreateCommentCommand() { IssueId = "Issue_1" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InvalidIssueId_ValidationError(string issueId)
    {
        // ARRANGE
        var validator = new CreateCommentCommandValidator();
        var model = new CreateCommentCommand() { IssueId = issueId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.IssueId);
    }

    [Fact]
    public async Task TestValidate_ValidContent_ValidationSuccess()
    {
        // ARRANGE
        var validator = new CreateCommentCommandValidator();
        var model = new CreateCommentCommand() { Content = "Sample content" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InvalidContent_ValidationError(string content)
    {
        // ARRANGE
        var validator = new CreateCommentCommandValidator();
        var model = new CreateCommentCommand() { Content = content };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}

