using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Commands.DeleteIssueCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Infrastructure.Services.AppDbContext;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

// TO DO
public class DeleteIssueCommandHandlerTests
{
    [Fact]
    public async void Handle_IssueExists_Success()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1", 
            Summary = "SummaryText", 
            CreatedBy = "admin"
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1", 
            Summary = "SummaryText", 
            CreatedBy = "admin",
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var handler = new DeleteIssueCommandHandler(dbContext);
        var command = new DeleteIssueCommand() { Id = "Issue_1" };

        // ACT
        Unit result = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Issues.Count().Should().Be(0);
    }

    [Fact]
    public async void Handle_IssueNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1",
            Summary = "SummaryText",
            CreatedBy = "admin"
        });
        dbContext.Issues.Add(new Issue
        {
            Id = "Issue_1",
            ProjectId = "Project_1",
            Summary = "SummaryText",
            CreatedBy = "admin",
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var handler = new DeleteIssueCommandHandler(dbContext);
        var command = new DeleteIssueCommand() { Id = "Issue_2" };

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeleteIssueCommandException>()
            .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new DeleteIssueCommand()
        {
            Id = "Issue",
            UserCredentials = new UserCredentials { Id = "admin" }
        };
        var handler = new DeleteIssueCommandHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeleteIssueCommandException>()
            .Where(e => e.Message.StartsWith("Deleting issue"));
    }
}
