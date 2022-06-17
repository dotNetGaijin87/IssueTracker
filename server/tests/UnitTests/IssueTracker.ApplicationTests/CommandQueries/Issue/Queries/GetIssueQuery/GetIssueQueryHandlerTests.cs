using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueQuery;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetIssueQueryHandlerTests
{
    [Fact]
    public async void Handle_ValidData_ReturnsCorrectData()
    {
        // ARRANGE
        AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Projects.Add(new Project { Id = "Project_2", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "SummaryText1",
            Description = "DescriptionText_1",
            CreatedBy = "User_1",
            Type = IssueType.Bug,
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Low,
            CreationTime = new DateTime(2000, 9,11, 12,0,0),
            CompletionTime = new DateTime(2000, 9, 11, 13, 0, 0)
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_2",
            ProjectId = "Project_2",
            Summary = "SummaryText_2",
            Description = "DescriptionText_2",
            CreatedBy = "User_2",
            Type = IssueType.Improvement,
            Progress = IssueProgress.InProgress,
            Priority = IssuePriority.High,
            CreationTime = new DateTime(2000, 9, 11, 12, 0, 0),
            CompletionTime = new DateTime(2001, 9, 11, 13, 0, 0)
        });
        dbContext.Permissions.Add(new Permission
        {
            IssueId = "Issue_1",
            UserId = "User_1",
            IsPinnedToKanban = true,
            KanbanRowPosition = 0,
            GrantedBy = "User_1",
            IssuePermission = IssuePermission.CanModify
        });
        dbContext.Permissions.Add(new Permission
        {
            IssueId = "Issue_2",
            UserId = "User_1",
            IsPinnedToKanban = true,
            KanbanRowPosition = 1,
            GrantedBy = "User_1",
            IssuePermission = IssuePermission.CanModify,

        });
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetIssueQuery() { Id = "Issue_2", UserCredentials = new UserCredentials {Name="User_1" } };
        var handler = new GetIssueQueryHandler(dbContext, mapper);

        // ACT
        IssueVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Should().NotBeNull();
        returnedIssue.Id.Should().Be("Issue_2");
        returnedIssue.Summary.Should().Be("SummaryText_2");
        returnedIssue.Type.Should().Be(IssueType.Improvement);
        returnedIssue.Progress.Should().Be(IssueProgress.InProgress);
        returnedIssue.Priority.Should().Be(IssuePriority.High);
        returnedIssue.Description.Should().Be("DescriptionText_2");
        returnedIssue.CreatedBy.Should().Be("User_2");
        returnedIssue.CreationTime.Should().Be(new DateTime(2000, 9, 11, 12, 0, 0));
        returnedIssue.CompletionTime.Should().Be(new DateTime(2001, 9, 11, 13, 0, 0));
        returnedIssue.Comments.Count().Should().Be(0);
        returnedIssue.Permission.IssueId.Should().Be("Issue_2");
        returnedIssue.Permission.UserId.Should().Be("User_1");
        returnedIssue.Permission.IssuePermission.Should().Be(IssuePermission.CanModify);
        returnedIssue.Permission.KanbanRowPosition.Should().Be(1);
        returnedIssue.Permission.IsPinnedToKanban.Should().Be(true);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
        dbContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetIssueQuery() { UserCredentials = new UserCredentials { Name = "User_1" } };
        var handler = new GetIssueQueryHandler(dbContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetIssueQueryException>()
                .Where(e => e.Message.StartsWith("Getting issue"));
    }
}
