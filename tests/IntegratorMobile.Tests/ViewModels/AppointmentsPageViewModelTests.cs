using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using IntegratorMobile.Tests.Mocks;
using Moq;
using Xunit;

namespace IntegratorMobile.Tests.ViewModels;

public class AppointmentsPageViewModelTests
{
    private readonly Mock<IAppointmentService> _mockAppointmentService;
    private readonly MockNavigationService _mockNavigation;

    public AppointmentsPageViewModelTests()
    {
        _mockAppointmentService = new Mock<IAppointmentService>();
        _mockNavigation = new MockNavigationService();
    }

    #region Load Appointments Tests

    [Fact]
    public async Task LoadAppointments_PopulatesTodayAppointments()
    {
        // Arrange
        var viewModel = CreateViewModel();
        var appointments = new List<Appointment>
        {
            new() { Id = "1", CustomerName = "Customer A" },
            new() { Id = "2", CustomerName = "Customer B" }
        };
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(appointments);
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        // Act
        await viewModel.LoadAppointmentsAsync();

        // Assert
        viewModel.TodayAppointments.Should().HaveCount(2);
        viewModel.HasTodayAppointments.Should().BeTrue();
    }

    [Fact]
    public async Task LoadAppointments_PopulatesUnresolvedAppointments()
    {
        // Arrange
        var viewModel = CreateViewModel();
        var pastAppointments = new List<Appointment>
        {
            new() { Id = "1", Status = AppointmentStatus.Completed },
            new() { Id = "2", Status = AppointmentStatus.InProgress }  // Unresolved
        };
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(pastAppointments);
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        // Act
        await viewModel.LoadAppointmentsAsync();

        // Assert
        viewModel.UnresolvedAppointments.Should().HaveCount(1);
        viewModel.HasUnresolvedAppointments.Should().BeTrue();
    }

    [Fact]
    public async Task LoadAppointments_EmptyLists_HasFlagsAreFalse()
    {
        // Arrange
        var viewModel = CreateViewModel();
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        // Act
        await viewModel.LoadAppointmentsAsync();

        // Assert
        viewModel.HasTodayAppointments.Should().BeFalse();
        viewModel.HasUnresolvedAppointments.Should().BeFalse();
        viewModel.HasFutureAppointments.Should().BeFalse();
    }

    #endregion

    #region Search/Filter Tests

    [Fact]
    public void ToggleSearch_TogglesVisibility()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.IsSearchVisible.Should().BeFalse();

        // Act
        viewModel.ToggleSearch();

        // Assert
        viewModel.IsSearchVisible.Should().BeTrue();
    }

    [Fact]
    public void ToggleSearch_WhenHiding_ClearsQuery()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.IsSearchVisible = true;
        viewModel.SearchQuery = "test";

        // Act
        viewModel.ToggleSearch();

        // Assert
        viewModel.IsSearchVisible.Should().BeFalse();
        viewModel.SearchQuery.Should().BeEmpty();
    }

    [Fact]
    public void ClearSearch_ClearsQueryAndHides()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.IsSearchVisible = true;
        viewModel.SearchQuery = "test";

        // Act
        viewModel.ClearSearch();

        // Assert
        viewModel.IsSearchVisible.Should().BeFalse();
        viewModel.SearchQuery.Should().BeEmpty();
    }

    [Fact]
    public async Task FilterAppointments_FiltersAllLists()
    {
        // Arrange
        var viewModel = CreateViewModel();
        var todayAppts = new List<Appointment>
        {
            new() { Id = "1", CustomerName = "ABC Company", SiteName = "Site", JobNumber = "J1", Location = new Location(), ServiceJobType = "Service" },
            new() { Id = "2", CustomerName = "XYZ Corp", SiteName = "Site", JobNumber = "J2", Location = new Location(), ServiceJobType = "Service" }
        };
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(todayAppts);
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        await viewModel.LoadAppointmentsAsync();

        // Act
        viewModel.FilterAppointments("ABC");

        // Assert
        viewModel.TodayAppointments.Should().HaveCount(1);
        viewModel.TodayAppointments[0].CustomerName.Should().Be("ABC Company");
    }

    [Fact]
    public async Task FilterAppointments_EmptyQuery_RestoresAllAppointments()
    {
        // Arrange
        var viewModel = CreateViewModel();
        var todayAppts = new List<Appointment>
        {
            new() { Id = "1", CustomerName = "ABC", SiteName = "S", JobNumber = "J", Location = new Location(), ServiceJobType = "T" },
            new() { Id = "2", CustomerName = "XYZ", SiteName = "S", JobNumber = "J", Location = new Location(), ServiceJobType = "T" }
        };
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(todayAppts);
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        await viewModel.LoadAppointmentsAsync();
        viewModel.FilterAppointments("ABC"); // Filter first

        // Act
        viewModel.FilterAppointments(""); // Clear filter

        // Assert
        viewModel.TodayAppointments.Should().HaveCount(2);
    }

    #endregion

    #region Navigation Tests

    [Fact]
    public async Task SelectAppointment_NavigatesToDetail()
    {
        // Arrange
        var viewModel = CreateViewModel();
        var appointment = new Appointment { Id = "apt-001" };

        // Act
        await viewModel.SelectAppointmentAsync(appointment);

        // Assert
        _mockNavigation.LastNavigationWas("appointments/detail?id=apt-001").Should().BeTrue();
    }

    [Fact]
    public async Task SelectAppointment_WithNull_DoesNotNavigate()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.SelectAppointmentAsync(null!);

        // Assert
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task GoBack_NavigatesBack()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoBackAsync();

        // Assert
        _mockNavigation.GoBackCallCount.Should().Be(1);
    }

    #endregion

    #region Done For Day Tests

    [Fact]
    public async Task DoneForDay_WhenConfirmed_ReschedulesAppointments()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockNavigation.AlertResponse = true; // User confirms
        
        var todayAppts = new List<Appointment>
        {
            new() { Id = "1", Status = AppointmentStatus.Scheduled },
            new() { Id = "2", Status = AppointmentStatus.Completed }
        };
        
        _mockAppointmentService.Setup(x => x.GetTodayAppointments()).ReturnsAsync(todayAppts);
        _mockAppointmentService.Setup(x => x.GetPastAppointments()).ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetFutureAppointments()).ReturnsAsync(new List<Appointment>());

        await viewModel.LoadAppointmentsAsync();

        // Act
        await viewModel.DoneForDayAsync();

        // Assert
        _mockNavigation.AlertHistory.Should().HaveCount(1);
        _mockAppointmentService.Verify(
            x => x.UpdateAppointmentStatus("1", AppointmentStatus.Rescheduled), 
            Times.Once);
        // Completed appointment should not be rescheduled
        _mockAppointmentService.Verify(
            x => x.UpdateAppointmentStatus("2", AppointmentStatus.Rescheduled), 
            Times.Never);
    }

    [Fact]
    public async Task DoneForDay_WhenCancelled_DoesNotReschedule()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockNavigation.AlertResponse = false; // User cancels

        // Act
        await viewModel.DoneForDayAsync();

        // Assert
        _mockAppointmentService.Verify(
            x => x.UpdateAppointmentStatus(It.IsAny<string>(), It.IsAny<AppointmentStatus>()), 
            Times.Never);
    }

    #endregion

    #region Helper Methods

    private TestableAppointmentsPageViewModel CreateViewModel()
    {
        return new TestableAppointmentsPageViewModel(_mockAppointmentService.Object, _mockNavigation);
    }

    #endregion
}

public class TestableAppointmentsPageViewModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly INavigationService _navigationService;
    
    private List<Appointment> _allTodayAppointments = new();
    private List<Appointment> _allUnresolvedAppointments = new();
    private List<Appointment> _allFutureAppointments = new();

    public List<Appointment> TodayAppointments { get; set; } = new();
    public List<Appointment> UnresolvedAppointments { get; set; } = new();
    public List<Appointment> FutureAppointments { get; set; } = new();
    public bool HasTodayAppointments { get; set; }
    public bool HasUnresolvedAppointments { get; set; }
    public bool HasFutureAppointments { get; set; }
    public bool IsSearchVisible { get; set; }
    public string SearchQuery { get; set; } = string.Empty;
    public bool IsBusy { get; set; }

    public TestableAppointmentsPageViewModel(IAppointmentService appointmentService, INavigationService navigationService)
    {
        _appointmentService = appointmentService;
        _navigationService = navigationService;
    }

    public async Task LoadAppointmentsAsync()
    {
        IsBusy = true;
        try
        {
            var today = await _appointmentService.GetTodayAppointments();
            _allTodayAppointments = today;
            TodayAppointments = new List<Appointment>(today);
            HasTodayAppointments = today.Any();

            var past = await _appointmentService.GetPastAppointments();
            var unresolved = past.Where(a => a.Status != AppointmentStatus.Completed).ToList();
            _allUnresolvedAppointments = unresolved;
            UnresolvedAppointments = new List<Appointment>(unresolved);
            HasUnresolvedAppointments = unresolved.Any();

            var future = await _appointmentService.GetFutureAppointments();
            _allFutureAppointments = future;
            FutureAppointments = new List<Appointment>(future);
            HasFutureAppointments = future.Any();
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void ToggleSearch()
    {
        IsSearchVisible = !IsSearchVisible;
        if (!IsSearchVisible)
        {
            SearchQuery = string.Empty;
        }
    }

    public void ClearSearch()
    {
        SearchQuery = string.Empty;
        IsSearchVisible = false;
    }

    public void FilterAppointments(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            TodayAppointments = new List<Appointment>(_allTodayAppointments);
            UnresolvedAppointments = new List<Appointment>(_allUnresolvedAppointments);
            FutureAppointments = new List<Appointment>(_allFutureAppointments);
        }
        else
        {
            var lowerQuery = query.ToLowerInvariant();
            TodayAppointments = _allTodayAppointments.Where(a => MatchesSearch(a, lowerQuery)).ToList();
            UnresolvedAppointments = _allUnresolvedAppointments.Where(a => MatchesSearch(a, lowerQuery)).ToList();
            FutureAppointments = _allFutureAppointments.Where(a => MatchesSearch(a, lowerQuery)).ToList();
        }
        
        HasTodayAppointments = TodayAppointments.Any();
        HasUnresolvedAppointments = UnresolvedAppointments.Any();
        HasFutureAppointments = FutureAppointments.Any();
    }

    private static bool MatchesSearch(Appointment appointment, string query)
    {
        return appointment.CustomerName.ToLowerInvariant().Contains(query) ||
               appointment.SiteName.ToLowerInvariant().Contains(query) ||
               appointment.JobNumber.ToLowerInvariant().Contains(query) ||
               appointment.Location.Address.ToLowerInvariant().Contains(query) ||
               appointment.Location.City.ToLowerInvariant().Contains(query) ||
               appointment.ServiceJobType.ToLowerInvariant().Contains(query);
    }

    public async Task SelectAppointmentAsync(Appointment? appointment)
    {
        if (appointment == null) return;
        await _navigationService.GoToAsync($"appointments/detail?id={appointment.Id}");
    }

    public async Task DoneForDayAsync()
    {
        var confirm = await _navigationService.DisplayAlertAsync(
            "Complete My Day",
            "This will mark all remaining appointments as 'Need to Return'. Are you sure?",
            "Yes, I'm Done",
            "Cancel");

        if (confirm)
        {
            foreach (var appointment in TodayAppointments.Where(a => a.Status != AppointmentStatus.Completed))
            {
                await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.Rescheduled);
            }
            await LoadAppointmentsAsync();
            await _navigationService.DisplayAlertAsync("Done", "Your day has been completed.", "OK");
        }
    }

    public async Task GoBackAsync()
    {
        await _navigationService.GoBackAsync();
    }
}
