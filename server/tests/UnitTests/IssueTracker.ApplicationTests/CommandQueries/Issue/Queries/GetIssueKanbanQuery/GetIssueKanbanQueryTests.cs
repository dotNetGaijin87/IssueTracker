using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;
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

public class GetIssueKanbanQueryTests
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
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Type = IssueType.Bug,
            Progress = IssueProgress.Canceled,
            Priority = IssuePriority.Low
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
            Priority = IssuePriority.High
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
        var command = new GetIssueKanbanQuery() { UserCredentials = new UserCredentials {Name="User_1" } };
        var handler = new GetIssueKanbanQueryHandler(dbContext, mapper);

        // ACT
        IEnumerable<KanbanCardVm> cards = await handler.Handle(command, new CancellationToken());

        // ASSERT
        cards.Should().NotBeNull();
        cards.Count().Should().Be(2);
        var card_1 = cards.Single(x => x.Id == "Issue_1");
        var card_2 = cards.Single(x => x.Id == "Issue_2");

        card_1.Should().NotBeNull();
        card_1.Id.Should().Be("Issue_1");
        card_1.ProjectId.Should().Be("Project_1");
        card_1.Summary.Should().Be("SummaryText1");
        card_1.Position.Should().Be(0);
        card_1.Type.Should().Be(IssueType.Bug);
        card_1.Progress.Should().Be(IssueProgress.Canceled);
        card_1.Priority.Should().Be(IssuePriority.Low);
        card_2.Should().NotBeNull();
        card_2.Id.Should().Be("Issue_2");
        card_2.ProjectId.Should().Be("Project_2");
        card_2.Summary.Should().Be("SummaryText2");
        card_2.Position.Should().Be(1);
        card_2.Type.Should().Be(IssueType.Improvement);
        card_2.Progress.Should().Be(IssueProgress.InProgress);
        card_2.Priority.Should().Be(IssuePriority.High);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
        dbContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetIssueKanbanQuery() { UserCredentials = new UserCredentials { Name = "User_1" } };
        var handler = new GetIssueKanbanQueryHandler(dbContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetIssueKanbanQueryException>()
                .Where(e => e.Message.StartsWith("Getting kanban"));
    }
}
