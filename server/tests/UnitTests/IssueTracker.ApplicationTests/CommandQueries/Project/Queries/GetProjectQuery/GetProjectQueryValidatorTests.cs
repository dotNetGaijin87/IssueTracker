using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
using Xunit;

namespace IssueTracker.ApplicationTests
{
    public class GetProjectQueryValidatorTests
    {

        [Fact]
        public void TestValidate_ValidId_ValidationSuccess()
        {
            // ARRANGE
            var validator = new GetProjectQueryValidator();
            var model = new GetProjectQuery() { Id = "Project_1" };

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
            var validator = new GetProjectQueryValidator();
            var model = new GetProjectQuery() { Id = projectId };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }
    }
}