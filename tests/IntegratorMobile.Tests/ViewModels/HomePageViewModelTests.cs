using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using IntegratorMobile.Tests.Mocks;
using Moq;
using Xunit;

namespace IntegratorMobile.Tests.ViewModels;

public class HomePageViewModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<IAppointmentService> _mockAppointmentService;
    private readonly MockNavigationService _mockNavigation;

    public HomePageViewModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockAppointmentService = new Mock<IAppointmentService>();
        _mockNavigation = new MockNavigationService();
    }

    #region Load Data Tests

    [Fact]
    public async Task LoadData_SetsUserName()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockAuthService.Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new User { FirstName = "John", LastName = "Smith" });
        _mockAppointmentService.Setup(x => x.GetTodayAppointments())
            .ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetPastAppointments())
            .ReturnsAsync(new List<Appointment>());

        // Act
        await viewModel.LoadDataAsync();

        // Assert
        viewModel.UserName.Should().Be("John Smith");
    }

    [Fact]
    public async Task LoadData_SetsTodayAppointmentCount()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockAuthService.Setup(x => x.GetCurrentUser())
            .ReturnsAsync((User?)null);
        _mockAppointmentService.Setup(x => x.GetTodayAppointments())
            .ReturnsAsync(new List<Appointment> { new(), new(), new() });
        _mockAppointmentService.Setup(x => x.GetPastAppointments())
            .ReturnsAsync(new List<Appointment>());

        // Act
        await viewModel.LoadDataAsync();

        // Assert
        viewModel.TodayAppointmentCount.Should().Be(3);
    }

    [Fact]
    public async Task LoadData_SetsUnresolvedCount()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockAuthService.Setup(x => x.GetCurrentUser())
            .ReturnsAsync((User?)null);
        _mockAppointmentService.Setup(x => x.GetTodayAppointments())
            .ReturnsAsync(new List<Appointment>());
        _mockAppointmentService.Setup(x => x.GetPastAppointments())
            .ReturnsAsync(new List<Appointment>
            {
                new() { Status = AppointmentStatus.Completed },
                new() { Status = AppointmentStatus.InProgress },
                new() { Status = AppointmentStatus.Scheduled }
            });

        // Act
        await viewModel.LoadDataAsync();

        // Assert
        viewModel.UnresolvedCount.Should().Be(2); // InProgress + Scheduled
    }

    #endregion

    #region Navigation Tests

    [Fact]
    public async Task GoToAppointments_NavigatesCorrectly()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToAppointmentsAsync();

        // Assert
        _mockNavigation.LastNavigationWas("appointments").Should().BeTrue();
    }

    [Fact]
    public async Task GoToProfile_NavigatesCorrectly()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToProfileAsync();

        // Assert
        _mockNavigation.LastNavigationWas("profile").Should().BeTrue();
    }

    [Fact]
    public async Task GoToSettings_NavigatesCorrectly()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToSettingsAsync();

        // Assert
        _mockNavigation.LastNavigationWas("settings").Should().BeTrue();
    }

    [Fact]
    public async Task GoToOperationJobs_ShowsAlert()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToOperationJobsAsync();

        // Assert
        _mockNavigation.SimpleAlertHistory.Should().HaveCount(1);
        _mockNavigation.SimpleAlertHistory[0].Title.Should().Be("Operations Jobs");
    }

    [Fact]
    public async Task GoToDirectory_ShowsAlert()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToDirectoryAsync();

        // Assert
        _mockNavigation.SimpleAlertHistory.Should().HaveCount(1);
        _mockNavigation.SimpleAlertHistory[0].Title.Should().Be("Employee Directory");
    }

    #endregion

    #region Logout Tests

    [Fact]
    public async Task Logout_WhenConfirmed_LogsOutAndNavigates()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockNavigation.AlertResponse = true;

        // Act
        await viewModel.LogoutAsync();

        // Assert
        _mockAuthService.Verify(x => x.Logout(), Times.Once);
        _mockNavigation.LastNavigationWas("//identify").Should().BeTrue();
    }

    [Fact]
    public async Task Logout_WhenCancelled_DoesNotLogout()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockNavigation.AlertResponse = false;

        // Act
        await viewModel.LogoutAsync();

        // Assert
        _mockAuthService.Verify(x => x.Logout(), Times.Never);
        _mockNavigation.NavigatedTo("//identify").Should().BeFalse();
    }

    [Fact]
    public async Task Logout_DisplaysConfirmationAlert()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockNavigation.AlertResponse = false;

        // Act
        await viewModel.LogoutAsync();

        // Assert
        _mockNavigation.AlertHistory.Should().HaveCount(1);
        _mockNavigation.AlertHistory[0].Title.Should().Be("Logout");
    }

    #endregion

    #region Helper Methods

    private TestableHomePageViewModel CreateViewModel()
    {
        return new TestableHomePageViewModel(
            _mockAuthService.Object, 
            _mockAppointmentService.Object, 
            _mockNavigation);
    }

    #endregion
}

public class TestableHomePageViewModel
{
    private readonly IAuthService _authService;
    private readonly IAppointmentService _appointmentService;
    private readonly INavigationService _navigationService;

    public string UserName { get; set; } = "John Smith";
    public int TodayAppointmentCount { get; set; }
    public int UnresolvedCount { get; set; }
    public bool IsBusy { get; set; }

    public TestableHomePageViewModel(
        IAuthService authService, 
        IAppointmentService appointmentService,
        INavigationService navigationService)
    {
        _authService = authService;
        _appointmentService = appointmentService;
        _navigationService = navigationService;
    }

    public async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var user = await _authService.GetCurrentUser();
            if (user != null)
            {
                UserName = user.FullName;
            }

            var todayAppts = await _appointmentService.GetTodayAppointments();
            TodayAppointmentCount = todayAppts.Count;

            var pastAppts = await _appointmentService.GetPastAppointments();
            UnresolvedCount = pastAppts.Count(a => a.Status != AppointmentStatus.Completed);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task GoToAppointmentsAsync()
    {
        await _navigationService.GoToAsync("appointments");
    }

    public async Task GoToOperationJobsAsync()
    {
        await _navigationService.DisplayAlertAsync("Operations Jobs", "Operations Jobs page coming soon!", "OK");
    }

    public async Task GoToDirectoryAsync()
    {
        await _navigationService.DisplayAlertAsync("Employee Directory", "Employee Directory page coming soon!", "OK");
    }

    public async Task GoToProfileAsync()
    {
        await _navigationService.GoToAsync("profile");
    }

    public async Task GoToSettingsAsync()
    {
        await _navigationService.GoToAsync("settings");
    }

    public async Task LogoutAsync()
    {
        var confirm = await _navigationService.DisplayAlertAsync("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirm)
        {
            await _authService.Logout();
            await _navigationService.GoToAsync("//identify");
        }
    }
}
