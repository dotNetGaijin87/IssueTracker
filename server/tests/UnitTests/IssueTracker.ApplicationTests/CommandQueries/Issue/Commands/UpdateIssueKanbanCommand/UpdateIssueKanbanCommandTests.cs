using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueKanbanCommand;
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

public class UpdateIssueKanbanCommandTests
{
    [Theory]
    [InlineData(IssueProgress.Canceled)]
    [InlineData(IssueProgress.Closed)]
    [InlineData(IssueProgress.InProgress)]
    [InlineData(IssueProgress.OnHold)]
    [InlineData(IssueProgress.ToBeTested)]
    [InlineData(IssueProgress.ToDo)]
    public async void Handle_ValidData_SavesAndReturnsCorrectData(IssueProgress newProgress)
    {
        // ARRANGE
        AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "SummaryText",
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Progress = IssueProgress.Canceled,
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
        dbContext.Projects.Add(new Project { Id = "Project_2", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_2",
            ProjectId = "Project_2",
            Summary = "SummaryText",
            Description = "DescriptionText",
            CreatedBy = "User_1",
            Progress = IssueProgress.Canceled,
        });
        dbContext.Permissions.Add(new Permission
        {
            IssueId = "Issue_2",
            UserId = "User_1",
            IsPinnedToKanban = true,
            KanbanRowPosition = 0,
            GrantedBy = "User_1",
            IssuePermission = IssuePermission.CanModify,

        });
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateIssueKanbanCommand()
        {
            IssueId = "Issue_1",
            UserCredentials = new UserCredentials { Name = "User_1"},
            Progress = newProgress,
            Permissions = new List<Permission> 
            { 
                new Permission { IssueId = "Issue_1" , KanbanRowPosition = 1, IsPinnedToKanban = true },
                new Permission { IssueId = "Issue_2" , KanbanRowPosition = 2, IsPinnedToKanban = false }
            },
        };
        var handler = new UpdateIssueKanbanCommandHandler(dbContext, mapper);

        // ACT
        IEnumerable<KanbanCardVm> kanbanCards = await handler.Handle(command, new CancellationToken());

        // ASSERT
        Issue savedIssue = dbContext.Issues.Single(x => x.Id == "Issue_1");
        savedIssue.Id.Should().Be("Issue_1");
        savedIssue.Progress.Should().Be(newProgress);

        dbContext.Permissions.Count().Should().Be(2);
        Permission savedPermission_1 = dbContext.Permissions.Single(x => x.IssueId == "Issue_1");
        Permission savedPermission_2 = dbContext.Permissions.Single(x => x.IssueId == "Issue_2");
        savedPermission_1.IsPinnedToKanban.Should().Be(true);
        savedPermission_1.KanbanRowPosition.Should().Be(1);
        savedPermission_2.IsPinnedToKanban.Should().Be(false);
        savedPermission_2.KanbanRowPosition.Should().Be(2);

        kanbanCards.Count().Should().Be(2);
        var modifiedIssue = kanbanCards.Single(x => x.Id == "Issue_1");
        modifiedIssue.Progress.Should().Be(newProgress);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
        dbContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateIssueKanbanCommand()
        {
            IssueId = "Issue_1",
            UserCredentials = new UserCredentials { Name = "User_1" },
            Permissions = new List<Permission>
            {
                new Permission { IssueId = "Issue_1" , KanbanRowPosition = 1, IsPinnedToKanban = true },
            },
        };
        var handler = new UpdateIssueKanbanCommandHandler(dbContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<UpdateIssueKanbanCommandException>()
                .Where(e => e.Message.StartsWith("Updating kanban"));
    }
}
