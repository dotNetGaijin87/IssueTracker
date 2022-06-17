using FluentAssertions;
using IssueTracker.Application.CommandQueries.Permissions.Queries.GetPermissionQuery;
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

public class GetPermissionQueryHandlerTests
{
    [Fact]
    public async void Handle_ValidData_ReturnsCorrectData()
    {
        // ARRANGE
        AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(default);
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_99",
            ProjectId = "Project_1",
            Summary = "SummaryText_99",
            Description = "DescriptionText_99",
            CreatedBy = "User_1",
            Type = IssueType.Improvement,
            Progress = IssueProgress.InProgress,
            Priority = IssuePriority.High,
            CreationTime = new DateTime(2000, 9, 11, 12, 0, 0),
            CompletionTime = new DateTime(2001, 9, 11, 13, 0, 0)
        });
        dbContext.Permissions.Add(new Permission
        {
            IssueId = "Issue_99",
            UserId = "User_1",
            IsPinnedToKanban = true,
            KanbanRowPosition = 0,
            GrantedBy = "User_1",
            IssuePermission = IssuePermission.CanModify
        });
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetPermissionQuery() {UserId = "User_1", IssueId = "Issue_99"};
        var handler = new GetPermissionQueryHandler(dbContext, mapper);

        // ACT
        PermissionVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Should().NotBeNull();
        returnedIssue.UserId.Should().Be("User_1");
        returnedIssue.IssueId.Should().Be("Issue_99");
        returnedIssue.IsPinnedToKanban.Should().Be(true);
        returnedIssue.KanbanRowPosition.Should().Be(0);
        returnedIssue.IssuePermission.Should().Be(IssuePermission.CanModify);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
        dbContext.Setup(x => x.Permissions).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetPermissionQuery() { UserId = "User_1", IssueId = "Issue_1" };
        var handler = new GetPermissionQueryHandler(dbContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetPermissionQueryException>()
                .Where(e => e.Message.StartsWith("Getting permission"));
    }
}
