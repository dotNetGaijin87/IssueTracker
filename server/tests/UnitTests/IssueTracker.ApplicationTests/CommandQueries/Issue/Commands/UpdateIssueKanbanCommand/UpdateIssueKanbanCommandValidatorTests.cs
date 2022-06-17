using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateIssueKanbanCommandValidatorTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData("1234")]
    [InlineData("a123456789123456789012345678901234567890123456789a")]
    public void TestValidate_ValidId_ValidationSuccess(string issueId)
    {
        // ARRANGE
        var validator = new UpdateIssueKanbanCommandValidator();
        var model = new UpdateIssueKanbanCommand() { IssueId = issueId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InValidId_ValidationError(string issueId)
    {
        // ARRANGE
        var validator = new UpdateIssueKanbanCommandValidator();
        var model = new UpdateIssueKanbanCommand() { IssueId = issueId };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.IssueId);
    }

    [Theory]
    [InlineData(UserRole.admin)]
    [InlineData(UserRole.manager)]
    [InlineData(UserRole.employee)]
    public void TestValidate_ValidRole_ValidationSuccess(UserRole role)
    {
        // ARRANGE
        var validator = new UpdateIssueKanbanCommandValidator();
        var model = new UpdateIssueKanbanCommand() { UserCredentials = new UserCredentials { Role = role } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }
 
}

