using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.DeleteProjectCommand;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using MediatR;
using Moq;

namespace IssueTracker.ApplicationTests;

public class DeleteProjectCommandHandlerTests
{
    [Fact]
    public async void Handle_ProjectExists_Success()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "ProjectId", Summary = "01234567891", CreatedBy = "admin", Progress = ProjectProgress.Open,
        });
        dbContext.SaveChanges();
        var handler = new DeleteProjectCommandHandler(dbContext);
        var command = new DeleteProjectCommand() { Id = "ProjectId" };

        // ACT
        Unit result = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Projects.Count().Should().Be(0);
        result.Should().Be(Unit.Value);
    }


    [Fact]
    public async void Handle_ProjectNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "Project_1", Summary = "SummaryText", CreatedBy = "admin"
        });
        dbContext.SaveChanges();
        var handler = new DeleteProjectCommandHandler(dbContext);
        var command = new DeleteProjectCommand() { Id = "Project_2" };

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<DeleteProjectCommandException>()
            .Where(e => e.Message.Contains("not found"));
    }


    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Projects).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new DeleteProjectCommand()
        {
            Id = "ProjectId",
            UserCredentials = new UserCredentials { Id = "admin" }
        };
        var handler = new DeleteProjectCommandHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<DeleteProjectCommandException>()
            .Where(e => e.Message.StartsWith("Deleting project"));
    }
}
