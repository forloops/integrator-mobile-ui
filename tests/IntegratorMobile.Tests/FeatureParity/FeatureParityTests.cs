using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using Xunit;

namespace IntegratorMobile.Tests.FeatureParity;

/// <summary>
/// Tests to verify feature parity between Vue and MAUI prototypes.
/// These tests verify that the MAUI mock services support all features
/// required by the Vue prototype.
/// </summary>
public class FeatureParityTests
{
    private readonly MockAuthService _authService;
    private readonly MockAppointmentService _appointmentService;

    public FeatureParityTests()
    {
        _authService = new MockAuthService();
        _appointmentService = new MockAppointmentService();
    }

    #region FR-AUTH: Authentication Feature Parity

    [Fact]
    public async Task FR_AUTH_001_CompanyIdentification_Supported()
    {
        // Company identification before login
        var company = await _authService.ValidateCompanyIdentifier("crowther");
        company.Should().NotBeNull("Company identification is required (FR-AUTH-001)");
    }

    [Fact]
    public async Task FR_AUTH_002_MultipleCompanies_Supported()
    {
        // Multiple companies for multi-tenant support
        var crowther = await _authService.ValidateCompanyIdentifier("crowther");
        var demo = await _authService.ValidateCompanyIdentifier("demo");
        var acme = await _authService.ValidateCompanyIdentifier("acme");

        crowther.Should().NotBeNull();
        demo.Should().NotBeNull();
        acme.Should().NotBeNull();
        
        crowther!.Name.Should().NotBe(demo!.Name);
    }

    [Fact]
    public async Task FR_AUTH_010_MicrosoftSSO_Supported()
    {
        // Microsoft SSO login
        var user = await _authService.LoginWithMicrosoft();
        user.Should().NotBeNull("Microsoft SSO login is required (FR-AUTH-010)");
    }

    [Fact]
    public async Task FR_AUTH_011_ManualLogin_Supported()
    {
        // Manual username/password login
        var user = await _authService.LoginWithCredentials("jsmith", "password");
        user.Should().NotBeNull("Manual login is required (FR-AUTH-011)");
    }

    #endregion

    #region FR-APPT: Appointment Feature Parity

    [Fact]
    public async Task FR_APPT_001_TodayAppointments_Supported()
    {
        // View today's appointments
        var appointments = await _appointmentService.GetTodayAppointments();
        appointments.Should().NotBeNull("Today appointments list is required (FR-APPT-001)");
    }

    [Fact]
    public async Task FR_APPT_002_FutureAppointments_Supported()
    {
        // View future appointments
        var appointments = await _appointmentService.GetFutureAppointments();
        appointments.Should().NotBeNull("Future appointments list is required");
    }

    [Fact]
    public async Task FR_APPT_003_AppointmentDetail_HasRequiredFields()
    {
        // Appointment detail with all required fields
        var appointment = await _appointmentService.GetAppointmentById("apt-001");
        
        appointment.Should().NotBeNull();
        appointment!.Id.Should().NotBeNullOrEmpty();
        appointment.CustomerName.Should().NotBeNullOrEmpty("Customer name is required");
        appointment.SiteName.Should().NotBeNullOrEmpty("Site name is required");
        appointment.ServiceJobType.Should().NotBeNullOrEmpty("Job type is required");
        appointment.Location.Should().NotBeNull("Location is required");
        appointment.Location.Address.Should().NotBeNullOrEmpty("Address is required");
        appointment.ScopeOfWork.Should().NotBeNullOrEmpty("Scope of work is required");
    }

    [Fact]
    public async Task FR_APPT_004_AppointmentStatuses_AllSupported()
    {
        // All appointment statuses are supported
        var statuses = Enum.GetValues<AppointmentStatus>();
        
        statuses.Should().Contain(AppointmentStatus.Scheduled);
        statuses.Should().Contain(AppointmentStatus.EnRoute);
        statuses.Should().Contain(AppointmentStatus.OnSite);
        statuses.Should().Contain(AppointmentStatus.InProgress);
        statuses.Should().Contain(AppointmentStatus.Completed);
        statuses.Should().Contain(AppointmentStatus.Cancelled);
        statuses.Should().Contain(AppointmentStatus.Rescheduled);
    }

    #endregion

    #region FR-PUNCH: Punch List Feature Parity

    [Fact]
    public async Task FR_PUNCH_PunchListSteps_AllSupported()
    {
        // Punch list has all 4 steps
        var appointment = await _appointmentService.GetAppointmentById("apt-001");
        var progress = appointment!.PunchListProgress;

        progress.Should().NotBeNull("Punch list progress is required");
        
        // Verify all 4 steps exist
        progress.DriveToAppointment.Should().NotBeNull();
        progress.AppointmentArrival.Should().NotBeNull();
        progress.SurveyBuildingsSystems.Should().NotBeNull();
        progress.CompleteAppointment.Should().NotBeNull();
    }

    [Fact]
    public void FR_PUNCH_StepStatuses_AllSupported()
    {
        // All punch list step statuses are supported
        var statuses = Enum.GetValues<PunchListStepStatus>();
        
        statuses.Should().Contain(PunchListStepStatus.Locked);
        statuses.Should().Contain(PunchListStepStatus.Available);
        statuses.Should().Contain(PunchListStepStatus.InProgress);
        statuses.Should().Contain(PunchListStepStatus.Completed);
    }

    #endregion

    #region FR-WORK: Work Item Feature Parity

    [Fact]
    public async Task FR_WORK_WorkItemTypes_AllSupported()
    {
        // All work item types from Vue prototype
        var types = Enum.GetValues<WorkItemType>();
        
        types.Should().Contain(WorkItemType.Inspection, "Inspection type is required");
        types.Should().Contain(WorkItemType.Survey, "Survey type is required");
        types.Should().Contain(WorkItemType.Estimate, "Estimate type is required");
        types.Should().Contain(WorkItemType.AdhocRepair, "Adhoc Repair type is required");
        types.Should().Contain(WorkItemType.LineItemRepair, "Line Item Repair type is required");
    }

    [Fact]
    public async Task FR_WORK_WorkItemStatuses_AllSupported()
    {
        // All work item statuses from Vue prototype
        var statuses = Enum.GetValues<WorkItemStatus>();
        
        statuses.Should().Contain(WorkItemStatus.Created);
        statuses.Should().Contain(WorkItemStatus.Ready);
        statuses.Should().Contain(WorkItemStatus.InProgress);
        statuses.Should().Contain(WorkItemStatus.Completed);
        statuses.Should().Contain(WorkItemStatus.NeedToReturn, "Need to Return status is required");
    }

    [Fact]
    public async Task FR_WORK_Milestones_TypesSupported()
    {
        // Milestone types for photo documentation
        var types = Enum.GetValues<MilestoneType>();
        
        types.Should().Contain(MilestoneType.Before, "Before milestone is required");
        types.Should().Contain(MilestoneType.InProgress, "In Progress milestone is required");
        types.Should().Contain(MilestoneType.Completed, "Completed milestone is required");
        types.Should().Contain(MilestoneType.Custom, "Custom milestone should be supported");
    }

    #endregion

    #region Data Model Feature Parity

    [Fact]
    public async Task DataModel_Building_HasSystems()
    {
        // Buildings contain systems hierarchy
        var appointment = await _appointmentService.GetAppointmentById("apt-001");
        
        appointment!.Buildings.Should().NotBeEmpty("Appointments should have buildings");
        appointment.Buildings.First().Systems.Should().NotBeEmpty("Buildings should have systems");
    }

    [Fact]
    public async Task DataModel_Customer_HasContacts()
    {
        // Customer info includes contacts
        var appointment = await _appointmentService.GetAppointmentById("apt-001");
        
        appointment!.Customer.Should().NotBeNull();
        appointment.Customer.Name.Should().NotBeNullOrEmpty();
        appointment.Customer.OnSiteContact.Should().NotBeNull();
    }

    [Fact]
    public async Task DataModel_Location_HasCoordinates()
    {
        // Location can have GPS coordinates
        var appointment = await _appointmentService.GetAppointmentById("apt-001");
        
        // At least one appointment should have coordinates for navigation
        var hasCoordinates = appointment!.Location.Latitude.HasValue && 
                            appointment.Location.Longitude.HasValue;
        hasCoordinates.Should().BeTrue("Location should support GPS coordinates for navigation");
    }

    #endregion
}
