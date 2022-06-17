using FluentAssertions;
using IssueTracker.Application.CommandQueries.Permissions.Commands.CreatePermissionCommand;
using IssueTracker.Application.CommandQueries.Permissions.Commands.UpdatePermissionCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IssueTracker.IntegrationTests;

public class PermissionEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _webHost = WebHost.GetSingletonHost(nameof(PermissionEndpointTests));

    [Fact]
    public async Task Create_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreatePermissionCommand
        {
            UserId = "User_20",
            IssueId = "Issue_3",
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanModify,
        };


        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/permission", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        PermissionVm permission = HttpHelpers.GetParsedBody<PermissionVm>(response);
        permission.Should().NotBeNull();
        permission.UserId.Should().Be("User_20");
        permission.IssueId.Should().Be("Issue_3");
        permission.IsPinnedToKanban.Should().Be(true);
        permission.KanbanRowPosition.Should().Be(0);
        permission.IssuePermission.Should().Be(IssuePermission.CanModify);
    }

    [Theory]
    [InlineData(null, "Issue_3")]
    [InlineData("User_20", null)]
    public async Task Create_InvalidUserIdOrIssueId_ReturnsBadRequestStatusAndProblemDetails(string userId, string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreatePermissionCommand
        {
            UserId = userId,
            IssueId = issueId,
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanModify,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/permission", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Id");
    }

    [Fact]
    public async Task Create_AlreadyExistingPermission_ReturnsConflictStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreatePermissionCommand
        {
            UserId = "User_2",
            IssueId = "Issue_1",
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanModify,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/permission", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("already exists");
    }


    [Fact]
    public async Task Get_ExistingPermission_ReturnsSuccessAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/permission/User_1/Issue_6");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        PermissionVm permission = HttpHelpers.GetParsedBody<PermissionVm>(response);
        permission.Should().NotBeNull();
        permission.UserId.Should().Be("User_1");
        permission.IssueId.Should().Be("Issue_6");
        permission.IsPinnedToKanban.Should().Be(false);
        permission.KanbanRowPosition.Should().Be(0);
        permission.IssuePermission.Should().Be(IssuePermission.CanDelete);
    }

    [Theory]
    [InlineData("NonexistentUser", "Issue_1")]
    [InlineData("User_1", "NonexistentIssue")]
    public async Task Get_NonexistentPermission_ReturnsNotFoundStatusAndProblemDetails(string userId, string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync($"api/permission/{userId}/{issueId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Fact]
    public async Task Update_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdatePermissionCommand
        {
            UserId = "User_21",
            IssueId = "Issue_3",
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanDelete,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/permission", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        PermissionVm permission = HttpHelpers.GetParsedBody<PermissionVm>(response);
        permission.Should().NotBeNull();
        permission.UserId.Should().Be("User_21");
        permission.IssueId.Should().Be("Issue_3");
        permission.IsPinnedToKanban.Should().Be(true);
        permission.KanbanRowPosition.Should().Be(1);
        permission.IssuePermission.Should().Be(IssuePermission.CanDelete);
    }

    [Theory]
    [InlineData("NonexistentUser", "Issue_1")]
    [InlineData("User_1", "NonexistentIssue")]
    public async Task Update_NonexistentPermission_ReturnsNotFoundStatusAndProblemDetails(string userId, string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdatePermissionCommand
        {
            UserId = userId,
            IssueId = issueId,
            IsPinnedToKanban = true,
            IssuePermission = IssuePermission.CanDelete,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/permission", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Fact]
    public async Task Delete_ReturnsNoContentStatus()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/permission/User_22/Issue_2");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("NonexistentUser", "Issue_1")]
    [InlineData("User_1", "NonexistentIssue")]
    public async Task Delete_NonexistentPermission_ReturnsNotFoundStatusAndProblemDetails(string userId, string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync($"api/permission/{userId}/{issueId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Theory]
    [InlineData("api/permission")]
    [InlineData("api/permission?UserId=User_3")]
    [InlineData("api/permission?IssueId=Issue_2")]
    [InlineData("api/permission?UserId=User_1&IssueId=Issue_1")]
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
        HttpResponseMessage response = await client.GetAsync("api/permission?UserId=User_3");

        // ASSERT
        response.EnsureSuccessStatusCode();

        PermissionsVm permissionsVm = HttpHelpers.GetParsedBody<PermissionsVm>(response);
        permissionsVm.Should().NotBeNull();
        permissionsVm.Permissions.Count().Should().Be(3);
        permissionsVm.Page.Should().Be(1);
        permissionsVm.PageCount.Should().Be(1);

        PermissionVm permission = permissionsVm.Permissions
            .Where(x => x.UserId == "User_3" && x.IssueId == "Issue_42")
            .Single();

        permission.IsPinnedToKanban.Should().Be(true);
        permission.KanbanRowPosition.Should().Be(2);
        permission.IssuePermission.Should().Be(IssuePermission.CanDelete);
    }
}
