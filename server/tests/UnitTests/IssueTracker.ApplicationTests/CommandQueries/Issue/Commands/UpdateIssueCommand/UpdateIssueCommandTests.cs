using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdateIssueCommandTests
{
    [Theory]
    [InlineData(IssueProgress.Canceled)]
    [InlineData(IssueProgress.Closed)]
    [InlineData(IssueProgress.InProgress)]
    [InlineData(IssueProgress.OnHold)]
    [InlineData(IssueProgress.ToBeTested)]
    [InlineData(IssueProgress.ToDo)]
    public async void Handle_ValidData_DbDataCorrect(IssueProgress newProgress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1", Summary = "SummaryText", CreatedBy = "User_1",
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "Old_Summary",
            Description = "Old_Description",
            CreatedBy = "User_1",
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Low,
            Type = IssueType.Bug
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateIssueCommand()
        {
            Id = "Issue_1",
            Summary = "New_Summary",
            Description = "New_Description",
            Progress = newProgress,
            FieldMask = new List<string> { "Summary" , "Progress",  },
            Priority = IssuePriority.Critical,
            Type = IssueType.Improvement
        };
        var handler = new UpdateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm issueVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        Issue savedIssue = dbContext.Issues.First();
        savedIssue.Id.Should().Be("Issue_1");
        savedIssue.Summary.Should().Be("New_Summary");
        savedIssue.Description.Should().Be("Old_Description");
        savedIssue.Progress.Should().Be(newProgress);
        savedIssue.Priority.Should().Be(IssuePriority.Low);
        savedIssue.Type.Should().Be(IssueType.Bug);
    }

    [Theory]
    [InlineData(IssuePriority.Low)]
    [InlineData(IssuePriority.Medium)]
    [InlineData(IssuePriority.High)]
    [InlineData(IssuePriority.Critical)]
    public async void Handle_ValidData_ReturnedDataCorrect(IssuePriority newPriority)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1",
            Summary = "SummaryText",
            CreatedBy = "User_1",
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "SummaryText",
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Low,
            Type = IssueType.Bug
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateIssueCommand()
        {
            Id = "Issue_1",
            FieldMask = new List<string> { "Priority", },
            Priority = newPriority,
        };
        var handler = new UpdateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm issueVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        issueVm.Id.Should().Be("Issue_1");
        issueVm.Summary.Should().Be("SummaryText");
        issueVm.Description.Should().Be("DescriptionText");
        issueVm.Progress.Should().Be(IssueProgress.Canceled);
        issueVm.Priority.Should().Be(newPriority);
        issueVm.Type.Should().Be(IssueType.Bug);
        issueVm.CreatedBy.Should().Be("User_1");
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateIssueCommand()
        {
            Id = "Issue_1",
            Summary = "SummaryText",
            Description = "DescriptionText",
            UserCredentials = new UserCredentials { Id = "admin" }
        };
        var handler = new UpdateIssueCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<UpdateIssueCommandException>()
                .Where(e => e.Message.StartsWith("Updating issue"));
    }
}
