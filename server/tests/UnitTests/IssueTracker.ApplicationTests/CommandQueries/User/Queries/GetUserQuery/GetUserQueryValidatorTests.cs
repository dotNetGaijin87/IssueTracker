using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetUserQueryValidatorTests
{
    [Fact]
    public async Task TestValidate_ValidId_ValidationSuccess()
    {
        // ARRANGE
        var validator = new GetUserQueryValidator();
        var model = new GetUserQuery() { Id = "User_1" };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task TestValidate_InvalidId_ValidationError(string userId)
    {
        // ARRANGE
        var validator = new GetUserQueryValidator();
        var model = new GetUserQuery() { Id = userId };

        // ACT
        var result = await validator.TestValidateAsync(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}
