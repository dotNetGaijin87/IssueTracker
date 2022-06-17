using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Commands.UpdateIssueCommand;
using IssueTracker.Application.CommandQueries.Issues.Queries.GetIssueKanbanQuery;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IssueTracker.IntegrationTests;

public class IssueEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _webHost = WebHost.GetSingletonHost(nameof(IssueEndpointTests));

    [Fact]
    public async Task Create_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateIssueCommand
        {
            Id = "New Issue",
            ProjectId = "Project_2",
            ResponsibleBy = new List<string> { "User_1" },
            Summary = "New Summary",
            Description = "New Description",
            Type = IssueType.Improvement,
            Priority = IssuePriority.High,
            Progress = IssueProgress.Closed,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/issue", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        IssueVm issue = HttpHelpers.GetParsedBody<IssueVm>(response);
        issue.Should().NotBeNull();
        issue.Id.Should().Be("New Issue");
        issue.ProjectId.Should().Be("Project_2");
        issue.Summary.Should().Be("New Summary");
        issue.Description.Should().Be("New Description");
        issue.Type.Should().Be(IssueType.Improvement);
        issue.Priority.Should().Be(IssuePriority.High);
        issue.Progress.Should().Be(IssueProgress.Closed);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_InvalidIssueId_ReturnsCorrectStatusAndCorrectContent(string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateIssueCommand
        {
            Id = issueId,
            ProjectId = "Project_1",
            ResponsibleBy = new List<string> { "User_1" },
            Summary = "New Summary",
            Description = "New Description",
            Type = IssueType.Improvement,
            Priority = IssuePriority.High,
            Progress = IssueProgress.Closed,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/issue", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Id");
    }

    [Fact]
    public async Task Create_AlreadyExistingIssueId_ReturnsConflictStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateIssueCommand
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            ResponsibleBy = new List<string> { "User_1" },
            Summary = "New Summary",
            Description = "New Description",
            Type = IssueType.Improvement,
            Priority = IssuePriority.High,
            Progress = IssueProgress.Closed,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/issue", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("already exists");
    }

    [Fact]
    public async Task Get_ReturnsSuccessCorrectContentTypeAndBody()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/issue/Issue_5");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        IssueVm issue = HttpHelpers.GetParsedBody<IssueVm>(response);
        issue.Should().NotBeNull();
        issue.Id.Should().Be("Issue_5");
        issue.ProjectId.Should().Be("Project_1");
        issue.CreatedBy.Should().Be("User_1");
        issue.Priority.Should().Be(IssuePriority.Critical);
        issue.Progress.Should().Be(IssueProgress.ToDo);
        issue.Type.Should().Be(IssueType.Improvement);
        issue.Summary.Should().Contain("Issue_5 summary");
        issue.CreationTime.Should().NotBe(default);
        issue.CompletionTime.Should().NotBe(default);
        issue.Permission.Should().Be(null);
    }

    [Fact]
    public async Task Get_NonexistentIssue_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/issue/NonexistentIssue");

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
        var body = new UpdateIssueCommand
        {
            Id = "Issue_1",
            Summary = "New Summary",
            Description = "New Description",
            Type = IssueType.Bug,
            Priority = IssuePriority.Critical,
            Progress = IssueProgress.InProgress,
            FieldMask = new List<string> { "Summary", "Description", "Type", "Priority", "Progress" },
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/issue", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        IssueVm issue = HttpHelpers.GetParsedBody<IssueVm>(response);
        issue.Should().NotBeNull();
        issue.Id.Should().Be("Issue_1");
        issue.Summary.Should().Be("New Summary");
        issue.Description.Should().Be("New Description");
        issue.Type.Should().Be(IssueType.Bug);
        issue.Priority.Should().Be(IssuePriority.Critical);
        issue.Progress.Should().Be(IssueProgress.InProgress);
    }

    [Theory]
    [InlineData(null, HttpStatusCode.BadRequest)]
    [InlineData("", HttpStatusCode.BadRequest)]
    public async Task Update_InvalidIssueId_ReturnsCorrectStatusAndCorrectContent(string issueId, HttpStatusCode statusCode)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateIssueCommand
        {
            Id = issueId,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/issue", content);

        // ASSERT
        response.StatusCode.Should().Be(statusCode);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Id");
    }

    [Fact]
    public async Task Update_NonexistentIssueId_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateIssueCommand
        {
            Id = "NonexistentIssue",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/issue", content);

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
        HttpResponseMessage response = await client.DeleteAsync("api/issue/Issue_11");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonexistentIssue_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/issue/NonexistentIssue");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }

    [Theory]
    [InlineData("api/issue")]
    [InlineData("api/issue?ProjectId=Project_1")]
    [InlineData("api/issue?ProjectId=Project_1&Type=Bug")]
    [InlineData("api/issue?ProjectId=Project_1&Progress=InProgress")]
    public async Task List_ReturnsSuccessStatusAndCorrectContentType(string url)
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
        HttpResponseMessage response = await client.GetAsync("api/issue?ProjectId=Project_1");

        // ASSERT
        response.EnsureSuccessStatusCode();

        IssueListVm IssueListVm = HttpHelpers.GetParsedBody<IssueListVm>(response);
        IssueListVm.Should().NotBeNull();
        IssueListVm.Page.Should().Be(1);
        IssueListVm.PageCount.Should().Be(1);
        IssueListVm.Issues.Count().Should().Be(6);

        IssueVm issue = IssueListVm.Issues.Where(x => x.Id == "Issue_3").First();
        issue.Id.Should().Be("Issue_3");
        issue.ProjectId.Should().Be("Project_1");
        issue.Summary.Should().Be("Issue_3 summary");
        issue.Description.Should().Be("Issue_3 description");
        issue.Type.Should().Be(IssueType.Improvement);
        issue.Progress.Should().Be(IssueProgress.ToBeTested);
        issue.Priority.Should().Be(IssuePriority.High);
        issue.CreatedBy.Should().Be("User_3");
        issue.CreationTime.Should().NotBe(default);
        issue.CompletionTime.Should().NotBe(default);
    }


    [Fact]
    public async Task GetIssueKanban_ReturnSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new GetIssueKanbanQuery
        {
            UserCredentials = new UserCredentials { Id = "User_5", Name = "User_5", Email = "www.user_5@email.com" }
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/issue/:GetIssueKanban", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        IEnumerable<KanbanCardVm> canbanCards = HttpHelpers.GetParsedBody<IEnumerable<KanbanCardVm>>(response);
        canbanCards.Should().NotBeNull();

        KanbanCardVm card = canbanCards.Where(x => x.Id == "Issue_3").Single();
        card.Id.Should().Be("Issue_3");
        card.Position.Should().Be(0);
        card.Priority.Should().Be(IssuePriority.High);
        card.Progress.Should().Be(IssueProgress.ToBeTested);
        card.Type.Should().Be(IssueType.Improvement);
        card.ProjectId.Should().Be("Project_1");
        card.Summary.Should().Be("Issue_3 summary");
    }
}
