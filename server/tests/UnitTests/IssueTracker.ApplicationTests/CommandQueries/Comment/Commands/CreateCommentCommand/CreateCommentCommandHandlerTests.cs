using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
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

public class CreateCommentCommandHandlerTests
{
    [Fact]
    public async void Handle_ValidData_SavedAndReturendDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateCommentCommand() 
        { 
            IssueId = "Issue_1", 
            Content = "Content_1", 
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        CreateCommentCommandHandler handler = new CreateCommentCommandHandler(dbContext, mapper);

        // ACT
        CommentVm returendCommentVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        Comment insertedComment = dbContext.Comments.Single();
        dbContext.Comments.Count().Should().Be(1);
        insertedComment.Id.Should().NotBeNullOrEmpty();
        insertedComment.Content.Should().Be("Content_1");
        insertedComment.CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        insertedComment.IssueId.Should().Be("Issue_1");
        insertedComment.UserId.Should().Be("User_1");

        returendCommentVm.Id.Should().NotBeNullOrEmpty();
        returendCommentVm.Content.Should().Be("Content_1");
        returendCommentVm.CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
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
        var command = new CreateCommentCommand()
        {
            IssueId = "Issue_1",
            Content = "Content_1",
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new CreateCommentCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<CreateCommentCommandException>()
            .Where(e => e.Message.StartsWith("Creating a comment"));
    }
}
