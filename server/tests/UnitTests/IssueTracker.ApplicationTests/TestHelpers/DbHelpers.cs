using IssueTracker.Application.Intrefaces;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Collections.Generic;
using TestSupport.EfHelpers;

namespace IssueTracker.ApplicationTests.Helpers
{
    public static class DbHelpers
    {
        public static AppDbContext GetEmptyDb(DateTime snapShot)
        {
            var options = SqliteInMemory.CreateOptions<AppDbContext>();
            var dateTime = new Mock<IDateTimeSnapshot>();
            dateTime.Setup(x => x.Now).Returns(snapShot);

        var dbContext = new AppDbContext(options, dateTime.Object);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        public static AppDbContext GetDbWith3Users3Projects3Issues3permissions(DateTime time)
        {
            AppDbContext dbContext = GetEmptyDb(time);
            dbContext.Users.Add(new User { Id = "User_1", Email = "www.user_1_email@.com" });
            dbContext.Users.Add(new User { Id = "User_2", Email = "www.user_2_email@.com" });
            dbContext.Users.Add(new User { Id = "User_3", Email = "www.user_3_email@.com" });
            dbContext.Projects.Add(new Project { Id = "Project_1", Summary = "Summary", CreatedBy = "User_1" });
            dbContext.Projects.Add(new Project { Id = "Project_2", Summary = "Summary", CreatedBy = "User_2" });
            dbContext.Projects.Add(new Project { Id = "Project_3", Summary = "Summary", CreatedBy = "User_3" });
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
                    ProjectId = "Project_2",
                    Summary = "SummaryText2",
                    Description = "DescriptionText",
                    CreatedBy = "User_2",
                    Type = IssueType.Improvement,
                    Progress = IssueProgress.InProgress,
                    Priority = IssuePriority.Critical
                },
                new Issue
                {
                    Id = "Issue_3",
                    ProjectId = "Project_3",
                    Summary = "SummaryText2",
                    Description = "DescriptionText",
                    CreatedBy = "User_3",
                    Type = IssueType.Improvement,
                    Progress = IssueProgress.InProgress,
                    Priority = IssuePriority.Critical
                },
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
                    UserId = "User_3",
                    IssueId = "Issue_3",
                    IsPinnedToKanban = false,
                    IssuePermission = IssuePermission.CanModify,
                    KanbanRowPosition = 0,
                },
            });
            dbContext.SaveChanges();


            return dbContext;
        }
    }
}
