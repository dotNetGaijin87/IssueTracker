using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeleteCommentCommandValidatorTests
{
    [Theory]
    [InlineData("Comment_1")]
    [InlineData("Issue_1_Comment_1")]
    public async Task TestValidate_ValidId_ValidationSuccess(string commentId)
    {
        // ARRANGE
        var validator = new DeleteCommentCommandValidator();
        var model = new DeleteCommentCommand() { Id = commentId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_ValidId_ValidationError(string commentId)
    {
        // ARRANGE
        var validator = new DeleteCommentCommandValidator();
        var model = new DeleteCommentCommand() { Id = commentId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}

