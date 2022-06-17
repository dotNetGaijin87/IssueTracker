using FluentAssertions;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using System.Linq;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class AppDbContextTests
{

    [Fact]
    public async void SaveChangesAsync_ProjectProgressOpen_CreationTimeCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2005, 5, 5, 12, 0, 0));
        var project = new Project() 
        { 
            Id = "ProjectId", 
            Summary ="SummaryText", 
            Description = "DescriptionText", 
            Progress = ProjectProgress.Open,
        };

        // ACT
        dbContext.Add(project);
        await dbContext.SaveChangesAsync();

        // ASSERT
        Project afterSaveProject = dbContext.Projects.Single(p => p.Id == project.Id);
        afterSaveProject.CreationTime.Should().Be(new DateTime(2005, 5, 5, 12, 0, 0));
        afterSaveProject.CompletionTime.Should().Be(null);
    }

    [Fact]
    public async void SaveChangesAsync_ProjectProgressClosed_CompletionTimeCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2005, 5, 5, 12, 0, 0));
        var project = new Project()
        {
            Id = "ProjectId",
            Summary = "SummaryText",
            Description = "DescriptionText",
            Progress = ProjectProgress.Closed,
        };

        // ACT
        dbContext.Add(project);
        await dbContext.SaveChangesAsync();

        // ASSERT
        Project afterSaveProject = dbContext.Projects.Single(p => p.Id == project.Id);
        afterSaveProject.CreationTime.Should().Be(new DateTime(2005, 5, 5, 12, 0, 0));
        afterSaveProject.CompletionTime.Should().Be(new DateTime(2005, 5, 5, 12, 0, 0));
    }

    [Theory]
    [InlineData(IssueProgress.ToDo)]
    [InlineData(IssueProgress.ToBeTested)]
    [InlineData(IssueProgress.OnHold)]
    [InlineData(IssueProgress.InProgress)]
    public async void SaveChangesAsync_IssueNotDone_CompletionTimeNull(IssueProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2005, 6, 6, 12, 0, 0));
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        var issueBefore = new Issue()
        {
            Id = "IssueId",
            ProjectId = "Project_1",
            Summary = "SummaryText",
            Description = "DescriptionText",
            Progress = progress,
        };

        // ACT
        dbContext.Add(issueBefore);
        await dbContext.SaveChangesAsync();

        // ASSERT
        Issue issueAfter = dbContext.Issues.Single(p => p.Id == issueBefore.Id);
        issueAfter.CreationTime.Should().Be(new DateTime(2005, 6, 6, 12, 0, 0));
        issueAfter.CompletionTime.Should().Be(null);
    }

    [Theory]
    [InlineData(IssueProgress.Canceled)]
    [InlineData(IssueProgress.Closed)]
    public async void SaveChangesAsync_IssueDone_CompletionTimeNotNull(IssueProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2005, 6, 6, 12, 0, 0));
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        var issueBefore = new Issue()
        {
            Id = "IssueId",
            ProjectId = "Project_1",
            Summary = "SummaryText",
            Description = "DescriptionText",
            Progress = progress,
        };

        // ACT
        dbContext.Add(issueBefore);
        await dbContext.SaveChangesAsync();

        // ASSERT
        Issue issueAfter = dbContext.Issues.Single(p => p.Id == issueBefore.Id);
        issueAfter.CreationTime.Should().Be(new DateTime(2005, 6, 6, 12, 0, 0));
        issueAfter.CompletionTime.Should().Be(new DateTime(2005, 6, 6, 12, 0, 0));
    }

    [Fact]
    public async void SaveChangesAsync_PermissionAdded_CreationTimeCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2005, 5, 5, 12, 0, 0));
        dbContext.Projects.Add(new Project { Id = "Project_1", CreatedBy = "User_1", Summary = "summary", });
        dbContext.Add(new Issue { Id = "IssueId",ProjectId = "Project_1", Summary = "Summary" });
        dbContext.Add(new User { Id = "UserId", Email = "www.email.com", Role = UserRole.admin });
        await dbContext.SaveChangesAsync();
        var permission = new Permission()
        {
            UserId = "UserId",
            IssueId = "IssueId",
            IssuePermission = IssuePermission.CanDelete
        };

        // ACT
        dbContext.Add(permission);
        await dbContext.SaveChangesAsync();

        // ASSERT
        Permission afterSavePermission = dbContext.Permissions.Single(x => x.UserId == permission.UserId && x.IssueId == permission.IssueId);
        afterSavePermission.CreationTime.Should().Be(new DateTime(2005, 5, 5, 12, 0, 0));
    }

}
