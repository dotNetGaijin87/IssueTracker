using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IssueTracker.IntegrationTests;

public class ProjectEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _webHost = WebHost.GetSingletonHost(nameof(ProjectEndpointTests));

    [Fact]
    public async Task Create_CorrectData_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateProjectCommand
        {
            Id = "Project_new",
            OwnedBy = "User_1",
            Summary = "New Summary",
            Description = "New Description",
            Progress = ProjectProgress.Open,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/project", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProjectVm project = HttpHelpers.GetParsedBody<ProjectVm>(response);
        project.Should().NotBeNull();
        project.Id.Should().Be("Project_new");
        project.CreatedBy.Should().Be("User_5");
        project.Summary.Should().Be("New Summary");
        project.Description.Should().Be("New Description");
        project.Progress.Should().Be(ProjectProgress.Open);
        project.CreationTime.Should().NotBe(default);
        project.CompletionTime.Should().NotBe(default);
    }

    [Theory]
    [InlineData(null, HttpStatusCode.BadRequest)]
    [InlineData("", HttpStatusCode.BadRequest)]
    public async Task Create_InvalidProjectId_ReturnsCorrectStatusAndCorrectContent(string projectId, HttpStatusCode statusCode)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateProjectCommand
        {
            Id = projectId,
            OwnedBy = "User_1",
            Summary = "New Summary",
            Description = "New Description",
            Progress = ProjectProgress.Closed,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/project", content);

        // ASSERT
        response.StatusCode.Should().Be(statusCode);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Id");
    }

    [Fact]
    public async Task Create_AlreadyExistingProjectId_ReturnsConflictStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateProjectCommand
        {
            Id = "Project_1",
            OwnedBy = "User_1",
            Summary = "New Summary",
            Description = "New Description",
            Progress = ProjectProgress.Closed,
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/project", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("already exists");
    }


    [Fact]
    public async Task Get_ExistingProject_ReturnsSuccessAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/project/Project_2");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProjectVm project = HttpHelpers.GetParsedBody<ProjectVm>(response);
        project.Should().NotBeNull();
        project.Id.Should().Be("Project_2");
        project.CreatedBy.Should().Be("User_3");
        project.Progress.Should().Be(ProjectProgress.Closed);
        project.Summary.Should().Contain("Project_2 summary");
        project.Description.Should().Contain("Project_2 description");
        project.CreationTime.Should().NotBe(default);
        project.CompletionTime.Should().NotBe(default);
    }

    [Fact]
    public async Task Get_NonexistentProject_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/project/NonexistentProject");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Fact]
    public async Task Update_ExistingProject_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateProjectCommand
        {
            Id = "Project_10",
            Summary = "New Summary",
            Description = "New Description",
            Progress = ProjectProgress.Closed,
            FieldMask = new List<string> { "Summary", "Description", "Progress" },
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/project", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProjectVm project = HttpHelpers.GetParsedBody<ProjectVm>(response);
        project.Should().NotBeNull();
        project.Id.Should().Be("Project_10");
        project.CreatedBy.Should().Be("User_6");
        project.Summary.Should().Be("New Summary");
        project.Description.Should().Be("New Description");
        project.Progress.Should().Be(ProjectProgress.Closed);
        project.CreationTime.Should().NotBe(default);
        project.CompletionTime.Should().NotBe(default);
    }

    [Theory]
    [InlineData(null, HttpStatusCode.BadRequest)]
    [InlineData("", HttpStatusCode.BadRequest)]
    public async Task Update_InvalidProjectId_ReturnsCorrectStatusAndCorrectContent(string projectId, HttpStatusCode statusCode)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateProjectCommand
        {
            Id = projectId,
            Summary = "New Summary",
            Description = "New Description",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/project", content);

        // ASSERT
        response.StatusCode.Should().Be(statusCode);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Id");
    }

    [Fact]
    public async Task Update_NonexistentProject_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateProjectCommand
        {
            Id = "NonexistentProject",
            Summary = "New Summary",
            Description = "New Description",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/project", content);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Fact]
    public async Task Delete_ExistingProject_ReturnsNoContentStatus()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/project/Project_11");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonexistentProject_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/project/NonexistentProject");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Theory]
    [InlineData("api/project")]
    [InlineData("api/project?progress=Open")]
    [InlineData("api/project?progress=Closed&id=Project_2")]
    [InlineData("api/project?progress=Closed&id=Project_1&createdBy=User_1")]
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
        HttpResponseMessage response = await client.GetAsync("api/project?id=Project_2");

        // ASSERT
        response.EnsureSuccessStatusCode();

        ProjectListVm projectListVm = HttpHelpers.GetParsedBody<ProjectListVm>(response);
        projectListVm.Should().NotBeNull();
        projectListVm.Page.Should().Be(1);
        projectListVm.PageCount.Should().Be(1);
        projectListVm.Projects.Count().Should().Be(1);

        ProjectVm project = projectListVm.Projects.First();
        project.Should().NotBeNull();
        project.Id.Should().Be("Project_2");
        project.CreatedBy.Should().Be("User_3");
        project.Summary.Should().Be("Project_2 summary");
        project.Description.Should().Be(null);
        project.Progress.Should().Be(ProjectProgress.Closed);
        project.CreationTime.Should().NotBe(default);
        project.CompletionTime.Should().NotBe(default);
    }
}
