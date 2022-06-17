using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;
using IssueTracker.Application.CommandQueries.Users.Queries.ListUsersQuery;
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

public class ListUsersQueryHandlerTests
{
    private async Task<AppDbContext> GetSeededDbWith13Users(DateTime time)
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(time);
        dbContext.Users.AddRange(
            new List<User> 
            {
                new User { Id = "User_01", Email = "www.user_01_email@.com",   Role = UserRole.admin, IsActivated = true, RegisteredOn = new DateTime(2010,1,1,1,1,1), LastLoggedOn = new DateTime(2020,1,1,1,1,1) },
                new User { Id = "User_02", Email = "www.user_02_email@.com",   Role = UserRole.manager, IsActivated = true, RegisteredOn = new DateTime(2010,1,2,1,1,1), LastLoggedOn = new DateTime(2020,1,2,1,1,1)  },
                new User { Id = "User_03", Email = "www.user_03_email@.com",   Role = UserRole.employee, IsActivated = false, RegisteredOn = new DateTime(2010,1,3,1,1,1), LastLoggedOn = new DateTime(2020,1,3,1,1,1)  },
                new User { Id = "User_04", Email = "www.user_04_email@.com",   Role = UserRole.employee, IsActivated = true, RegisteredOn = new DateTime(2010,1,4,1,1,1), LastLoggedOn = new DateTime(2020,1,4,1,1,1)  },
                new User { Id = "User_05", Email = "www.user_05_email@.com",   Role = UserRole.manager, IsActivated = true , RegisteredOn = new DateTime(2010,1,5,1,1,1), LastLoggedOn = new DateTime(2020,1,5,1,1,1) },
                new User { Id = "User_06", Email = "www.user_06_email@.com",   Role = UserRole.employee, IsActivated = false, RegisteredOn = new DateTime(2010,1,6,1,1,1), LastLoggedOn = new DateTime(2020,1,6,1,1,1)  },
                new User { Id = "User_07", Email = "www.user_07_email@.com",   Role = UserRole.manager, IsActivated = true, RegisteredOn = new DateTime(2010,1,7,1,1,1), LastLoggedOn = new DateTime(2020,1,7,1,1,1)  },
                new User { Id = "User_08", Email = "www.user_08_email@.com",   Role = UserRole.employee, IsActivated = true, RegisteredOn = new DateTime(2010,1,8,1,1,1), LastLoggedOn = new DateTime(2020,1,8,1,1,1)  },
                new User { Id = "User_09", Email = "www.user_09_email@.com",   Role = UserRole.admin, IsActivated = false, RegisteredOn = new DateTime(2010,1,9,1,1,1), LastLoggedOn = new DateTime(2020,1,9,1,1,1)  },
                new User { Id = "User_10", Email = "www.user_10_email@.com",   Role = UserRole.employee, IsActivated = true, RegisteredOn = new DateTime(2010,1,10,1,1,1), LastLoggedOn = new DateTime(2020,1,10,1,1,1)  },
                new User { Id = "User_11", Email = "www.user_11_email@.com",   Role = UserRole.employee, IsActivated = true, RegisteredOn = new DateTime(2010,1,11,1,1,1), LastLoggedOn = new DateTime(2020,1,11,1,1,1)  },
                new User { Id = "User_12", Email = "www.user_12_email@.com",   Role = UserRole.admin, IsActivated = false, RegisteredOn = new DateTime(2010,1,12,1,1,1), LastLoggedOn = new DateTime(2020,1,12,1,1,1)  },
                new User { Id = "User_13", Email = "www.user_13_email@.com",   Role = UserRole.employee, IsActivated = true , RegisteredOn = new DateTime(2010,1,13,1,1,1), LastLoggedOn = new DateTime(2020,1,13,1,1,1) },
            });

        await dbContext.SaveChangesAsync();


        return dbContext;
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async void Handle_PageSizeSmallerThanAllRecords_RecordsReturnedEqualsPageSize(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery() { PageSize = pageSize };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(pageSize);
        userList.Page.Should().Be(1);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(20)]
    public async void Handle_PageSizeGreaterThanAllRecords_PageCountEquals1(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery() { PageSize = pageSize };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(13);
        userList.Page.Should().Be(1);
    }

    [Fact]
    public async void Handle_SecondaPage_Returns4Users()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery() { Id = "User_1", PageSize = 10, Page = 1 };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(4);
        userList.Page.Should().Be(1);
        userList.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_3rdPage_Returns0Users()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery() { Id = "User_1", PageSize = 10, Page = 3 };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(0);
        userList.Page.Should().Be(3);
        userList.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_FilterById_Returns9Users()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(new DateTime(2001, 1, 1, 12, 30, 5));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery()
        {
            Id = "User_0",
            PageSize = 10,
            Page = 1,
        };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(9);
        UserVm firstUser = userList.Users.First();
        UserVm secondUser = userList.Users.Skip(1).Take(1).First();


        firstUser.Id.Should().Be("User_01");
        firstUser.Email.Should().Be("www.user_01_email@.com");
        firstUser.Role.Should().Be(UserRole.admin);
        firstUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        firstUser.LastLoggedOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));

        secondUser.Id.Should().Be("User_02");
        secondUser.Email.Should().Be("www.user_02_email@.com");
        secondUser.Role.Should().Be(UserRole.manager);
        secondUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        secondUser.LastLoggedOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
    }

    [Fact]
    public async void Handle_FilterByEmail_Returns4Users()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Users(new DateTime(2001, 1, 1, 12, 30, 5));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery()
        {
            Email = "www.user_1",
            PageSize = 10,
            Page = 1,
        };
        var handler = new ListUsersQueryHandler(dbContext, mapper);

        // ACT
        UserListVm userList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        userList.Users.Count().Should().Be(4);
        UserVm firstUser = userList.Users.First();
        UserVm secondUser = userList.Users.Skip(1).Take(1).First();


        firstUser.Id.Should().Be("User_10");
        firstUser.Email.Should().Be("www.user_10_email@.com");
        firstUser.Role.Should().Be(UserRole.employee);
        firstUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        firstUser.LastLoggedOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));

        secondUser.Id.Should().Be("User_11");
        secondUser.Email.Should().Be("www.user_11_email@.com");
        secondUser.Role.Should().Be(UserRole.employee);
        secondUser.RegisteredOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        secondUser.LastLoggedOn.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Users).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListUsersQuery() { Id = "User_1" };
        var handler = new ListUsersQueryHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<ListUsersQueryException>()
            .Where(e => e.Message.StartsWith("Listing users"));
    }
}