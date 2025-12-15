using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Logic;

/// <summary>
/// Tests for status display logic used across the application.
/// </summary>
public class StatusDisplayTests
{
    #region Status to Color Mapping Tests

    [Theory]
    [InlineData("SCHEDULED", "#64748B")]
    [InlineData("EN ROUTE", "#0284C7")]
    [InlineData("ON SITE", "#16A34A")]
    [InlineData("IN PROGRESS", "#0284C7")]
    [InlineData("COMPLETED", "#16A34A")]
    [InlineData("NEED TO RETURN", "#D97706")]
    [InlineData("CANCELLED", "#DC2626")]
    public void StatusToColor_ReturnsCorrectHex(string status, string expectedHex)
    {
        // Act
        var result = GetStatusColor(status);
        
        // Assert
        result.Should().Be(expectedHex);
    }

    [Theory]
    [InlineData("scheduled", "#64748B")]
    [InlineData("en route", "#0284C7")]
    [InlineData("Completed", "#16A34A")]
    public void StatusToColor_IsCaseInsensitive(string status, string expectedHex)
    {
        // Act
        var result = GetStatusColor(status);
        
        // Assert
        result.Should().Be(expectedHex);
    }

    [Theory]
    [InlineData("UNKNOWN")]
    [InlineData("")]
    [InlineData("INVALID")]
    public void StatusToColor_UnknownStatus_ReturnsDefault(string status)
    {
        // Act
        var result = GetStatusColor(status);
        
        // Assert
        result.Should().Be("#64748B"); // Default gray
    }

    #endregion

    #region Badge Type Mapping Tests

    [Theory]
    [InlineData(AppointmentStatus.Scheduled, "Default")]
    [InlineData(AppointmentStatus.EnRoute, "EnRoute")]
    [InlineData(AppointmentStatus.OnSite, "OnSite")]
    [InlineData(AppointmentStatus.InProgress, "InProgress")]
    [InlineData(AppointmentStatus.Completed, "Completed")]
    [InlineData(AppointmentStatus.Cancelled, "Cancelled")]
    [InlineData(AppointmentStatus.Rescheduled, "Warning")]
    public void AppointmentStatusToBadgeType_ReturnsCorrectType(AppointmentStatus status, string expectedBadgeType)
    {
        // Act
        var result = GetBadgeTypeForStatus(status);
        
        // Assert
        result.Should().Be(expectedBadgeType);
    }

    [Theory]
    [InlineData(WorkItemStatus.Created, "Default")]
    [InlineData(WorkItemStatus.Ready, "Ready")]
    [InlineData(WorkItemStatus.InProgress, "InProgress")]
    [InlineData(WorkItemStatus.Completed, "Completed")]
    [InlineData(WorkItemStatus.NeedToReturn, "NeedToReturn")]
    public void WorkItemStatusToBadgeType_ReturnsCorrectType(WorkItemStatus status, string expectedBadgeType)
    {
        // Act
        var result = GetBadgeTypeForStatus(status);
        
        // Assert
        result.Should().Be(expectedBadgeType);
    }

    #endregion

    #region Punch List Step Status Tests

    [Theory]
    [InlineData(PunchListStepStatus.Locked, false)]
    [InlineData(PunchListStepStatus.Available, true)]
    [InlineData(PunchListStepStatus.InProgress, true)]
    [InlineData(PunchListStepStatus.Completed, false)]
    public void PunchListStep_IsActionable_ReturnsCorrectValue(PunchListStepStatus status, bool expected)
    {
        // Act
        var result = IsStepActionable(status);
        
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void PunchListProgress_NextAvailableStep_ReturnsCorrect()
    {
        // Arrange - Drive completed, arrival in progress
        var progress = new PunchListProgress
        {
            DriveToAppointment = PunchListStepStatus.Completed,
            AppointmentArrival = PunchListStepStatus.InProgress,
            SurveyBuildingsSystems = PunchListStepStatus.Locked,
            CompleteAppointment = PunchListStepStatus.Locked
        };
        
        // Act
        var nextStep = GetNextActionableStep(progress);
        
        // Assert
        nextStep.Should().Be("AppointmentArrival");
    }

    [Fact]
    public void PunchListProgress_AllCompleted_ReturnsNull()
    {
        // Arrange
        var progress = new PunchListProgress
        {
            DriveToAppointment = PunchListStepStatus.Completed,
            AppointmentArrival = PunchListStepStatus.Completed,
            SurveyBuildingsSystems = PunchListStepStatus.Completed,
            CompleteAppointment = PunchListStepStatus.Completed
        };
        
        // Act
        var nextStep = GetNextActionableStep(progress);
        
        // Assert
        nextStep.Should().BeNull();
    }

    #endregion

    #region Helper Methods (Logic extracted for testing)

    private static string GetStatusColor(string status)
    {
        return status.ToUpper() switch
        {
            "SCHEDULED" => "#64748B",
            "EN ROUTE" => "#0284C7",
            "ON SITE" => "#16A34A",
            "IN PROGRESS" => "#0284C7",
            "COMPLETED" => "#16A34A",
            "NEED TO RETURN" => "#D97706",
            "CANCELLED" => "#DC2626",
            _ => "#64748B"
        };
    }

    private static string GetBadgeTypeForStatus(AppointmentStatus status)
    {
        return status switch
        {
            AppointmentStatus.Scheduled => "Default",
            AppointmentStatus.EnRoute => "EnRoute",
            AppointmentStatus.OnSite => "OnSite",
            AppointmentStatus.InProgress => "InProgress",
            AppointmentStatus.Completed => "Completed",
            AppointmentStatus.Cancelled => "Cancelled",
            AppointmentStatus.Rescheduled => "Warning",
            _ => "Default"
        };
    }

    private static string GetBadgeTypeForStatus(WorkItemStatus status)
    {
        return status switch
        {
            WorkItemStatus.Created => "Default",
            WorkItemStatus.Ready => "Ready",
            WorkItemStatus.InProgress => "InProgress",
            WorkItemStatus.Completed => "Completed",
            WorkItemStatus.NeedToReturn => "NeedToReturn",
            _ => "Default"
        };
    }

    private static bool IsStepActionable(PunchListStepStatus status)
    {
        return status == PunchListStepStatus.Available || status == PunchListStepStatus.InProgress;
    }

    private static string? GetNextActionableStep(PunchListProgress progress)
    {
        if (IsStepActionable(progress.DriveToAppointment))
            return "DriveToAppointment";
        if (IsStepActionable(progress.AppointmentArrival))
            return "AppointmentArrival";
        if (IsStepActionable(progress.SurveyBuildingsSystems))
            return "SurveyBuildingsSystems";
        if (IsStepActionable(progress.CompleteAppointment))
            return "CompleteAppointment";
        return null;
    }

    #endregion
}
