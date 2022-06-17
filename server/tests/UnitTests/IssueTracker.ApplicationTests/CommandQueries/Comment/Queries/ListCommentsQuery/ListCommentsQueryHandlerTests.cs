using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Queries.ListCommentsQuery;
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


public class ListCommentsQueryHandlerTests
{
    private async Task<AppDbContext> GetDbSeededWith13Comments()
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2001, 1, 1, 12, 30, 5));
        dbContext.Users.Add(new User { Id = "User_1", Role = UserRole.admin, Email = "www.email@.com" });
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1",
            Summary = "Summary_1",
            Description = "Description_1",
            CreatedBy = "User_1",
            Progress = ProjectProgress.Closed,
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "Summary_1",
            Description = "Description_1",
            CreatedBy = "User_1",
        });

        dbContext.Comments.AddRange(new List<Comment>
        {
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_1", CreationTime = new DateTime(2001, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_2", CreationTime = new DateTime(2002, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_3", CreationTime = new DateTime(2003, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_4", CreationTime = new DateTime(2004, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_5", CreationTime = new DateTime(2005, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_6", CreationTime = new DateTime(2006, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_7", CreationTime = new DateTime(2007, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_8", CreationTime = new DateTime(2008, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_9", CreationTime = new DateTime(2009, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_10", CreationTime = new DateTime(2010, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_11", CreationTime = new DateTime(2011, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_12", CreationTime = new DateTime(2012, 1, 1, 12, 0, 0) },
            new Comment { IssueId = "Issue_1", UserId = "User_1", Content = "Content_13", CreationTime = new DateTime(2013, 1, 1, 12, 0, 0) },
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
        AppDbContext dbContext = await GetDbSeededWith13Comments();
        var query = new ListCommentsQuery() { IssueId = "Issue_1", PageSize = pageSize };
        var handler = new ListCommentsQueryHandler(dbContext);

        // ACT
        CommentListVm commentList = await handler.Handle(query, new CancellationToken());

        // ASSERT
        commentList.Comments.Count().Should().Be(pageSize);
        commentList.Page.Should().Be(1);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(20)]
    public async void Handle_PageSizeGreaterThanAllRecords_PageCountEquals1(int pageSize)
    {
        // ARRANGE
        AppDbContext dbContext = await GetDbSeededWith13Comments();
        var query = new ListCommentsQuery() { IssueId = "Issue_1", PageSize = pageSize };
        var handler = new ListCommentsQueryHandler(dbContext);

        // ACT
        CommentListVm commentList = await handler.Handle(query, new CancellationToken());

        // ASSERT
        commentList.Comments.Count().Should().Be(13);
        commentList.Page.Should().Be(1);
        commentList.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_SecondaPage_Returns3Comments()
    {
        // ARRANGE
        AppDbContext dbContext = await GetDbSeededWith13Comments();
        var query = new ListCommentsQuery() { IssueId = "Issue_1", PageSize = 10, Page = 2 };
        var handler = new ListCommentsQueryHandler(dbContext);

        // ACT
        CommentListVm commentList = await handler.Handle(query, new CancellationToken());

        // ASSERT
        commentList.Comments.Count().Should().Be(3);
        commentList.Page.Should().Be(2);
        commentList.PageCount.Should().Be(2);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Comments).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var query = new ListCommentsQuery() { IssueId = "Issue_1", PageSize = 10, Page = 2 };
        var handler = new ListCommentsQueryHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(query, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<ListCommentsQueryException>()
            .Where(e => e.Message.StartsWith("Listing comments"));
    }
}