using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Queries.ListIssuesQuery;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class ListIssuesCommandHandlerTests
{
    private AppDbContext GetDbWith3Users2Projects10Issues(DateTime time)
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(time);
        dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
        dbContext.Users.Add(new User { Id = "User_2", Email = "www.user_2_email@.com" });
        dbContext.Users.Add(new User { Id = "User_3", Email = "www.user_3_email@.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Projects.Add(new Project { Id = "Project_2", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "SummaryText1",
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Type = IssueType.Improvement,
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Medium
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_2",
            ProjectId = "Project_2",
            Summary = "SummaryText2",
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Type = IssueType.Improvement,
            Progress = IssueProgress.InProgress,
            Priority = IssuePriority.Low
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_3",
            ProjectId = "Project_2",
            Summary = "SummaryText_3",
            Description = "DescriptionText_3",
            CreatedBy = "User_3",
            Type = IssueType.Improvement,
            Progress = IssueProgress.Closed,
            Priority = IssuePriority.High
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_4",
            ProjectId = "Project_2",
            Summary = "SummaryText_4",
            Description = "DescriptionText_4",
            CreatedBy = "User_4",
            Type = IssueType.Improvement,
            Progress = IssueProgress.Closed,
            Priority = IssuePriority.High
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_5",
            ProjectId = "Project_2",
            Summary = "SummaryText_5",
            Description = "DescriptionText_5",
            CreatedBy = "User_5",
            Type = IssueType.Bug,
            Progress = IssueProgress.ToBeTested,
            Priority = IssuePriority.High
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_6",
            ProjectId = "Project_2",
            Summary = "SummaryText_6",
            Description = "DescriptionText_6",
            CreatedBy = "User_6",
            Type = IssueType.Improvement,
            Progress = IssueProgress.ToDo,
            Priority = IssuePriority.Critical
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_7",
            ProjectId = "Project_2",
            Summary = "SummaryText_7",
            Description = "DescriptionText_7",
            CreatedBy = "User_7",
            Type = IssueType.Improvement,
            Progress = IssueProgress.InProgress,
            Priority = IssuePriority.High
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_8",
            ProjectId = "Project_2",
            Summary = "SummaryText_8",
            Description = "DescriptionText_8",
            CreatedBy = "User_8",
            Type = IssueType.Bug,
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Low
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_9",
            ProjectId = "Project_2",
            Summary = "SummaryText_9",
            Description = "DescriptionText_9",
            CreatedBy = "User_9",
            Type = IssueType.Improvement,
            Progress = IssueProgress.OnHold,
            Priority = IssuePriority.Medium
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_10",
            ProjectId = "Project_2",
            Summary = "SummaryText_10",
            Description = "DescriptionText_10",
            CreatedBy = "User_10",
            Type = IssueType.Improvement,
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Critical
        });
        dbContext.SaveChanges();


        return dbContext;
    }

    [Theory]
    [InlineData(IssueType.Bug, 2)]
    [InlineData(IssueType.Improvement, 8)]
    public async void Handle_FilteringByIssueType(IssueType type, int issueCount)
    {
        // ARRANGE
        using AppDbContext dbContext = GetDbWith3Users2Projects10Issues(default);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListIssuesQuery { Type = type };
        var handler = new ListIssuesQueryHandler(dbContext, mapper);

        // ACT
        IssueListVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Should().NotBeNull();
        returnedIssue.Issues.Count().Should().Be(issueCount);
    }

    [Theory]
    [InlineData(IssueProgress.ToDo, 1)]
    [InlineData(IssueProgress.InProgress, 2)]
    [InlineData(IssueProgress.ToBeTested, 1)]
    [InlineData(IssueProgress.OnHold, 1)]
    [InlineData(IssueProgress.Canceled, 3)]
    [InlineData(IssueProgress.Closed, 2)]
    public async void Handle_FilteringByIssueProgress(IssueProgress progress, int issueCount)
    {
        // ARRANGE
        using AppDbContext dbContext = GetDbWith3Users2Projects10Issues(default);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListIssuesQuery { Progress = progress };
        var handler = new ListIssuesQueryHandler(dbContext, mapper);

        // ACT
        IssueListVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Should().NotBeNull();
       returnedIssue.Issues.Count().Should().Be(issueCount);
    }

    [Theory]
    [InlineData(IssuePriority.Low, 2)]
    [InlineData(IssuePriority.Medium, 2)]
    [InlineData(IssuePriority.High, 4)]
    [InlineData(IssuePriority.Critical, 2)]
    public async void Handle_FilteringByIssuePriority(IssuePriority priority, int issueCount)
    {
        // ARRANGE
        using AppDbContext dbContext = GetDbWith3Users2Projects10Issues(default);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListIssuesQuery { Priority = priority };
        var handler = new ListIssuesQueryHandler(dbContext, mapper);

        // ACT
        IssueListVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Should().NotBeNull();
        returnedIssue.Issues.Count().Should().Be(issueCount);
    }
}
