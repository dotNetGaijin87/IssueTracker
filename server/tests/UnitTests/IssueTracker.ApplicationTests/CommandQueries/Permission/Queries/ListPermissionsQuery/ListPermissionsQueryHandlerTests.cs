using FluentAssertions;
using IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;
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

public class ListPermissionsQueryHandlerTests
{
    private async Task<AppDbContext> GetSeededDbWith3Users5Issues12Permissions()
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(default);
        dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
        dbContext.Users.Add(new User { Id = "User_2", Email = "www.user_2_email@.com" });
        dbContext.Users.Add(new User { Id = "User_3", Email = "www.user_3_email@.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Issues.AddRange(new List<Issue>
        {
            new Issue
            {
                Id = "Issue_1",
                ProjectId = "Project_1",
                Summary = "SummaryText1",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.Canceled,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_2",
                ProjectId = "Project_1",
                Summary = "SummaryText_2",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.Closed,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_3",
                ProjectId = "Project_1",
                Summary = "SummaryText_3",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Critical
            },
            new Issue
            {
                Id = "Issue_4",
                ProjectId = "Project_1",
                Summary = "SummaryText_4",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Low
            },
            new Issue
            {
                Id = "Issue_5",
                ProjectId = "Project_1",
                Summary = "SummaryText_5",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.ToBeTested,
                Priority = IssuePriority.High
            },
        });
        dbContext.Permissions.AddRange(new List<Permission>
        {
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_1",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_2",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_3",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_1",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_2",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_3",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_4",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_3",
                IssueId = "Issue_1",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_3",
                IssueId = "Issue_2",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_3",
                IssueId = "Issue_3",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },
            new Permission
            {
                UserId = "User_3",
                IssueId = "Issue_4",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanDelete,
                KanbanRowPosition = 0,
            },            
            new Permission
            {
                UserId = "User_3",
                IssueId = "Issue_5",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 0,
            },
        });

        await dbContext.SaveChangesAsync();


        return dbContext;
    }
 
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async void Handle_PageSizeSmallerThanAllRecords_RecordsReturnedEqualsPageSize(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith3Users5Issues12Permissions();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListPermissionsQuery() { PageSize = pageSize };
        var handler = new ListPermissionsQueryHandler(dbContext, mapper);

        // ACT
        PermissionsVm permissions = await handler.Handle(command, new CancellationToken());

        // ASSERT
        permissions.Permissions.Count().Should().Be(pageSize);
        permissions.Page.Should().Be(1);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(20)]
    public async void Handle_PageSizeGreaterThanAllRecords_PageCountEquals1(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith3Users5Issues12Permissions();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListPermissionsQuery() { PageSize = pageSize };
        var handler = new ListPermissionsQueryHandler(dbContext, mapper);

        // ACT
        PermissionsVm permissions = await handler.Handle(command, new CancellationToken());

        // ASSERT
        permissions.Permissions.Count().Should().Be(12);
        permissions.Page.Should().Be(1);
        permissions.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_FirstPage_Returns3Permissions()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith3Users5Issues12Permissions();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListPermissionsQuery() { IssueId = "Issue_1", PageSize = 10, Page = 1 };
        var handler = new ListPermissionsQueryHandler(dbContext, mapper);

        // ACT
        PermissionsVm permissions = await handler.Handle(command, new CancellationToken());

        // ASSERT
        permissions.Permissions.Count().Should().Be(3);
        permissions.Page.Should().Be(1);
        permissions.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_FilterByCreator_Returns5Permissions()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith3Users5Issues12Permissions();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListPermissionsQuery() { UserId = "User_3", PageSize = 10, Page = 1 };
        var handler = new ListPermissionsQueryHandler(dbContext, mapper);

        // ACT
        PermissionsVm permissions = await handler.Handle(command, new CancellationToken());

        // ASSERT
        permissions.Permissions.Count().Should().Be(5);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
        dbContext.Setup(x => x.Permissions).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListPermissionsQuery() { UserId = "User_3", PageSize = 10, Page = 1 };
        var handler = new ListPermissionsQueryHandler(dbContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<ListPermissionsQueryException>()
            .Where(e => e.Message.StartsWith("Listing permissions"));
    }
}