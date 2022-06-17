using FluentAssertions;
using IssueTracker.Application.CommandQueries.Issues.Commands.CreateIssueCommand;
using IssueTracker.Application.Models;
using IssueTracker.ApplicationTests.Helpers;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using IssueTracker.Infrastructure.Services.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class CreateIssueCommandHandlerTests
{
    [Theory]
    [InlineData(IssueProgress.ToDo)]
    [InlineData(IssueProgress.InProgress)]
    [InlineData(IssueProgress.ToBeTested)]
    [InlineData(IssueProgress.OnHold)]
    [InlineData(IssueProgress.Canceled)]
    [InlineData(IssueProgress.Closed)]
    public async void Handle_VariousIssueProgress_ReturnedAndSavedDataCorrect(IssueProgress progress)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            ProjectId = "Project_1",
            Summary = "Summary",
            Description = "Description",
            Progress = progress,
            Type = IssueType.Bug,
            Priority = IssuePriority.Low,
            UserCredentials = new UserCredentials { Name = "User_1" }
            
        };
        var handler = new CreateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Id.Should().Be("Issue_11");
        returnedIssue.Summary.Should().Be("Summary");
        returnedIssue.CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        returnedIssue.Description.Should().Be("Description");
        returnedIssue.CreatedBy.Should().Be("User_1");
        returnedIssue.Progress.Should().Be(progress);
        returnedIssue.Type.Should().Be(IssueType.Bug);
        returnedIssue.Priority.Should().Be(IssuePriority.Low);

        Issue savedIssue = dbContext.Issues.Where(x => x.Id == "Issue_11").Single();
        dbContext.Issues.Count().Should().Be(4);
        savedIssue.Id.Should().Be("Issue_11");
        savedIssue.Summary.Should().Be("Summary");
        savedIssue.CreationTime.Should().Be(new DateTime(2000, 1, 1, 12, 0, 0));
        savedIssue.Description.Should().Be("Description");
        savedIssue.CreatedBy.Should().Be("User_1");
        savedIssue.Progress.Should().Be(progress);
        savedIssue.Type.Should().Be(IssueType.Bug);
        savedIssue.Priority.Should().Be(IssuePriority.Low);
    }

    [Theory]
    [InlineData(IssueType.Bug)]
    [InlineData(IssueType.Improvement)]
    public async void Handle_VariousIssueType_ReturnedDataCorrect(IssueType type)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            ProjectId = "Project_1",
            Summary = "Summary",
            Type = type,
            UserCredentials = new UserCredentials { Name = "User_1" }

        };
        var handler = new CreateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Type.Should().Be(type);
    }

    [Theory]
    [InlineData(IssuePriority.Low)]
    [InlineData(IssuePriority.Medium)]
    [InlineData(IssuePriority.High)]
    [InlineData(IssuePriority.Critical)]
    public async void Handle_VariousIssuePriority_ReturnedDataCorrect(IssuePriority priority)
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2000, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            ProjectId = "Project_1",
            Summary = "Summary",
            Description = "Description",
            Priority = priority,
            UserCredentials = new UserCredentials { Name = "User_2" }

        };
        var handler = new CreateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm returnedIssue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        returnedIssue.Priority.Should().Be(priority);
    }

    [Fact]
    public async void Handle_CorrectPermissionsAreSaved()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(new DateTime(2002, 1, 1, 12, 0, 0));
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            ProjectId = "Project_1",
            Summary = "Summary",
            Description = "Description",
            Progress = IssueProgress.Canceled,
            UserCredentials = new UserCredentials   { Name = "User_1"   },
            ResponsibleBy = new List<string>        { "User_2", "User_3" },
        };
        var handler = new CreateIssueCommandHandler(dbContext, mapper);

        // ACT
        IssueVm issue = await handler.Handle(command, new CancellationToken());

        // ASSERT
        Issue savedIssue = dbContext.Issues.Where(x => x.Id == "Issue_11").Include(x => x.Permissions).First();
        savedIssue.Permissions.Count.Should().Be(3);
        Permission permission1 = savedIssue.Permissions.Where(x => x.UserId == "User_1").Single();
        Permission permission2 = savedIssue.Permissions.Where(x => x.UserId == "User_2").Single();
        Permission permission3 = savedIssue.Permissions.Where(x => x.UserId == "User_3").Single();

        permission1.IssuePermission.Should().Be(IssuePermission.CanDelete);
        permission2.IssuePermission.Should().Be(IssuePermission.CanModify);
        permission3.IssuePermission.Should().Be(IssuePermission.CanModify);
        permission1.GrantedBy.Should().Be("User_1");
        permission2.GrantedBy.Should().Be("User_1");
        permission3.GrantedBy.Should().Be("User_1");
        permission1.IsPinnedToKanban.Should().Be(true);
        permission2.IsPinnedToKanban.Should().Be(true);
        permission3.IsPinnedToKanban.Should().Be(true);
        permission1.CreationTime.Should().Be(new DateTime(2002, 1, 1, 12, 0, 0));
        permission2.CreationTime.Should().Be(new DateTime(2002, 1, 1, 12, 0, 0));
        permission3.CreationTime.Should().Be(new DateTime(2002, 1, 1, 12, 0, 0));
    }

    [Fact]
    public async void Handle_IssueAlreadyExist_ThrowsException()
    {
        // ARRANGE
        using AppDbContext dbContext = DbHelpers.GetDbWith3Users3Projects3Issues3permissions(default);
        dbContext.Issues.Add(new Issue { Id = "Issue_11", ProjectId = "Project_1", Summary = "Summary" });
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            ProjectId = "Project_1",
            Summary = "Summary",
            Description = "Description",
            UserCredentials = new UserCredentials { Name = "User_1" },
        };
        var handler = new CreateIssueCommandHandler(dbContext, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should()
            .ThrowAsync<CreateIssueCommandException>();
    }

    [Fact]
    public async void Handle_ExceptionRaised_ThrowsException()
    {
        // ARRANGE
        var dContext = new Mock<AppDbContext>();
        dContext.Setup(x => x.Issues).Throws(new Exception());
        var mapper = AutoMapperHelpers.CreateAutoMapper();
        var command = new CreateIssueCommand()
        {
            Id = "Issue_11",
            Summary = "Summary",
            Description = "Description",
            UserCredentials = new UserCredentials { Name = "User_1" },
        };
        var handler = new CreateIssueCommandHandler(dContext.Object, mapper);

        // ACT
        Func<Task> handle = async () => await handler.Handle(command, new CancellationToken());

        // ASSERT
        await handle.Should().ThrowAsync<CreateIssueCommandException>()
            .Where(e => e.Message.StartsWith("Creating issue"));
    }
}
