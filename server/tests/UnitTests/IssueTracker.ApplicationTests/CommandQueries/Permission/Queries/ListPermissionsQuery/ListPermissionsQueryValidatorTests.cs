using FluentValidation.TestHelper;
using IssueTracker.Application.CommandQueries.Permissions.Queries.ListPermissionsQuery;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using System;
using System.Collections.Generic;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class ListPermissionsQueryValidatorTests
{
    public static AppDbContext GetDbWith2Users2Projects13Issues13Permissions()
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2000, 1, 1, 12, 0, 0));
        dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
        dbContext.Users.Add(new User { Id = "User_2", Email = "www.user_2_email@.com" });
        dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
        dbContext.Projects.Add(new Project { Id = "Project_2", Summary = "Summary", CreatedBy = "User_1" });
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
                Summary = "SummaryText2",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Critical
            },
            new Issue
            {
                Id = "Issue_3",
                ProjectId = "Project_1",
                Summary = "SummaryText3",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.Closed,
                Priority = IssuePriority.Low
            },
            new Issue
            {
                Id = "Issue_4",
                ProjectId = "Project_1",
                Summary = "SummaryText4",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Low
            },
            new Issue
            {
                Id = "Issue_5",
                ProjectId = "Project_1",
                Summary = "SummaryText5",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.OnHold,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_6",
                ProjectId = "Project_1",
                Summary = "SummaryText6",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Low
            },
            new Issue
            {
                Id = "Issue_7",
                ProjectId = "Project_1",
                Summary = "SummaryText7",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.Closed,
                Priority = IssuePriority.Critical
            },
            new Issue
            {
                Id = "Issue_8",
                ProjectId = "Project_1",
                Summary = "SummaryText8",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_9",
                ProjectId = "Project_1",
                Summary = "SummaryText9",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.Canceled,
                Priority = IssuePriority.High
            },
            new Issue
            {
                Id = "Issue_10",
                ProjectId = "Project_1",
                Summary = "SummaryText10",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_11",
                ProjectId = "Project_1",
                Summary = "SummaryText11",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.ToDo,
                Priority = IssuePriority.Low
            },
            new Issue
            {
                Id = "Issue_12",
                ProjectId = "Project_1",
                Summary = "SummaryText12",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Bug,
                Progress = IssueProgress.InProgress,
                Priority = IssuePriority.Medium
            },
            new Issue
            {
                Id = "Issue_13",
                ProjectId = "Project_1",
                Summary = "SummaryText13",
                Description = "DescriptionText",
                CreatedBy = "User_1",
                Type = IssueType.Improvement,
                Progress = IssueProgress.ToBeTested,
                Priority = IssuePriority.High
            }
        });
        dbContext.Permissions.AddRange(new List<Permission>
        {
            new Permission
            {
                UserId = "User_1",
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
                UserId = "User_1",
                IssueId = "Issue_3",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 1,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_4",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 1,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_5",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 2,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_6",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 2,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_7",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 3,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_8",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 3,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_9",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 4,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_10",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 4,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_11",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 5,
            },
            new Permission
            {
                UserId = "User_2",
                IssueId = "Issue_12",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 5,
            },
            new Permission
            {
                UserId = "User_1",
                IssueId = "Issue_13",
                IsPinnedToKanban = false,
                IssuePermission = IssuePermission.CanModify,
                KanbanRowPosition = 6,
            },
        });

        dbContext.SaveChanges();


        return dbContext;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void TestValidate_PageGreaterThanZero_ValidationSuccess(int page)
    {
        // ARRANGE
        var validator = new ListPermissionsQueryValidator();
        var model = new ListPermissionsQuery() { Page = page };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void TestValidate_PageNotGreaterThanZero_ValidationError(int page)
    {
        // ARRANGE
        var validator = new ListPermissionsQueryValidator();
        var model = new ListPermissionsQuery() { Page = page };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    public void TestValidate_PageSizeGreaterThanOne_ValidationSuccess(int pageSize)
    {
        // ARRANGE
        var validator = new ListPermissionsQueryValidator();
        var model = new ListPermissionsQuery() { PageSize = pageSize };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void TestValidate_PageSizeInvalid_ValidationError(int pageSize)
    {
        // ARRANGE
        var validator = new ListPermissionsQueryValidator();
        var model = new ListPermissionsQuery() { PageSize = pageSize };

        // ACT
        var result = validator.TestValidate(model);

        // ASSERT
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}

