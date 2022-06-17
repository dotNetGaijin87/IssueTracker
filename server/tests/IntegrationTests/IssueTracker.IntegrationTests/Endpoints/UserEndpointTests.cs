using FluentAssertions;
using IssueTracker.Application.CommandQueries.Users.Commands.UpdateUserCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IssueTracker.IntegrationTests;

public class UserEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _webHost = WebHost.GetSingletonHost(nameof(UserEndpointTests));

    [Fact]
    public async Task Get_ExistingUser_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/user/User_2");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        UserVm user = HttpHelpers.GetParsedBody<UserVm>(response);
        user.Should().NotBeNull();
        user.Id.Should().Be("User_2");
        user.IsActivated.Should().Be(false);
        user.Email.Should().Be("www.user_2@email.com");
        user.RegisteredOn.Should().NotBe(default);
        user.LastLoggedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task Get_NonexistentUser_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/user/NonexistentUser");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }

    [Fact]
    public async Task Update_ExistingUser_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateUserCommand
        {
            FieldMask = new List<string> { "isActivated", "Id" },
            IsActivated = true,
            Id = "User_4"
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/user", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        UserVm user = HttpHelpers.GetParsedBody<UserVm>(response);
        user.Should().NotBeNull();
        user.Id.Should().Be("User_4");
        user.IsActivated.Should().Be(true);
        user.Role.Should().Be(UserRole.employee);
        user.Email.Should().Be("www.user_4@email.com");
        user.RegisteredOn.Should().NotBe(default);
        user.LastLoggedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task Update_NonexistentUser_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateUserCommand
        {
            FieldMask = new List<string> { "isActivated" },
            IsActivated = true,
            Id = "NonexistentUser"
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/user", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));
        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }

    [Fact]
    public async Task Delete_ExistingUser_ReturnsNoContentStatus()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/user/User_1");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonexistentUser_ReturnsNotFoundStatus()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/user/NonexistentUser");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("api/user")]
    [InlineData("api/user?id=User_1")]
    [InlineData("api/user?email=www.user_1@email.com")]
    [InlineData("api/user?id=User_1&email=www.user_1@email.com")]
    [InlineData("api/user?id=NonexistentUser")]
    public async Task List_ReturnSuccessStatusAndCorrectContentType(string url)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync(url);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));
    }

    [Fact]
    public async Task List_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/user?id=User_1");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        UserListVm usersVm = HttpHelpers.GetParsedBody<UserListVm>(response);
        usersVm.Should().NotBeNull();
        usersVm.Page.Should().Be(1);
        usersVm.PageCount.Should().Be(2);
        usersVm.Users.Count().Should().Be(10);

        UserVm user = usersVm.Users.Where(x => x.Id == "User_10").First();
        user.Should().NotBeNull();
        user.Id.Should().Be("User_10");
        user.Email.Should().Be("www.user_10@email.com");
        user.Role.Should().Be(UserRole.employee);
        user.IsActivated.Should().Be(false);
        user.RegisteredOn.Should().NotBe(default);
        user.LastLoggedOn.Should().NotBe(default);
    }
}
