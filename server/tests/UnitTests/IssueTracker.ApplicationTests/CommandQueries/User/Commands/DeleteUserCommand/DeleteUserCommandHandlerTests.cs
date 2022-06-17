using FluentAssertions;
using IssueTracker.Application.CommandQueries.Users.Commands.DeleteUserCommand;
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

public class DeleteUserCommandHandlerTests
{
    [Fact]
    public async void Handle_UserExists_Success()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Users.Add( new User
        {
            Email = "www.user_1_email@.com",
            Id = "User_1",
        });
        dbContext.SaveChanges();

        var handler = new DeleteUserCommandHandler(dbContext);
        var command = new DeleteUserCommand() { Id = "User_1" };

        // ACT
        Unit result = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Users.Count().Should().Be(0);
        result.Should().Be(Unit.Value);
    }

    [Fact]
    public async void Handle_UserNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Users.Add(new User
        {
            Email = "www.user_1_email@.com",
            Id = "User_1",
        });
        dbContext.SaveChanges();
        var handler = new DeleteUserCommandHandler(dbContext);
        var command = new DeleteUserCommand() { Id = "Non_Existing_User" };

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeleteUserCommandException>()
            .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dbContext = new Mock<AppDbContext>();
            dbContext.Setup(x => x.Users).Throws(new Exception());
        var handler = new DeleteUserCommandHandler(dbContext.Object);
        var command = new DeleteUserCommand() { Id = "User_1" };

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<DeleteUserCommandException>()
            .Where(e => e.Message.StartsWith("Deleting user"));
    }
}
