using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;
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

public class UpdateCommentCommandHandlerTests
{
    [Fact]
    public async void Handle_ValidData_SavedAndReturendDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2002, 1, 1, 12, 0, 0));
        dbContext.Comments.Add(new Comment 
        { 
            IssueId = "Issue_1", 
            UserId = "User_1", 
            Content = "Old_Content",
            CreationTime = new DateTime(2002, 1, 1, 12, 0, 0)
        });
        dbContext.SaveChanges();
        var id = dbContext.Comments.First().Id;
        dbContext.ChangeTracker.Clear();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateCommentCommand() 
        { 
            Id = id, 
            Content = "New_Content", 
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new UpdateCommentCommandHandler(dbContext, mapper);

        // ACT
        CommentVm returendCommentVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        Comment insertedComment = dbContext.Comments.Single();
        dbContext.Comments.Count().Should().Be(1);
        insertedComment.Id.Should().NotBeNullOrEmpty();
        insertedComment.Content.Should().Be("New_Content");
        insertedComment.CreationTime.Should().Be(new DateTime(2002, 1, 1, 12, 0, 0));
        insertedComment.IssueId.Should().Be("Issue_1");
        insertedComment.UserId.Should().Be("User_1");

        returendCommentVm.Id.Should().NotBeNullOrEmpty();
        returendCommentVm.Content.Should().Be("New_Content");
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
        var command = new UpdateCommentCommand()
        {
            Id = "Comment_1",
            Content = "New_Content",
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new UpdateCommentCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<UpdateCommentCommandException>()
            .Where(e => e.Message.StartsWith("Updating the comment"));
    }
}
