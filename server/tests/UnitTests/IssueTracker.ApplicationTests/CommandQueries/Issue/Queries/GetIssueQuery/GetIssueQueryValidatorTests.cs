using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;
using Xunit;

namespace IssueTracker.ApplicationTests
{
    public class GetIssueQueryValidatorTests
    {

        [Fact]
        public void TestValidate_ValidId_ValidationSuccess()
        {
            // ARRANGE
            var validator = new GetIssueQueryValidator();
            var model = new GetIssueQuery() { Id = "Issue_1" };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TestValidate_InvalidId_ValidationError(string projectId)
        {
            // ARRANGE
            var validator = new GetIssueQueryValidator();
            var model = new GetIssueQuery() { Id = projectId };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}