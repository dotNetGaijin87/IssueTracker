using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
using IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;
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

public class UpdateUserCommandTests
{
    [Fact]
    public async void Handle_ValidData_ReturnedAndSavedDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2002, 2, 2, 2, 2, 2));
        dbContext.Users.Add(new User
        {
            Id = "User_1",
            IsActivated = true,
            Email = "www.user_1_email@.com",
            Role = UserRole.manager,
            RegisteredOn = new DateTime(2001, 1,1,1,1,1),
        });
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateUserCommand()
        {
            Id = "User_1",
            FieldMask = new List<string>() {"IsActivated" },
            IsActivated = false,
        };
        var handler = new UpdateUserCommandHandler(dbContext, mapper);

        // ACT
        UserVm returnedUser = await handler.Handle(command, new CancellationToken());

        // ASSERT
        User dbUser = dbContext.Users.Single();
        dbUser.Id.Should().Be("User_1");
        dbUser.IsActivated.Should().Be(false);
        dbUser.Email.Should().Be("www.user_1_email@.com");

        returnedUser.Id.Should().Be("User_1");
        returnedUser.IsActivated.Should().Be(false);
        returnedUser.Email.Should().Be("www.user_1_email@.com");
        returnedUser.Role.Should().Be(UserRole.manager);
        returnedUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 1, 1, 1));
        returnedUser.LastLoggedOn.Should().Be(new DateTime(2002, 2, 2, 2, 2, 2));
    }

    [Fact]
    public async void Handle_UserNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2002, 2, 2, 2, 2, 2));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateUserCommand()
        {
            Id = "User_1",
            FieldMask = new List<string>() { "IsActivated" },
            IsActivated = false,
        };
        var handler = new UpdateUserCommandHandler(dbContext, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<UpdateUserCommandException>()
                .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Users).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateUserCommand()
        {
            Id = "User_1",
            FieldMask = new List<string>() { "IsActivated" },
            IsActivated = false,
        };
        var handler = new UpdateUserCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<UpdateUserCommandException>()
                .Where(e => e.Message.StartsWith("Updating user"));
    }
}
