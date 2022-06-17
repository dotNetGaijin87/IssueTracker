using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class CreateIssueCommandValidatorTests
{

    [Theory]
    [InlineData("abc")]
    [InlineData("abcdefgh")]
    [InlineData("01234567890123456789012345678901234567890123456789")]
    public void TestValidate_ValidId_ValidationSuccess(string id)
    {
        // ARRANGE
        var validator = new CreateIssueCommandValidator();
        var model = new CreateIssueCommand() { Id = id };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("012345678901234567890123456789012345678901234567890")]
    public void TestValidate_InValidId_ValidationError(string id)
    {
        // ARRANGE
        var validator = new CreateIssueCommandValidator();
        var model = new CreateIssueCommand() { Id = id };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData("01234567890123456789")]
    [InlineData("abcdefghijklm")]
    public void TestValidate_ValidSummary_ValidationSuccess(string summary)
    {
        // ARRANGE
        var validator = new CreateIssueCommandValidator();
        var model = new CreateIssueCommand() { Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Summary);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456789")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891")]
    public void TestValidate_InvalidSummary_ValidationError(string summary)
    {
        // ARRANGE
        var validator = new CreateIssueCommandValidator();
        var model = new CreateIssueCommand() { Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Summary);
    }
}

