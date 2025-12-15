using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using IntegratorMobile.Tests.Mocks;
using Moq;
using Xunit;

namespace IntegratorMobile.Tests.ViewModels;

public class ManualLoginPageViewModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly MockNavigationService _mockNavigation;

    public ManualLoginPageViewModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockNavigation = new MockNavigationService();
    }

    #region Validation Tests

    [Fact]
    public async Task Login_WithEmptyUsername_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "";
        viewModel.Password = "password";

        // Act
        await viewModel.LoginAsync();

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Contain("username");
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "jsmith";
        viewModel.Password = "";

        // Act
        await viewModel.LoginAsync();

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Contain("password");
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task Login_WithWhitespaceUsername_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "   ";
        viewModel.Password = "password";

        // Act
        await viewModel.LoginAsync();

        // Assert
        viewModel.ShowError.Should().BeTrue();
    }

    #endregion

    #region Login Tests

    [Fact]
    public async Task Login_WithValidCredentials_NavigatesToHome()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "jsmith";
        viewModel.Password = "password";
        
        _mockAuthService
            .Setup(x => x.LoginWithCredentials("jsmith", "password"))
            .ReturnsAsync(new User { Id = "1", Username = "jsmith" });

        // Act
        await viewModel.LoginAsync();

        // Assert
        viewModel.ShowError.Should().BeFalse();
        _mockNavigation.LastNavigationWas("//home").Should().BeTrue();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "jsmith";
        viewModel.Password = "wrongpassword";
        
        _mockAuthService
            .Setup(x => x.LoginWithCredentials("jsmith", "wrongpassword"))
            .ReturnsAsync((User?)null);

        // Act
        await viewModel.LoginAsync();

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Contain("Invalid");
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task Login_TrimsUsername()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.Username = "  jsmith  ";
        viewModel.Password = "password";
        
        _mockAuthService
            .Setup(x => x.LoginWithCredentials("jsmith", "password"))
            .ReturnsAsync(new User { Id = "1", Username = "jsmith" });

        // Act
        await viewModel.LoginAsync();

        // Assert
        _mockAuthService.Verify(x => x.LoginWithCredentials("jsmith", "password"), Times.Once);
    }

    #endregion

    #region Property Change Tests

    [Fact]
    public void UsernameChanged_ClearsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.ShowError = true;
        viewModel.ErrorMessage = "Some error";

        // Act
        viewModel.OnUsernameChanged("new");

        // Assert
        viewModel.ShowError.Should().BeFalse();
        viewModel.ErrorMessage.Should().BeEmpty();
    }

    [Fact]
    public void PasswordChanged_ClearsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.ShowError = true;
        viewModel.ErrorMessage = "Some error";

        // Act
        viewModel.OnPasswordChanged("new");

        // Assert
        viewModel.ShowError.Should().BeFalse();
        viewModel.ErrorMessage.Should().BeEmpty();
    }

    #endregion

    #region Navigation Tests

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

    private TestableManualLoginPageViewModel CreateViewModel()
    {
        return new TestableManualLoginPageViewModel(_mockAuthService.Object, _mockNavigation);
    }

    #endregion
}

public class TestableManualLoginPageViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    public string Title { get; set; } = "Manual Login";
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool ShowError { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsBusy { get; set; }

    public TestableManualLoginPageViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            ShowError = true;
            ErrorMessage = "Please enter your username";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ShowError = true;
            ErrorMessage = "Please enter your password";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            var user = await _authService.LoginWithCredentials(Username.Trim(), Password);
            
            if (user != null)
            {
                ShowError = false;
                await _navigationService.GoToAsync("//home");
            }
            else
            {
                ShowError = true;
                ErrorMessage = "Invalid username or password. Please try again.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task GoBackAsync()
    {
        await _navigationService.GoBackAsync();
    }

    public void OnUsernameChanged(string value)
    {
        ShowError = false;
        ErrorMessage = string.Empty;
    }

    public void OnPasswordChanged(string value)
    {
        ShowError = false;
        ErrorMessage = string.Empty;
    }
}
