using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.Application.CommandQueries.Users.Commands.CreateUserSafelyCommand;
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

public class CreateUserSafelyCommandHandlerTests
{
    [Theory]
    [InlineData(UserRole.admin,true)]
    [InlineData(UserRole.manager, false)]
    [InlineData(UserRole.employee, false)]
    public async void Handle_UserLogsForTheFirstTime_DbAndReturendDataCorrect(UserRole role, bool isActivated)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateUserSafelyCommand() 
        {
            UserCredentials = new UserCredentials() 
            { 
                Email ="www.userEmail@.com",
                Id = "UserId_1",
                Name = "User_1",
                Role = role
            }
        };
        var handler = new CreateUserSafelyCommandHandler(dbContext, mapper);

        // ACT
        UserVm returnedUserVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Users.Count().Should().Be(1);
        User savedUser = dbContext.Users.Single();
        savedUser.Id.Should().Be("User_1");
        savedUser.IsActivated.Should().Be(isActivated);
        savedUser.Email.Should().Be("www.userEmail@.com");
        savedUser.Role.Should().Be(role);
        savedUser.RegisteredOn.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        savedUser.LastLoggedOn.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));

        returnedUserVm.Id.Should().Be("User_1");
        returnedUserVm.IsActivated.Should().Be(isActivated);
        returnedUserVm.Email.Should().Be("www.userEmail@.com");
        returnedUserVm.Role.Should().Be(role);
        returnedUserVm.RegisteredOn.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        returnedUserVm.LastLoggedOn.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
    }

    [Fact]
    public async void Handle_UserLogsNthTime_DbAndReturendDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2003, 3, 3, 3, 3, 3));
        dbContext.Users.AddRange(
            new User
            {
                Email = "www.user_1_email@.com",
                Id = "User_1",
                Role = UserRole.manager,
                IsActivated = true,
                RegisteredOn = new DateTime(2001, 1, 1, 1, 1, 1),
                LastLoggedOn = new DateTime(2002, 2, 2, 2, 2, 2),
            },
            new User
            {
                Email = "www.user_2_email@.com",
                Id = "User_2",
                Role = UserRole.employee,
                IsActivated = false,
                RegisteredOn = new DateTime(2001, 1, 1, 1, 1, 1),
                LastLoggedOn = new DateTime(2002, 2, 2, 2, 2, 2),
            }
        );
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateUserSafelyCommand()
        {
            UserCredentials = new UserCredentials()
            {
                Id = "UserId_2",
                Name = "User_2",
                Email = "www.user_2_email@.com",
                Role = UserRole.employee
            }
        };
        var handler = new CreateUserSafelyCommandHandler(dbContext, mapper);

        // ACT
        UserVm returnedUserVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Users.Count().Should().Be(2);
        User updatedUser = dbContext.Users.Where(x => x.Id == "User_2").Single();
        updatedUser.IsActivated.Should().Be(false);
        updatedUser.Email.Should().Be("www.user_2_email@.com");
        updatedUser.Role.Should().Be(UserRole.employee);
        updatedUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 1, 1, 1));
        updatedUser.LastLoggedOn.Should().Be(new DateTime(2003, 3, 3, 3, 3, 3));

        returnedUserVm.Id.Should().Be("User_2");
        returnedUserVm.IsActivated.Should().Be(false);
        returnedUserVm.Email.Should().Be("www.user_2_email@.com");
        returnedUserVm.Role.Should().Be(UserRole.employee);
        returnedUserVm.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 1, 1, 1));
        returnedUserVm.LastLoggedOn.Should().Be(new DateTime(2003, 3, 3, 3, 3, 3));
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Users).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateUserSafelyCommand()
        {
            UserCredentials = new UserCredentials()
            {
                Id = "UserId_1",
                Name = "User_1",
                Email = "www.user_1_email@.com",
                Role = UserRole.manager
            }
        };
        var handler = new CreateUserSafelyCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<CreateUserSafelyCommandException>()
            .Where(e => e.Message.StartsWith("Creating user"));
    }
}
