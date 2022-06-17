using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Commands.UpdateProjectCommand;
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

public class UpdateProjectCommandTests
{
    [Theory]
    [InlineData(ProjectProgress.Open)]
    [InlineData(ProjectProgress.Closed)]
    public async void Handle_ValidData_DbDataCorrect(ProjectProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        dbContext.Projects.Add(new Project
        {
            Id = "ProjectId",
            Summary = "Old Summary",
            Description = "Old Description",
            CreatedBy = "admin",
            Progress = ProjectProgress.Open,
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateProjectCommand()
        {
            Id = "ProjectId",
            Summary = "New Summary",
            Description = "New Description",
            Progress = progress,
            FieldMask = new List<string> { "Summary", "Description", "Progress",  },
        };
        var handler = new UpdateProjectCommandHandler(dbContext, mapper);

        // ACT
        ProjectVm projectVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        dbContext.Projects.Count().Should().Be(1);
        dbContext.Projects.First().Id.Should().Be("ProjectId");
        dbContext.Projects.First().Summary.Should().Be("New Summary");
        dbContext.Projects.First().Description.Should().Be("New Description");
        dbContext.Projects.First().Progress.Should().Be(progress);
    }

    [Theory]
    [InlineData(ProjectProgress.Open)]
    [InlineData(ProjectProgress.Closed)]
    public async void Handle_ValidData_ReturnedDataCorrect(ProjectProgress progress)
    {
        // ARRANGE
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MaxValue);
        dbContext.Projects.Add(new Project
        {
            Id = "ProjectId",
            Summary = "Old Summary",
            Description = "Old Description",
            CreatedBy = "admin",
            Progress = ProjectProgress.Open,
        });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var command = new UpdateProjectCommand()
        {
            Id = "ProjectId",
            Summary = "New Summary",
            Description = "New Description",
            Progress = progress,
            FieldMask = new List<string> { "Summary", "Description", "Progress", },
        };
        var handler = new UpdateProjectCommandHandler(dbContext, mapper);

        // ACT
        ProjectVm projectVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectVm.Id.Should().Be("ProjectId");
        projectVm.CreatedBy.Should().Be("admin");
        projectVm.CreationTime.Should().NotBe(null);
        projectVm.Summary.Should().Be("New Summary");
        projectVm.Description.Should().Be("New Description");
        projectVm.Progress.Should().Be(progress);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Projects).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new UpdateProjectCommand()
        {
            Id = "P",
            Summary = "S",
            Description = "D",
            UserCredentials = new UserCredentials { Id = "admin" }
        };
        var handler = new UpdateProjectCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<UpdateProjectCommandException>()
                .Where(e => e.Message.StartsWith("Updating project"));
    }
}
