using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Queries.GetCommentQuery;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetCommentQueryHandlerTests
{
    [Fact]
    public async void Handle_ValidRequest_ReturnsCorrectData()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2000, 1, 1, 12, 0, 0));
        dbContext.Comments.Add(new Comment
        {
            IssueId = "Issue_1",
            UserId = "User_1",
            Content = "Content_1",
            CreationTime = new DateTime(2002, 1, 1, 12, 0, 0)
        });
        dbContext.SaveChanges();
        var id = dbContext.Comments.First().Id;
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetCommentQuery() 
        { 
            Id = id, 
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new GetCommentQueryHandler(dbContext, mapper);

        // ACT
        CommentVm returendCommentVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returendCommentVm.Id.Should().Be(id);
        returendCommentVm.Content.Should().Be("Content_1");
        returendCommentVm.CreationTime.Should().Be(new DateTime(2002, 1, 1, 12, 0, 0));
        returendCommentVm.IssueId.Should().Be("Issue_1");
        returendCommentVm.UserId.Should().Be("User_1");
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Comments).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetCommentQuery()
        {
            Id = "Comment_1",
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new GetCommentQueryHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<GetCommentQueryException>()
            .Where(e => e.Message.StartsWith("Getting comment"));
    }
}
