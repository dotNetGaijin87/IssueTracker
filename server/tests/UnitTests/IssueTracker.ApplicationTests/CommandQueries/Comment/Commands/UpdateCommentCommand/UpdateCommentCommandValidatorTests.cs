using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateCommentCommandValidatorTests
{
    [Fact]
    public void TestValidate_ValidId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new UpdateCommentCommandValidator();
        var model = new UpdateCommentCommand() { Id = "Comment_1" };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_ValidId_ValidationError(string commentId)
    {
        // ARRANGE
        var validator = new UpdateCommentCommandValidator();
        var model = new UpdateCommentCommand() { Id = commentId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void TestValidate_ValidContent_ValidationSuccess()
    {
        // ARRANGE
        var validator = new UpdateCommentCommandValidator();
        var model = new UpdateCommentCommand() { Content = "Sample content" };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InvalidContent_ValidationError(string content)
    {
        // ARRANGE
        var validator = new UpdateCommentCommandValidator();
        var model = new UpdateCommentCommand() { Content = content };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }
}

