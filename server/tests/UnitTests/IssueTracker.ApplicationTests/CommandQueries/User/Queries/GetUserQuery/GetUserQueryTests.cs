using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
using IssueTracker.Application.CommandQueries.Users.Queries.GetUserQuery;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetUserQueryTests
{
    [Fact]
    public async void Handle_ValidData_ReturnedDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2002, 2, 2, 2, 2, 2));
        dbContext.Users.Add(new User
        {
            Id = "User_1",
            IsActivated = true,
            Email = "www.user_1_email@.com",
            Role = UserRole.manager,
            RegisteredOn = new DateTime(2001, 1, 1, 1, 1, 1),
            LastLoggedOn = new DateTime(2002, 2, 2, 2, 2, 2),
        });
        dbContext.SaveChanges();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetUserQuery() { Id = "User_1" };
        var handler = new GetUserQueryHandler(dbContext, mapper);

        // ACT
        UserVm user = await handler.Handle(command, new CancellationToken());

        // ASSERT
        user.Id.Should().Be("User_1");
        user.IsActivated.Should().Be(true);
        user.Email.Should().Be("www.user_1_email@.com");
        user.Role.Should().Be(UserRole.manager);
        user.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 1, 1, 1));
        user.LastLoggedOn.Should().Be(new DateTime(2002, 2, 2, 2, 2, 2));
    }

    [Fact]
    public async void Handle_UserNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2002, 2, 2, 2, 2, 2));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetUserQuery() { Id = "User_1" };
        var handler = new GetUserQueryHandler(dbContext, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetUserQueryException>()
                .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Users).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetUserQuery() { Id = "User_1" };
        var handler = new GetUserQueryHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetUserQueryException>()
                .Where(e => e.Message.StartsWith("Getting user"));
    }
}
