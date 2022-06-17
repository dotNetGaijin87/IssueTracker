using FluentAssertions;
using IssueTracker.Application.CommandQueries.Projects.Queries.ListProjectsQuery;
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

public class ListProjectsQueryHandlerTests
{
    private async Task<AppDbContext> GetSeededDbWith13Projects()
    {
        AppDbContext dbContext = DbHelpers.GetEmptyDb(new DateTime(2001, 1, 1, 12, 30, 5));
        dbContext.Projects.AddRange(
            new List<Project> 
            {
                new Project
                {
                    Id = "ProjectId_1",
                    Summary = "Summary_1",
                    Description = "Description_1",
                    CreatedBy = "user_1",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_2",
                    Summary = "Summary_2",
                    Description = "Description_2",
                    CreatedBy = "user_2",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_3",
                    Summary = "Summary_3",
                    Description = "Description_3",
                    CreatedBy = "user_3",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_4",
                    Summary = "Summary_4",
                    Description = "Description_4",
                    CreatedBy = "user_5",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_5",
                    Summary = "Summary_5",
                    Description = "Description_5",
                    CreatedBy = "user_5",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_6",
                    Summary = "Summary_6",
                    Description = "Description_6",
                    CreatedBy = "user_6",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_7",
                    Summary = "Summary_7",
                    Description = "Description_7",
                    CreatedBy = "user_5",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_8",
                    Summary = "Summary_8",
                    Description = "Description_8",
                    CreatedBy = "user_8",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_9",
                    Summary = "Summary_9",
                    Description = "Description_9",
                    CreatedBy = "user_9",
                    Progress = ProjectProgress.Closed,
                },
                new Project
                {
                    Id = "ProjectId_10",
                    Summary = "Summary_10",
                    Description = "Description_10",
                    CreatedBy = "user_10",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_11",
                    Summary = "Summary_11",
                    Description = "Description_11",
                    CreatedBy = "user_11",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_22",
                    Summary = "Summary_22",
                    Description = "Description_22",
                    CreatedBy = "user_22",
                    Progress = ProjectProgress.Open,
                },
                new Project
                {
                    Id = "ProjectId_13",
                    Summary = "Summary_13",
                    Description = "Description_13",
                    CreatedBy = "user_13",
                    Progress = ProjectProgress.Closed,
                },
            });

        await dbContext.SaveChangesAsync();


        return dbContext;
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async void Handle_PageSizeSmallerThanAllRecords_RecordsReturnedEqualsPageSize(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery() { Id = "ProjectId", PageSize = pageSize };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(pageSize);
        projectList.Page.Should().Be(1);
    }

    [Theory]
    [InlineData(15)]
    [InlineData(20)]
    public async void Handle_PageSizeGreaterThanAllRecords_PageCountEquals1(int pageSize)
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery() { Id = "ProjectId", PageSize = pageSize };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(13);
        projectList.Page.Should().Be(1);
        projectList.PageCount.Should().Be(1);
    }

    [Fact]
    public async void Handle_SecondaPage_Returns3Projects()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery() { Id = "ProjectId", PageSize = 10, Page = 2 };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(3);
        projectList.Page.Should().Be(2);
        projectList.PageCount.Should().Be(2);
    }

    [Fact]
    public async void Handle_FilterById_Returns2ProjectsWithCorrectData()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery()
        {
            Id = "ProjectId_2",
            PageSize = 10,
            Page = 1,
        };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(2);
        projectList.Projects[0].Id.Should().Be("ProjectId_2");
        projectList.Projects[0].Summary.Should().Be("Summary_2");
        projectList.Projects[0].Description.Should().Be(null);
        projectList.Projects[0].Progress.Should().Be(ProjectProgress.Closed);
        projectList.Projects[0].CreatedBy.Should().Be("user_2");
        projectList.Projects[0].CreationTime.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        projectList.Projects[0].CompletionTime.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));

        projectList.Projects[1].Id.Should().Be("ProjectId_22");
        projectList.Projects[1].Summary.Should().Be("Summary_22");
        projectList.Projects[1].Description.Should().Be(null);
        projectList.Projects[1].Progress.Should().Be(ProjectProgress.Open);
        projectList.Projects[1].CreatedBy.Should().Be("user_22");
        projectList.Projects[1].CreationTime.Should().Be(new DateTime(2001, 1, 1, 12, 30, 5));
        projectList.Projects[1].CompletionTime.Should().Be(null);
    }

    [Fact]
    public async void Handle_FilterByCreator_Returns3Projects()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery() 
        { 
            Id = "ProjectId", 
            PageSize = 10, 
            Page = 1,
            CreatedBy = "user_5"
        };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(3);
    }

    [Fact]
    public async void Handle_FilterByProgress_Returns6Projects()
    {
        // ARRANGE
        using AppDbContext dbContext = await GetSeededDbWith13Projects();
        var command = new ListProjectsQuery()
        {
            Id = "ProjectId",
            PageSize = 10,
            Page = 1,
            Progress = ProjectProgress.Open
        };
        var handler = new ListProjectsQueryHandler(dbContext);

        // ACT
        ProjectListVm projectList = await handler.Handle(command, new CancellationToken());

        // ASSERT
        projectList.Projects.Count().Should().Be(6);
    }

    [Fact]
    public async void Handle_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
            dContext.Setup(x => x.Projects).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new ListProjectsQuery() { Id = "ProjectId" };
        var handler = new ListProjectsQueryHandler(dContext.Object);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());


        // ASSERT
        await handle.Should().ThrowAsync<ListProjectsQueryException>()
            .Where(e => e.Message.StartsWith("Listing projects"));
    }
}