using FluentAssertions;
using IssueTracker.Application.CommandQueries.Permissions.Commands.DeletePermissionCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Infrastructure.Services.AppDbContext;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DeletePermissionCommandHandlerTests
{
    [Fact]
    public async void Handle_IssueExists_Success()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(default);
        var handler = new DeletePermissionCommandHandler(dbContext);
        var command = new DeletePermissionCommand() { UserId ="User_1", IssueId = "Issue_1" };

        // ACT
        Unit result = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Permissions.Count().Should().Be(2);
    }

    [Fact]
    public async void Handle_IssueNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(default);
        var handler = new DeletePermissionCommandHandler(dbContext);
        var command = new DeletePermissionCommand() { UserId = "User_1", IssueId = "NotFound_Issue" };

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeletePermissionCommandException>()
            .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new DeletePermissionCommand() { UserId = "User_1", IssueId = "Issue_1" };
        var handler = new DeletePermissionCommandHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeletePermissionCommandException>()
            .Where(e => e.Message.StartsWith("Deleting permission"));
    }
}
