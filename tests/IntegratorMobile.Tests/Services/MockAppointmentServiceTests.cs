using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using Xunit;

namespace IntegratorMobile.Tests.Services;

public class MockAppointmentServiceTests
{
    private readonly MockAppointmentService _sut;

    public MockAppointmentServiceTests()
    {
        _sut = new MockAppointmentService();
    }

    #region GetTodayAppointments Tests

    [Fact]
    public async Task GetTodayAppointments_ReturnsOnlyTodaysAppointments()
    {
        // Act
        var result = await _sut.GetTodayAppointments();

        // Assert
        result.Should().NotBeNull();
        result.Should().AllSatisfy(a => a.ScheduledStart.Date.Should().Be(DateTime.Today));
    }

    [Fact]
    public async Task GetTodayAppointments_OrderedByScheduledStart()
    {
        // Act
        var result = await _sut.GetTodayAppointments();

        // Assert
        result.Should().BeInAscendingOrder(a => a.ScheduledStart);
    }

    #endregion

    #region GetFutureAppointments Tests

    [Fact]
    public async Task GetFutureAppointments_ReturnsOnlyFutureAppointments()
    {
        // Act
        var result = await _sut.GetFutureAppointments();

        // Assert
        result.Should().NotBeNull();
        result.Should().AllSatisfy(a => a.ScheduledStart.Date.Should().BeAfter(DateTime.Today));
    }

    [Fact]
    public async Task GetFutureAppointments_OrderedByScheduledStart()
    {
        // Act
        var result = await _sut.GetFutureAppointments();

        // Assert
        result.Should().BeInAscendingOrder(a => a.ScheduledStart);
    }

    #endregion

    #region GetPastAppointments Tests

    [Fact]
    public async Task GetPastAppointments_ReturnsAppointmentsBeforeToday()
    {
        // Act
        var result = await _sut.GetPastAppointments();

        // Assert
        result.Should().NotBeNull();
        // Past appointments include completed from today or any from before today
        result.Should().AllSatisfy(a => 
        {
            var isPastDate = a.ScheduledStart.Date < DateTime.Today;
            var isCompletedToday = a.Status == AppointmentStatus.Completed && a.ScheduledStart.Date == DateTime.Today;
            (isPastDate || isCompletedToday).Should().BeTrue();
        });
    }

    [Fact]
    public async Task GetPastAppointments_OrderedByScheduledStartDescending()
    {
        // Act
        var result = await _sut.GetPastAppointments();

        // Assert
        result.Should().BeInDescendingOrder(a => a.ScheduledStart);
    }

    #endregion

    #region GetAppointmentById Tests

    [Fact]
    public async Task GetAppointmentById_WithValidId_ReturnsAppointment()
    {
        // Act
        var result = await _sut.GetAppointmentById("apt-001");

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("apt-001");
        result.CustomerName.Should().Be("Sunset Plaza HOA");
    }

    [Fact]
    public async Task GetAppointmentById_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _sut.GetAppointmentById("invalid-id");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAppointmentById_ReturnsAppointmentWithAllDetails()
    {
        // Act
        var result = await _sut.GetAppointmentById("apt-001");

        // Assert
        result.Should().NotBeNull();
        result!.Location.Should().NotBeNull();
        result.Location.Address.Should().NotBeNullOrEmpty();
        result.Buildings.Should().NotBeEmpty();
        result.AssignedUsers.Should().NotBeEmpty();
        result.Customer.Should().NotBeNull();
    }

    #endregion

    #region UpdateAppointmentStatus Tests

    [Fact]
    public async Task UpdateAppointmentStatus_ToEnRoute_UpdatesStatusAndTimestamp()
    {
        // Arrange
        var appointmentId = "apt-001";

        // Act
        await _sut.UpdateAppointmentStatus(appointmentId, AppointmentStatus.EnRoute);

        // Assert
        var appointment = await _sut.GetAppointmentById(appointmentId);
        appointment.Should().NotBeNull();
        appointment!.Status.Should().Be(AppointmentStatus.EnRoute);
        appointment.EnRouteStartTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        appointment.PunchListProgress.DriveToAppointment.Should().Be(PunchListStepStatus.InProgress);
    }

    [Fact]
    public async Task UpdateAppointmentStatus_ToOnSite_UpdatesPunchList()
    {
        // Arrange
        var appointmentId = "apt-001";
        await _sut.UpdateAppointmentStatus(appointmentId, AppointmentStatus.EnRoute);

        // Act
        await _sut.UpdateAppointmentStatus(appointmentId, AppointmentStatus.OnSite);

        // Assert
        var appointment = await _sut.GetAppointmentById(appointmentId);
        appointment.Should().NotBeNull();
        appointment!.Status.Should().Be(AppointmentStatus.OnSite);
        appointment.ArrivalTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        appointment.PunchListProgress.DriveToAppointment.Should().Be(PunchListStepStatus.Completed);
        appointment.PunchListProgress.AppointmentArrival.Should().Be(PunchListStepStatus.InProgress);
    }

    [Fact]
    public async Task UpdateAppointmentStatus_ToCompleted_SetsCompletedTime()
    {
        // Arrange
        var appointmentId = "apt-001";

        // Act
        await _sut.UpdateAppointmentStatus(appointmentId, AppointmentStatus.Completed);

        // Assert
        var appointment = await _sut.GetAppointmentById(appointmentId);
        appointment.Should().NotBeNull();
        appointment!.Status.Should().Be(AppointmentStatus.Completed);
        appointment.CompletedTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        appointment.PunchListProgress.CompleteAppointment.Should().Be(PunchListStepStatus.Completed);
    }

    [Fact]
    public async Task UpdateAppointmentStatus_WithInvalidId_DoesNotThrow()
    {
        // Act & Assert
        var act = () => _sut.UpdateAppointmentStatus("invalid-id", AppointmentStatus.EnRoute);
        await act.Should().NotThrowAsync();
    }

    #endregion

    #region GetWorkItemsForAppointment Tests

    [Fact]
    public async Task GetWorkItemsForAppointment_WithValidId_ReturnsWorkItems()
    {
        // Act
        var result = await _sut.GetWorkItemsForAppointment("apt-001");

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().AllSatisfy(w => w.AppointmentId.Should().Be("apt-001"));
    }

    [Fact]
    public async Task GetWorkItemsForAppointment_WithInvalidId_ReturnsEmptyList()
    {
        // Act
        var result = await _sut.GetWorkItemsForAppointment("invalid-id");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region GetWorkItemById Tests

    [Fact]
    public async Task GetWorkItemById_WithValidId_ReturnsWorkItem()
    {
        // Act
        var result = await _sut.GetWorkItemById("wi-001");

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("wi-001");
        result.Title.Should().Be("Annual HVAC Inspection");
    }

    [Fact]
    public async Task GetWorkItemById_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _sut.GetWorkItemById("invalid-id");

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region UpdateWorkItemStatus Tests

    [Fact]
    public async Task UpdateWorkItemStatus_ToCompleted_SetsCompletedAt()
    {
        // Arrange
        var workItemId = "wi-001";

        // Act
        await _sut.UpdateWorkItemStatus(workItemId, WorkItemStatus.Completed);

        // Assert
        var workItem = await _sut.GetWorkItemById(workItemId);
        workItem.Should().NotBeNull();
        workItem!.Status.Should().Be(WorkItemStatus.Completed);
        workItem.CompletedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task UpdateWorkItemStatus_ToInProgress_DoesNotSetCompletedAt()
    {
        // Arrange
        var workItemId = "wi-001";

        // Act
        await _sut.UpdateWorkItemStatus(workItemId, WorkItemStatus.InProgress);

        // Assert
        var workItem = await _sut.GetWorkItemById(workItemId);
        workItem.Should().NotBeNull();
        workItem!.Status.Should().Be(WorkItemStatus.InProgress);
        workItem.CompletedAt.Should().BeNull();
    }

    [Fact]
    public async Task UpdateWorkItemStatus_WithInvalidId_DoesNotThrow()
    {
        // Act & Assert
        var act = () => _sut.UpdateWorkItemStatus("invalid-id", WorkItemStatus.Completed);
        await act.Should().NotThrowAsync();
    }

    #endregion

    #region Data Integrity Tests

    [Fact]
    public async Task MockData_AppointmentsHaveValidBuildings()
    {
        // Arrange
        var appointments = await _sut.GetTodayAppointments();
        appointments.AddRange(await _sut.GetFutureAppointments());
        appointments.AddRange(await _sut.GetPastAppointments());

        // Assert
        foreach (var appointment in appointments.Where(a => a.Buildings != null && a.Buildings.Count > 0))
        {
            appointment.Buildings.Should().AllSatisfy(b =>
            {
                b.Id.Should().NotBeNullOrEmpty();
                b.Name.Should().NotBeNullOrEmpty();
            });
        }
    }

    [Fact]
    public async Task MockData_AppointmentsHaveValidLocations()
    {
        // Arrange
        var appointments = await _sut.GetTodayAppointments();

        // Assert
        appointments.Should().AllSatisfy(a =>
        {
            a.Location.Should().NotBeNull();
            a.Location.Address.Should().NotBeNullOrEmpty();
            a.Location.City.Should().NotBeNullOrEmpty();
            a.Location.State.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task MockData_WorkItemsReferenceValidAppointments()
    {
        // Arrange - Get all appointment IDs
        var allAppointments = new List<Appointment>();
        allAppointments.AddRange(await _sut.GetTodayAppointments());
        allAppointments.AddRange(await _sut.GetFutureAppointments());
        allAppointments.AddRange(await _sut.GetPastAppointments());
        var validAppointmentIds = allAppointments.Select(a => a.Id).ToHashSet();

        // Act - Get work items for each appointment
        foreach (var appointment in allAppointments)
        {
            var workItems = await _sut.GetWorkItemsForAppointment(appointment.Id);
            
            // Assert - Only check if there are work items
            if (workItems.Count > 0)
            {
                workItems.Should().AllSatisfy(w =>
                {
                    validAppointmentIds.Should().Contain(w.AppointmentId);
                });
            }
        }
    }

    #endregion
}
