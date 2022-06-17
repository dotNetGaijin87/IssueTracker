using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Commands.DeleteCommentCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Infrastructure.Services.AppDbContext;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeleteCommentCommandHandlerTests
{
    [Fact]
    public async void Handle_ValidRequest_DataDeleted()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2002, 1, 1, 12, 0, 0));
        dbContext.Comments.Add(new Comment
        {
            IssueId = "Issue_1",
            UserId = "User_1",
            Content = "Content_1",
            CreationTime = new DateTime(2002, 1, 1, 12, 0, 0)
        });
        dbContext.SaveChanges();
        var id = dbContext.Comments.First().Id;
        var command = new DeleteCommentCommand()
        {
            Id = id,
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new DeleteCommentCommandHandler(dbContext);

        // ACT
        Unit returnedValue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Comments.Count().Should().Be(0);
        returnedValue.Should().Be(Unit.Value);

    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Comments).Throws(new Exception());
        var command = new DeleteCommentCommand()
        {
            Id = "Comment_1",
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new DeleteCommentCommandHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeleteCommentCommandException>()
            .Where(e => e.Message.StartsWith("Deleting comment"));
    }
}
