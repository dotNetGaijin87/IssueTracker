using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class ListCommentsQueryValidatorTests
{
    [Fact]
    public async Task TestValidate_ValidPage_ValidationSuccess()
    {
        // ARRANGE
        var validator = new ListCommentsQueryValidator();
        var model = new ListCommentsQuery() { Page = 1};

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task TestValidate_InvalidPage_ValidationError(int page)
    {
        // ARRANGE
        var validator = new ListCommentsQueryValidator();
        var model = new ListCommentsQuery() { Page = page };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public async Task TestValidate_ValidPageSize_ValidationSuccess()
    {
        // ARRANGE
        var validator = new ListCommentsQueryValidator();
        var model = new ListCommentsQuery() { PageSize = 10 };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public async Task TestValidate_InvalidContent_ValidationError(int pageSize)
    {
        // ARRANGE
        var validator = new ListCommentsQueryValidator();
        var model = new ListCommentsQuery() { PageSize = pageSize };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}

