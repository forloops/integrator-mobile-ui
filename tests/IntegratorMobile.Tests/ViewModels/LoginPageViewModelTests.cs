using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using IntegratorMobile.Tests.Mocks;
using Moq;
using Xunit;

namespace IntegratorMobile.Tests.ViewModels;

public class LoginPageViewModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly MockNavigationService _mockNavigation;

    public LoginPageViewModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockNavigation = new MockNavigationService();
    }

    #region LoginWithMicrosoft Tests

    [Fact]
    public async Task LoginWithMicrosoft_Success_NavigatesToHome()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockAuthService
            .Setup(x => x.LoginWithMicrosoft())
            .ReturnsAsync(new User { Id = "1", Username = "jsmith" });

        // Act
        await viewModel.LoginWithMicrosoftAsync();

        // Assert
        _mockNavigation.LastNavigationWas("//home").Should().BeTrue();
        viewModel.ErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public async Task LoginWithMicrosoft_Failure_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        _mockAuthService
            .Setup(x => x.LoginWithMicrosoft())
            .ReturnsAsync((User?)null);

        // Act
        await viewModel.LoginWithMicrosoftAsync();

        // Assert
        _mockNavigation.NavigationHistory.Should().BeEmpty();
        viewModel.ErrorMessage.Should().Contain("Login failed");
    }

    #endregion

    #region Navigation Tests

    [Fact]
    public async Task GoToManualLogin_NavigatesToManualLoginPage()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToManualLoginAsync();

        // Assert
        _mockNavigation.LastNavigationWas("login/manual").Should().BeTrue();
    }

    [Fact]
    public async Task GoToLoginHelp_DisplaysAlert()
    {
        // Arrange
        var viewModel = CreateViewModel();

        // Act
        await viewModel.GoToLoginHelpAsync();

        // Assert
        _mockNavigation.SimpleAlertHistory.Should().HaveCount(1);
        _mockNavigation.SimpleAlertHistory[0].Title.Should().Be("Login Help");
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

    #region Helper Methods

    private TestableLoginPageViewModel CreateViewModel()
    {
        return new TestableLoginPageViewModel(_mockAuthService.Object, _mockNavigation);
    }

    #endregion
}

public class TestableLoginPageViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    public string Title { get; set; } = "Login";
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsBusy { get; set; }

    public TestableLoginPageViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    public async Task LoginWithMicrosoftAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            var user = await _authService.LoginWithMicrosoft();
            
            if (user != null)
            {
                await _navigationService.GoToAsync("//home");
            }
            else
            {
                ErrorMessage = "Login failed. Please try again.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task GoToManualLoginAsync()
    {
        await _navigationService.GoToAsync("login/manual");
    }

    public async Task GoToLoginHelpAsync()
    {
        await _navigationService.DisplayAlertAsync("Login Help", 
            "Need help logging in?\n\n• Forgot Username\n• Forgot Password\n• Setup Account\n\nContact your administrator for assistance.", 
            "OK");
    }

    public async Task GoBackAsync()
    {
        await _navigationService.GoBackAsync();
    }
}
