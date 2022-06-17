using FluentAssertions;
using IssueTracker.Application.CommandQueries.Comments.Commands.CreateCommentCommand;
using IssueTracker.Application.CommandQueries.Comments.Commands.UpdateCommentCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IssueTracker.IntegrationTests;

public class CommentEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    WebApplicationFactory<Program> _webHost = WebHost.GetSingletonHost(nameof(CommentEndpointTests));

    [Fact]
    public async Task Create_ReturnsSuccessStatusAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateCommentCommand
        {
            UserId = "User_5",
            IssueId = "Issue_1",
            Content = "Some content",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/comment", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        CommentVm comment = HttpHelpers.GetParsedBody<CommentVm>(response);
        comment.Should().NotBeNull();
        comment.UserId.Should().Be("User_5");
        comment.IssueId.Should().Be("Issue_1");
        comment.Content.Should().Be("Some content");
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData("Issue_1", null)]
    [InlineData("", "Some comment")]
    public async Task Create_InvalidData_ReturnsBadRequestStatusAndProblemDetails(string issueId, string content)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateCommentCommand
        {
            UserId = "User_5",
            IssueId = issueId,
            Content = content,
        };
        HttpContent bodyContent = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/comment", bodyContent);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().ContainAny(new List<string> { "Issue", "Content" });
    }


    [Fact]
    public async Task Get_ReturnsSuccessAndCorrectContent()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/comment/Comment_1");

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        CommentVm comment = HttpHelpers.GetParsedBody<CommentVm>(response);
        comment.Should().NotBeNull();
        comment.UserId.Should().Be("User_1");
        comment.IssueId.Should().Be("Issue_1");
        comment.Content.Should().Be("This should be resolved by the end of the month.");
    }

    [Fact]
    public async Task Get_NonexistentComment_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.GetAsync("api/comment/NonexistentComment");

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
        var body = new UpdateCommentCommand
        {
            Id = "Comment_2",
            Content = "New content",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/comment", content);

        // ASSERT
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        CommentVm comment = HttpHelpers.GetParsedBody<CommentVm>(response);
        comment.Should().NotBeNull();
        comment.UserId.Should().Be("User_3");
        comment.IssueId.Should().Be("Issue_1");
        comment.Content.Should().Be("New content");
        comment.CreationTime.Should().NotBe(default);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData("Comment_1", null)]
    [InlineData("Comment_1", "")]
    [InlineData(null, "Some comment")]
    [InlineData("", "Some comment")]
    public async Task Update_InvalidData_ReturnsBadRequestStatusAndProblemDetailsType(string id, string content)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateCommentCommand
        {
            Id = id,
            Content = content,
        };
        HttpContent bodyContent = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/comment", bodyContent);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Issue");
    }

    [Fact]
    public async Task Update_NonexistentComment_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new UpdateCommentCommand
        {
            Id = "NonexistentComment",
            Content = "New content",
        };
        HttpContent content = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PatchAsync("api/comment", content);

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
        HttpResponseMessage response = await client.DeleteAsync("api/comment/Comment_3");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_NonexistentComment_ReturnsNotFoundStatusAndProblemDetails()
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();

        // ACT
        HttpResponseMessage response = await client.DeleteAsync("api/comment/NonexistentComment");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Error");
        problemDetails.Detail.Should().Contain("not found");
    }


    [Theory]
    [InlineData("api/comment?IssueId=Issue_1")]
    [InlineData("api/comment?IssueId=Issue_1&Content=someContent")]
    [InlineData("api/comment?IssueId=Issue_1&Content=someContent&createdBy=User_1")]
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
        HttpResponseMessage response = await client.GetAsync("api/comment?IssueId=Issue_2");

        // ASSERT
        response.EnsureSuccessStatusCode();

        CommentListVm commentsVm = HttpHelpers.GetParsedBody<CommentListVm>(response);
        commentsVm.Should().NotBeNull();
        commentsVm.Page.Should().Be(1);
        commentsVm.PageCount.Should().Be(1);
        commentsVm.Comments.Count().Should().Be(3);

        CommentVm comment = commentsVm.Comments.Where(x => x.Id == "Comment_5").Single();
        comment.Should().NotBeNull();
        comment.UserId.Should().Be("User_4");
        comment.IssueId.Should().Be("Issue_2");
        comment.Content.Should().Be("I was able to take care of that issue, though.");
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task List_InvalidIssueId_ReturnsBadRequestStatusAndProblemDetails(string issueId)
    {
        // ARRANGE
        HttpClient client = _webHost.CreateClient();
        var body = new CreateCommentCommand
        {
            UserId = "User_5",
            IssueId = issueId,
            Content = "Some content",
        };
        HttpContent bodyContent = HttpHelpers.CreatetHttpContent(body);

        // ACT
        HttpResponseMessage response = await client.PostAsync("api/comment", bodyContent);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        Assert.Equal("application/problem+json; charset=utf-8", HttpHelpers.ToStringContentType(response));

        ProblemDetails problemDetails = HttpHelpers.GetParsedBody<ProblemDetails>(response);
        problemDetails.Should().NotBeNull();
        problemDetails.Title.Should().Contain("Validation Error");
        problemDetails.Detail.Should().Contain("Issue");
    }

}
