using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
using Xunit;

namespace IssueTracker.ApplicationTests
{
    public class GetProjectQueryValidatorTests
    {

        [Fact]
        public async Task TestValidate_ValidId_ValidationSuccess()
        {
            // ARRANGE
            var validator = new GetProjectQueryValidator();
            var model = new GetProjectQuery() { Id = "Project_1" };

            // ACT
            var result = await validator.TestValidateAsync(model);

            // ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task TestValidate_InvalidId_ValidationError(string projectId)
        {
            // ARRANGE
            var validator = new GetProjectQueryValidator();
            var model = new GetProjectQuery() { Id = projectId };

            // ACT
            var result = await validator.TestValidateAsync(model);

            // ASSERT
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}