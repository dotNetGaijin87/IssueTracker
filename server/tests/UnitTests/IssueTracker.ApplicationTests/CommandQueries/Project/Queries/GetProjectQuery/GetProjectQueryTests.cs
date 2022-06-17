using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Queries.GetProjectCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class GetProjectQueryTests
{
    [Fact]
    public async void Handle_ProjectExist_ReturnedDataCorrect()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2001,1,1,12,30,5));
        dbContext.Projects.Add(new Project
        {
            Id = "ProjectId", 
            Summary = "Summary",
            Description = "Description",
            CreatedBy = "admin",
            Progress = ProjectProgress.Closed,
        });
        await dbContext.SaveChangesAsync();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetProjectQuery() { Id = "ProjectId"  };
        var handler = new GetProjectQueryHandler(dbContext, mapper);

        // ACT
        ProjectVm projectVm = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectVm.Id.Should().Be("ProjectId");
        projectVm.Summary.Should().Be("Summary");
        projectVm.Description.Should().Be("Description");
        projectVm.CreatedBy.Should().Be("admin");
        projectVm.CreationTime.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        projectVm.Progress.Should().Be(ProjectProgress.Closed);
    }

    [Fact]
    public async void Handle_ProjectNotFound_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetEmptyDb(DateTime.MinValue);
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetProjectQuery()
        {
            Id = "ProjectId",
        };
        var handler = new GetProjectQueryHandler(dbContext, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
                .ThrowAsync<GetProjectQueryException>()
                .Where(e => e.Message.Contains("not found"));
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Projects).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new GetProjectQuery() { Id = "ProjectId" };
        var handler = new GetProjectQueryHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<GetProjectQueryException>()
            .Where(e => e.Message.StartsWith("Getting project"));
    }
}
