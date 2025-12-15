using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Models;

public class WorkItemModelTests
{
    #region StatusDisplay Tests

    [Theory]
    [InlineData(WorkItemStatus.Created, "CREATED")]
    [InlineData(WorkItemStatus.Ready, "READY")]
    [InlineData(WorkItemStatus.InProgress, "IN PROGRESS")]
    [InlineData(WorkItemStatus.Completed, "COMPLETED")]
    [InlineData(WorkItemStatus.NeedToReturn, "NEED TO RETURN")]
    public void StatusDisplay_ReturnsCorrectString(WorkItemStatus status, string expected)
    {
        // Arrange
        var workItem = new WorkItem { Status = status };

        // Act & Assert
        workItem.StatusDisplay.Should().Be(expected);
    }

    #endregion

    #region TypeDisplay Tests

    [Theory]
    [InlineData(WorkItemType.Inspection, "Inspection")]
    [InlineData(WorkItemType.Survey, "Survey")]
    [InlineData(WorkItemType.Estimate, "Estimate")]
    [InlineData(WorkItemType.AdhocRepair, "Adhoc Repair")]
    [InlineData(WorkItemType.LineItemRepair, "Line Item Repair")]
    public void TypeDisplay_ReturnsCorrectString(WorkItemType type, string expected)
    {
        // Arrange
        var workItem = new WorkItem { Type = type };

        // Act & Assert
        workItem.TypeDisplay.Should().Be(expected);
    }

    #endregion

    #region Default Values Tests

    [Fact]
    public void WorkItem_HasDefaultValues()
    {
        // Arrange & Act
        var workItem = new WorkItem();

        // Assert
        workItem.Id.Should().BeEmpty();
        workItem.AppointmentId.Should().BeEmpty();
        workItem.BuildingId.Should().BeEmpty();
        workItem.SystemId.Should().BeEmpty();
        workItem.Title.Should().BeEmpty();
        workItem.Description.Should().BeEmpty();
        workItem.BuildingName.Should().BeEmpty();
        workItem.SystemName.Should().BeEmpty();
        workItem.Milestones.Should().NotBeNull().And.BeEmpty();
        workItem.NeedToReturnReason.Should().BeNull();
        workItem.CreatedBy.Should().BeEmpty();
        workItem.CompletedAt.Should().BeNull();
        workItem.Status.Should().Be(WorkItemStatus.Created); // Default enum value
        workItem.Type.Should().Be(WorkItemType.Inspection); // Default enum value
    }

    #endregion

    #region Milestone Tests

    [Fact]
    public void Milestone_HasDefaultValues()
    {
        // Arrange & Act
        var milestone = new Milestone();

        // Assert
        milestone.Id.Should().BeEmpty();
        milestone.WorkItemId.Should().BeEmpty();
        milestone.Name.Should().BeEmpty();
        milestone.Description.Should().BeEmpty();
        milestone.Media.Should().NotBeNull().And.BeEmpty();
        milestone.CreatedBy.Should().BeEmpty();
        milestone.Type.Should().Be(MilestoneType.Before); // Default enum value
    }

    [Theory]
    [InlineData(MilestoneType.Before)]
    [InlineData(MilestoneType.InProgress)]
    [InlineData(MilestoneType.Completed)]
    [InlineData(MilestoneType.Custom)]
    public void Milestone_SupportsAllTypes(MilestoneType type)
    {
        // Arrange & Act
        var milestone = new Milestone { Type = type };

        // Assert
        milestone.Type.Should().Be(type);
    }

    #endregion

    #region WorkItem Status Transitions Tests

    [Fact]
    public void WorkItem_CanTransitionFromReadyToInProgress()
    {
        // Arrange
        var workItem = new WorkItem { Status = WorkItemStatus.Ready };

        // Act
        workItem.Status = WorkItemStatus.InProgress;

        // Assert
        workItem.Status.Should().Be(WorkItemStatus.InProgress);
        workItem.StatusDisplay.Should().Be("IN PROGRESS");
    }

    [Fact]
    public void WorkItem_CanTransitionFromInProgressToCompleted()
    {
        // Arrange
        var workItem = new WorkItem { Status = WorkItemStatus.InProgress };

        // Act
        workItem.Status = WorkItemStatus.Completed;
        workItem.CompletedAt = DateTime.Now;

        // Assert
        workItem.Status.Should().Be(WorkItemStatus.Completed);
        workItem.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public void WorkItem_CanTransitionToNeedToReturn()
    {
        // Arrange
        var workItem = new WorkItem { Status = WorkItemStatus.InProgress };

        // Act
        workItem.Status = WorkItemStatus.NeedToReturn;
        workItem.NeedToReturnReason = "Part not available";

        // Assert
        workItem.Status.Should().Be(WorkItemStatus.NeedToReturn);
        workItem.NeedToReturnReason.Should().Be("Part not available");
    }

    #endregion
}
