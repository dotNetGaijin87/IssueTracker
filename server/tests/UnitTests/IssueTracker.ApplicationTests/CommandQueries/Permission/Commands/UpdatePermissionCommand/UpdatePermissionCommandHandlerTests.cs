using FluentAssertions;
using IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class UpdatePermissionCommandHandlerTests
{
    [Theory]
    [InlineData(true, IssuePermission.None)]
    [InlineData(true, IssuePermission.CanModify)]
    [InlineData(false, IssuePermission.CanDelete)]
    public async void Handle_ValidData_ReturnedAndSavedDataCorrect(bool isPinnedToKanban, IssuePermission issuePermission)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(default);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdatePermissionCommand()
        {
            UserId = "User_1",
            IssueId = "Issue_1",
            IsPinnedToKanban = isPinnedToKanban,
            IssuePermission = issuePermission,
            UserCredentials = new UserCredentials { Name = "User_2" }         
        };
        var handler = new UpdatePermissionCommandHandler(dbContext, mapper);

        // ACT
        PermissionVm returnedPermission = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedPermission.UserId.Should().Be("User_1");
        returnedPermission.IssueId.Should().Be("Issue_1");
        returnedPermission.IsPinnedToKanban.Should().Be(isPinnedToKanban);
        returnedPermission.IssuePermission.Should().Be(issuePermission);
        returnedPermission.KanbanRowPosition.Should().Be(0);

        dbContext.Permissions.Count().Should().Be(3); 
        Permission savedPermission = dbContext.Permissions
            .Where(x => x.UserId == "User_1" && x.IssueId == "Issue_1")
            .Single();

        savedPermission.UserId.Should().Be("User_1");
        savedPermission.IssueId.Should().Be("Issue_1");
        savedPermission.IsPinnedToKanban.Should().Be(isPinnedToKanban);
        savedPermission.IssuePermission.Should().Be(issuePermission);
        savedPermission.KanbanRowPosition.Should().Be(0);
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdatePermissionCommand()
        {
            UserId = "User_1",
            IssueId = "Issue_1",
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanModify,
            UserCredentials = new UserCredentials { Name = "User_1" }
        };
        var handler = new UpdatePermissionCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
            .ThrowAsync<UpdatePermissionCommandException>()
            .Where(e => e.Message.StartsWith("Updating permission"));
    }
}
