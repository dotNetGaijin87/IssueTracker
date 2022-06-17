using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using System.Collections.Generic;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateIssueCommandValidatorTests
{

    [Theory]
    [InlineData("abc")]
    [InlineData("1234")]
    [InlineData("a123456789123456789012345678901234567890123456789a")]
    public void TestValidate_ValidId_ValidationSuccess(string id)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { Id = id };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ab")]
    [InlineData("$")]
    [InlineData("a12345678912345678901234567890123456789012345678901")]
    public void TestValidate_InValidId_ValidationError(string id)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { Id = id };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_EmptyFieldMask_ValidationSuccess(string summary)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { Id = "Issue_1", Summary = summary };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Summary);
    }

    [Theory]
    [InlineData("abcdefghij")]
    [InlineData("1234567890")]
    [InlineData("abcdefghijklmnopqrstvwzabcdefghi")]
    public void TestValidate_ValidSummary_ValidationSuccess(string summary)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { Id = "Issue_1", Summary = summary, FieldMask = new List<string> { "Summary" } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Summary);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abcdefghi")]
    [InlineData(@"a12345678912345678901234567890123456789012345678901a12345678912345678901234567890123456789012345678901")]
    public void TestValidate_InValidSummary_ValidationError(string summary)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { Id = "Issue_1", Summary = summary, FieldMask = new List<string> { "Summary" } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Summary);
    }

    [Theory]
    [InlineData(UserRole.admin)]
    [InlineData(UserRole.manager)]
    public void TestValidate_ValidRole_ValidationSuccess(UserRole role)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand() { UserCredentials = new UserCredentials { Role = role } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }
 
    [Theory]
    [InlineData(IssuePermission.CanModify)]
    [InlineData(IssuePermission.CanDelete)]
    public void TestValidate_UserHasValidRights_ValidationSuccess(IssuePermission permission)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Add(new User { Id = "User_1", Email = "www.user1email.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Issues.Add(new Issue { Id = "Issue_1", ProjectId = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Permissions.Add(new Permission { UserId = "User_1", IssueId = "Issue_1", IssuePermission = permission });
        dbContext.SaveChanges();

        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand
        {
            Id = "Issue_1",
            UserCredentials = new UserCredentials { Role = UserRole.employee, Name = "User_1" }
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void TestValidate_UserDoesNotHaveRights_ValidationError()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Add(new User { Id = "User_1", Email = "www.user1email.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Issues.Add(new Issue { Id = "Issue_1", ProjectId = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Permissions.Add(new Permission { UserId = "User_1", IssueId = "Issue_1", IssuePermission = IssuePermission.CanDelete });
        dbContext.SaveChanges();

        var validator = new UpdateIssueCommandValidator(dbContext);
        var model = new UpdateIssueCommand
        {
            Id = "Issue_1",
            UserCredentials = new UserCredentials { Role = UserRole.employee, Name = "User_2" }
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x);
    }
}

