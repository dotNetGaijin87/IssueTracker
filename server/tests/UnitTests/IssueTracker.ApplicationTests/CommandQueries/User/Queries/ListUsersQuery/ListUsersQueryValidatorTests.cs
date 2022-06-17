using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;
using IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;
using Xunit;

namespace IssueTracker.ApplicationTests
{
    public class ListUsersQueryValidatorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void TestValidate_PageGreaterThanZero_ValidationSuccess(int page)
        {
            // ARRANGE
            var validator = new ListUsersQueryValidator();
            var model = new ListUsersQuery() { Page = page };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.Page);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void TestValidate_PageNotGreaterThanZero_ValidationError(int page)
        {
            // ARRANGE
            var validator = new ListUsersQueryValidator();
            var model = new ListUsersQuery() { Page = page };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldHaveValidationErrorFor(x => x.Page);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(20)]
        public void TestValidate_PageSizeGreaterThanOne_ValidationSuccess(int pageSize)
        {
            // ARRANGE
            var validator = new ListUsersQueryValidator();
            var model = new ListUsersQuery() { PageSize = pageSize };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(21)]
        public void TestValidate_PageSizeInvalid_ValidationError(int pageSize)
        {
            // ARRANGE
            var validator = new ListUsersQueryValidator();
            var model = new ListUsersQuery() { PageSize = pageSize };

            // ACT
            var result = validator.TestValidate(model);

            // ASSERT
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }
    }
}
