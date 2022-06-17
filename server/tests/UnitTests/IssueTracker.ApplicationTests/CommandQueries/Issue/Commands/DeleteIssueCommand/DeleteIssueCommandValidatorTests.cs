using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeleteIssueCommandValidatorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TestValidate_InValidId_ValidationError(string id)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new DeleteIssueCommandValidator(dbContext);
        var model = new DeleteIssueCommand() 
        { 
            Id = id, 
            UserCredentials = new UserCredentials { Role = UserRole.admin} 
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }


    [Theory]
    [InlineData(UserRole.admin)]
    [InlineData(UserRole.manager)]
    public void TestValidate_ValidRole_ValidationSuccess(UserRole role)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var validator = new DeleteIssueCommandValidator(dbContext);
        var model = new DeleteIssueCommand() { UserCredentials = new UserCredentials { Role = role } };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.UserCredentials.Role);
    }


    [Fact]
    public void TestValidate_UserCreatedTheIssue_ValidationSuccess()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Add(new User { Id = "User_1", Email ="www.user1email.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Issues.Add(new Issue { Id = "Issue_1",ProjectId = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Permissions.Add(new Permission { UserId = "User_1", IssueId = "Issue_1", IssuePermission = IssuePermission.CanDelete, });
        dbContext.SaveChanges();
 
        var validator = new DeleteIssueCommandValidator(dbContext);
        var model = new DeleteIssueCommand
        { 
            Id = "Issue_1",
            UserCredentials = new UserCredentials { Role = UserRole.employee, Name ="User_1" } 
        };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x);
    }


    [Fact]
    public void TestValidate_UserDidNotCreateTheIssue_ValidationError()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Add(new User { Id = "User_1", Email = "www.user1email.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Issues.Add(new Issue { Id = "Issue_1", ProjectId = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Permissions.Add(new Permission { UserId = "User_1", IssueId = "Issue_1", IssuePermission = IssuePermission.CanDelete, });
        dbContext.SaveChanges();


        var validator = new DeleteIssueCommandValidator(dbContext);
        var model = new DeleteIssueCommand
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

