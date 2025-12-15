using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using Xunit;

namespace IntegratorMobile.Tests.Integration;

/// <summary>
/// Integration tests for the complete appointment workflow (Punch List).
/// Tests the full flow from scheduled to completed.
/// </summary>
public class AppointmentWorkflowTests
{
    private readonly MockAppointmentService _appointmentService;
    private readonly MockAuthService _authService;

    public AppointmentWorkflowTests()
    {
        _appointmentService = new MockAppointmentService();
        _authService = new MockAuthService();
    }

    #region Full Workflow Tests

    [Fact]
    public async Task CompleteAppointmentWorkflow_ScheduledToCompleted()
    {
        // Arrange - Login and get an appointment
        await _authService.LoginWithCredentials("jsmith", "password");
        var appointments = await _appointmentService.GetTodayAppointments();
        var appointment = appointments.First();
        
        appointment.Status.Should().Be(AppointmentStatus.Scheduled);
        appointment.PunchListProgress.DriveToAppointment.Should().Be(PunchListStepStatus.Available);

        // Step 1: Start driving (En Route)
        await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.EnRoute);
        
        var updatedAppt = await _appointmentService.GetAppointmentById(appointment.Id);
        updatedAppt!.Status.Should().Be(AppointmentStatus.EnRoute);
        updatedAppt.EnRouteStartTime.Should().NotBeNull();
        updatedAppt.PunchListProgress.DriveToAppointment.Should().Be(PunchListStepStatus.InProgress);

        // Step 2: Arrive on site
        await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.OnSite);
        
        updatedAppt = await _appointmentService.GetAppointmentById(appointment.Id);
        updatedAppt!.Status.Should().Be(AppointmentStatus.OnSite);
        updatedAppt.ArrivalTime.Should().NotBeNull();
        updatedAppt.PunchListProgress.DriveToAppointment.Should().Be(PunchListStepStatus.Completed);
        updatedAppt.PunchListProgress.AppointmentArrival.Should().Be(PunchListStepStatus.InProgress);

        // Step 3: Start work (In Progress)
        await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.InProgress);
        
        updatedAppt = await _appointmentService.GetAppointmentById(appointment.Id);
        updatedAppt!.Status.Should().Be(AppointmentStatus.InProgress);
        updatedAppt.PunchListProgress.AppointmentArrival.Should().Be(PunchListStepStatus.Completed);
        updatedAppt.PunchListProgress.SurveyBuildingsSystems.Should().Be(PunchListStepStatus.InProgress);

        // Step 4: Complete appointment
        await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.Completed);
        
        updatedAppt = await _appointmentService.GetAppointmentById(appointment.Id);
        updatedAppt!.Status.Should().Be(AppointmentStatus.Completed);
        updatedAppt.CompletedTime.Should().NotBeNull();
        updatedAppt.PunchListProgress.SurveyBuildingsSystems.Should().Be(PunchListStepStatus.Completed);
        updatedAppt.PunchListProgress.CompleteAppointment.Should().Be(PunchListStepStatus.Completed);
    }

    #endregion

    #region Work Item Workflow Tests

    [Fact]
    public async Task WorkItemWorkflow_ReadyToCompleted()
    {
        // Arrange
        var workItems = await _appointmentService.GetWorkItemsForAppointment("apt-001");
        var workItem = workItems.First(w => w.Status == WorkItemStatus.Ready);
        var workItemId = workItem.Id;

        workItem.Status.Should().Be(WorkItemStatus.Ready);
        workItem.CompletedAt.Should().BeNull();

        // Act - Start work
        await _appointmentService.UpdateWorkItemStatus(workItemId, WorkItemStatus.InProgress);

        // Assert
        var updated = await _appointmentService.GetWorkItemById(workItemId);
        updated!.Status.Should().Be(WorkItemStatus.InProgress);
        updated.CompletedAt.Should().BeNull();

        // Act - Complete work
        await _appointmentService.UpdateWorkItemStatus(workItemId, WorkItemStatus.Completed);

        // Assert
        updated = await _appointmentService.GetWorkItemById(workItemId);
        updated!.Status.Should().Be(WorkItemStatus.Completed);
        updated.CompletedAt.Should().NotBeNull();
        updated.CompletedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task WorkItemWorkflow_CanMarkNeedToReturn()
    {
        // Arrange
        var workItems = await _appointmentService.GetWorkItemsForAppointment("apt-001");
        var workItem = workItems.First(w => w.Status == WorkItemStatus.Ready);
        var workItemId = workItem.Id;

        // Act
        await _appointmentService.UpdateWorkItemStatus(workItemId, WorkItemStatus.NeedToReturn);

        // Assert
        var updated = await _appointmentService.GetWorkItemById(workItemId);
        updated!.Status.Should().Be(WorkItemStatus.NeedToReturn);
        updated.CompletedAt.Should().BeNull(); // Not completed
    }

    #endregion

    #region Multi-User Scenario Tests

    [Fact]
    public async Task MultipleUsers_CanAccessSameAppointment()
    {
        // Arrange - Two service instances (simulating different users)
        var service1 = new MockAppointmentService();
        var service2 = new MockAppointmentService();

        // Act
        var appt1 = await service1.GetAppointmentById("apt-001");
        var appt2 = await service2.GetAppointmentById("apt-001");

        // Assert - Both get the same data
        appt1.Should().NotBeNull();
        appt2.Should().NotBeNull();
        appt1!.CustomerName.Should().Be(appt2!.CustomerName);
    }

    #endregion

    #region Appointment List Category Tests

    [Fact]
    public async Task AppointmentCategories_AreExclusive()
    {
        // Arrange
        var todayAppts = await _appointmentService.GetTodayAppointments();
        var futureAppts = await _appointmentService.GetFutureAppointments();
        var pastAppts = await _appointmentService.GetPastAppointments();

        var todayIds = todayAppts.Select(a => a.Id).ToHashSet();
        var futureIds = futureAppts.Select(a => a.Id).ToHashSet();
        var pastIds = pastAppts.Select(a => a.Id).ToHashSet();

        // Assert - No overlaps between today and future
        todayIds.Intersect(futureIds).Should().BeEmpty();
        
        // Note: Past might include completed today appointments, so we don't check that overlap
    }

    #endregion
}
