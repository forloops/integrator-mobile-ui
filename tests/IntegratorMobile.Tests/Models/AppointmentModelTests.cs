using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Models;

public class AppointmentModelTests
{
    #region StatusDisplay Tests

    [Theory]
    [InlineData(AppointmentStatus.Scheduled, "SCHEDULED")]
    [InlineData(AppointmentStatus.EnRoute, "EN ROUTE")]
    [InlineData(AppointmentStatus.OnSite, "ON SITE")]
    [InlineData(AppointmentStatus.InProgress, "IN PROGRESS")]
    [InlineData(AppointmentStatus.Completed, "COMPLETED")]
    [InlineData(AppointmentStatus.Cancelled, "CANCELLED")]
    [InlineData(AppointmentStatus.Rescheduled, "RESCHEDULED")]
    public void StatusDisplay_ReturnsCorrectString(AppointmentStatus status, string expected)
    {
        // Arrange
        var appointment = new Appointment { Status = status };

        // Act & Assert
        appointment.StatusDisplay.Should().Be(expected);
    }

    #endregion

    #region Location Tests

    [Fact]
    public void Location_FullAddress_FormatsCorrectly()
    {
        // Arrange
        var location = new Location
        {
            Address = "123 Main St",
            City = "Los Angeles",
            State = "CA",
            Zip = "90001"
        };

        // Act & Assert
        location.FullAddress.Should().Be("123 Main St, Los Angeles, CA 90001");
    }

    [Fact]
    public void Location_CityStateZip_FormatsCorrectly()
    {
        // Arrange
        var location = new Location
        {
            Address = "123 Main St",
            City = "Los Angeles",
            State = "CA",
            Zip = "90001"
        };

        // Act & Assert
        location.CityStateZip.Should().Be("Los Angeles, CA 90001");
    }

    #endregion

    #region Default Values Tests

    [Fact]
    public void Appointment_HasDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        appointment.Id.Should().BeEmpty();
        appointment.CustomerName.Should().BeEmpty();
        appointment.Location.Should().NotBeNull();
        appointment.PunchListProgress.Should().NotBeNull();
        appointment.AssignedUsers.Should().NotBeNull().And.BeEmpty();
        appointment.Buildings.Should().NotBeNull().And.BeEmpty();
        appointment.WorkItems.Should().NotBeNull().And.BeEmpty();
        appointment.ArrivalPhotos.Should().NotBeNull().And.BeEmpty();
        appointment.Status.Should().Be(AppointmentStatus.Scheduled); // Default enum value
    }

    [Fact]
    public void PunchListProgress_HasCorrectDefaults()
    {
        // Arrange & Act
        var progress = new PunchListProgress();

        // Assert
        progress.DriveToAppointment.Should().Be(PunchListStepStatus.Available);
        progress.AppointmentArrival.Should().Be(PunchListStepStatus.Locked);
        progress.SurveyBuildingsSystems.Should().Be(PunchListStepStatus.Locked);
        progress.CompleteAppointment.Should().Be(PunchListStepStatus.Locked);
    }

    #endregion

    #region Contact Tests

    [Fact]
    public void Contact_HasDefaultValues()
    {
        // Arrange & Act
        var contact = new Contact();

        // Assert
        contact.Name.Should().BeEmpty();
        contact.Phone.Should().BeEmpty();
        contact.Email.Should().BeEmpty();
    }

    #endregion

    #region Media Tests

    [Fact]
    public void Media_HasDefaultType()
    {
        // Arrange & Act
        var media = new Media();

        // Assert
        media.Type.Should().Be("photo");
    }

    #endregion
}
