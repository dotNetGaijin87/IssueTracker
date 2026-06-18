using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;
using IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetCommentQueryValidatorTests
{
    [Fact]
    public async Task TestValidate_ValidId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new GetCommentQueryValidator();
        var model = new GetCommentQuery() { Id = "Comment_1" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InvalidId_ValidationError(string commentId)
    {
        // ARRANGE
        var validator = new GetCommentQueryValidator();
        var model = new GetCommentQuery() { Id = commentId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}

