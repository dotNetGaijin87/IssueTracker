using FluentAssertions;
using IssueTracker.Application.Utils;
using IssueTracker.Domain.Models;
using IssueTracker.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace IssueTracker.ApplicationTests;

public class DynamicHelpersTests
{
    [Fact]
    public void UpdateObjectWithFieldMask_FullFieldMask_Success()
    {
        // ARRANGE
        var old = new Project
        {
            Id = "old_id",
            Summary = "old_summary",
            Description = "old_description",
            CreationTime = DateTime.MinValue,
            CompletionTime = DateTime.MinValue,
            Progress = ProjectProgress.Open,
            CreatedBy = "old_user",
        };
        var @new = new Project
        {
            Id = "new_id",
            Summary = "new_summary",
            Description = "new_description",
            CreationTime = DateTime.MaxValue,
            CompletionTime = DateTime.MaxValue,
            Progress = ProjectProgress.Closed,
            CreatedBy = "new_user",
        };
        var fieldMask = new List<string>() { "Id", "Summary", "Description", "CreationTime", "CompletionTime", "Progress", "CreatedBy" };

        // ACT
        var updated = DynamicUtils.UpdateObjectWithFieldMask(@new, fieldMask, old);

        // ASSERT
        updated.Id.Should().Be("new_id");
        updated.Summary.Should().Be("new_summary");
        updated.Description.Should().Be("new_description");
        updated.CreationTime.Should().Be(DateTime.MaxValue);
        updated.CompletionTime.Should().Be(DateTime.MaxValue);
        updated.CreatedBy.Should().Be("new_user");
        updated.Progress.Should().Be(ProjectProgress.Closed);
    }

    [Fact]
    public void UpdateObjectWithFieldMask_PartialFieldMask_Success()
    {
        // ARRANGE
        var old = new Project
        {
            Id = "old_id",
            Summary = "old_summary",
            Description = "old_description",
            CreationTime = DateTime.MinValue,
            CompletionTime = DateTime.MinValue,
            Progress = ProjectProgress.Open,
            CreatedBy = "old_user",
        };
        var @new = new Project
        {
            Id = "new_id",
            Summary = "new_summary",
            Description = "new_description",
            CreationTime = DateTime.MaxValue,
            CompletionTime = DateTime.MaxValue,
            Progress = ProjectProgress.Closed,
            CreatedBy = "new_user",
        };
        var fieldMask = new List<string>() { "Id", "Summary", };

        // ACT
        var updated = DynamicUtils.UpdateObjectWithFieldMask(@new, fieldMask, old);

        // ASSERT
        updated.Id.Should().Be("new_id");
        updated.Summary.Should().Be("new_summary");
        updated.Description.Should().Be("old_description");
        updated.CreationTime.Should().Be(DateTime.MinValue);
        updated.CompletionTime.Should().Be(DateTime.MinValue);
        updated.CreatedBy.Should().Be("old_user");
        updated.Progress.Should().Be(ProjectProgress.Open);
    }

    [Fact]
    public void UpdateObjectWithFieldMask_EmptyFieldMask_Success()
    {
        // ARRANGE
        var old = new Project
        {
            Id = "old_id",
            Summary = "old_summary",
            Description = "old_description",
            CreationTime = DateTime.MinValue,
            CompletionTime = DateTime.MinValue,
            Progress = ProjectProgress.Open,
            CreatedBy = "old_user",
        };
        var @new = new Project
        {
            Id = "new_id",
            Summary = "new_summary",
            Description = "new_description",
            CreationTime = DateTime.MaxValue,
            CompletionTime = DateTime.MaxValue,
            Progress = ProjectProgress.Closed,
            CreatedBy = "new_user",
        };
        var fieldMask = new List<string>() { };

        // ACT
        var updated = DynamicUtils.UpdateObjectWithFieldMask(@new, fieldMask, old);

        // ASSERT
        updated.Id.Should().Be("old_id");
        updated.Summary.Should().Be("old_summary");
        updated.Description.Should().Be("old_description");
        updated.CreationTime.Should().Be(DateTime.MinValue);
        updated.CompletionTime.Should().Be(DateTime.MinValue);
        updated.CreatedBy.Should().Be("old_user");
        updated.Progress.Should().Be(ProjectProgress.Open);
    }
}
