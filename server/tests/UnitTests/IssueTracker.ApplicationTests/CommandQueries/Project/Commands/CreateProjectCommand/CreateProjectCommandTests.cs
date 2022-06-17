using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.CreateProjectCommand;
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

public class CreateProjectCommandHandlerTests
{
    [Theory]
    [InlineData(ProjectProgress.Open)]
    [InlineData(ProjectProgress.Closed)]
    public async void Handle_ValidData_DbDataCorrect(ProjectProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateProjectCommand() 
        { 
            Id = "ProjectId", 
            Summary ="SummaryText", 
            Description = "DescriptionText", 
            Progress = progress,
            UserCredentials = new UserCredentials { Name = "admin" }
        };
        var handler = new CreateProjectCommandHandler(dbContext, mapper);

        // ACT
        ProjectVm projectVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Projects.Count().Should().Be(1);
        dbContext.Projects.First().Id.Should().Be("ProjectId");
        dbContext.Projects.First().Summary.Should().Be("SummaryText");
        dbContext.Projects.First().CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        dbContext.Projects.First().Description.Should().Be("DescriptionText");
        dbContext.Projects.First().CreatedBy.Should().Be("admin");
        dbContext.Projects.First().Progress.Should().Be(progress);
    }

    [Theory]
    [InlineData(ProjectProgress.Open)]
    [InlineData(ProjectProgress.Closed)]
    public async void Handle_ValidData_ReturnsCorrectData(ProjectProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateProjectCommand()
        {
            Id = "ProjectId",
            Summary = "SummaryText",
            Description = "DescriptionText",
            Progress = progress,
            UserCredentials = new UserCredentials { Name = "admin" }
        };
        var handler = new CreateProjectCommandHandler(dbContext, mapper);

        // ACT
        ProjectVm projectVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectVm.Should().NotBeNull();
        projectVm.Id.Should().Be("ProjectId");
        projectVm.Summary.Should().Be("SummaryText");
        projectVm.CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        projectVm.Description.Should().Be("DescriptionText");
        projectVm.CreatedBy.Should().Be("admin");
        projectVm.Progress.Should().Be(progress);
    }

    [Fact]
    public async void Handle_ProjectAlreadyExists_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        dbContext.Projects.Add(new Project
        {
            Id = "ProjectId",
            Summary = "01234567891",
            CreatedBy = "admin",
            Progress = ProjectProgress.Closed,
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var command = new CreateProjectCommand()
        {
            Id = "ProjectId",
            Summary = "SummaryText",
            Progress = ProjectProgress.Closed,
            UserCredentials = new UserCredentials { Id = "admin" }
        };
        var handler = new CreateProjectCommandHandler(dbContext, mapper);

        // ACT      
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());
 

        // ASSERT
        await handle.Should()
            .ThrowAsync<CreateProjectCommandException>();
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Projects).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateProjectCommand()
        {
            Id = "P", Summary = "S", Description = "D", UserCredentials = new UserCredentials { Id = "admin" }
        };

        var handler = new CreateProjectCommandHandler(dContext.Object, mapper);
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ACT
        // ASSERT
        await handle.Should().ThrowAsync<CreateProjectCommandException>()
            .Where(e => e.Message.StartsWith("Creating project"));
    }
}
