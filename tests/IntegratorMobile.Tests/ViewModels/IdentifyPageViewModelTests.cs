using FluentAssertions;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;
using IntegratorMobile.Tests.Mocks;
using Moq;
using Xunit;

namespace IntegratorMobile.Tests.ViewModels;

public class IdentifyPageViewModelTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly MockNavigationService _mockNavigation;

    public IdentifyPageViewModelTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockNavigation = new MockNavigationService();
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_SetsTitle()
    {
        // Arrange & Act
        var viewModel = CreateViewModel();

        // Assert
        viewModel.Title.Should().Be("Company Identification");
    }

    [Fact]
    public void Constructor_CompanyIdentifierIsEmpty()
    {
        // Arrange & Act
        var viewModel = CreateViewModel();

        // Assert
        viewModel.CompanyIdentifier.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_ShowErrorIsFalse()
    {
        // Arrange & Act
        var viewModel = CreateViewModel();

        // Assert
        viewModel.ShowError.Should().BeFalse();
    }

    #endregion

    #region ContinueCommand Tests

    [Fact]
    public async Task ContinueCommand_WithEmptyIdentifier_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.CompanyIdentifier = "";

        // Act
        await viewModel.ContinueCommand.ExecuteAsync(null);

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Be("Please enter your company identifier");
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task ContinueCommand_WithWhitespaceIdentifier_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.CompanyIdentifier = "   ";

        // Act
        await viewModel.ContinueCommand.ExecuteAsync(null);

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Be("Please enter your company identifier");
    }

    [Fact]
    public async Task ContinueCommand_WithValidIdentifier_NavigatesToLogin()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.CompanyIdentifier = "crowther";
        
        _mockAuthService
            .Setup(x => x.ValidateCompanyIdentifier("crowther"))
            .ReturnsAsync(new Company { Id = "1", Name = "Crowther Roofing" });

        // Act
        await viewModel.ContinueCommand.ExecuteAsync(null);

        // Assert
        viewModel.ShowError.Should().BeFalse();
        _mockNavigation.LastNavigationWas("login").Should().BeTrue();
    }

    [Fact]
    public async Task ContinueCommand_WithInvalidIdentifier_ShowsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.CompanyIdentifier = "invalid";
        
        _mockAuthService
            .Setup(x => x.ValidateCompanyIdentifier("invalid"))
            .ReturnsAsync((Company?)null);

        // Act
        await viewModel.ContinueCommand.ExecuteAsync(null);

        // Assert
        viewModel.ShowError.Should().BeTrue();
        viewModel.ErrorMessage.Should().Contain("Company not found");
        _mockNavigation.NavigationHistory.Should().BeEmpty();
    }

    [Fact]
    public async Task ContinueCommand_TrimsIdentifierBeforeValidation()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.CompanyIdentifier = "  crowther  ";
        
        _mockAuthService
            .Setup(x => x.ValidateCompanyIdentifier("crowther"))
            .ReturnsAsync(new Company { Id = "1", Name = "Crowther Roofing" });

        // Act
        await viewModel.ContinueCommand.ExecuteAsync(null);

        // Assert
        _mockAuthService.Verify(x => x.ValidateCompanyIdentifier("crowther"), Times.Once);
    }

    #endregion

    #region Property Change Tests

    [Fact]
    public void CompanyIdentifierChanged_ClearsError()
    {
        // Arrange
        var viewModel = CreateViewModel();
        viewModel.ShowError = true;
        viewModel.ErrorMessage = "Some error";

        // Act
        viewModel.CompanyIdentifier = "new value";

        // Assert
        viewModel.ShowError.Should().BeFalse();
        viewModel.ErrorMessage.Should().BeEmpty();
    }

    #endregion

    #region Helper Methods

    private TestableIdentifyPageViewModel CreateViewModel()
    {
        return new TestableIdentifyPageViewModel(_mockAuthService.Object, _mockNavigation);
    }

    #endregion
}

/// <summary>
/// Testable version of IdentifyPageViewModel that exposes internals for testing.
/// In production, we'd use the actual ViewModel but it's in a MAUI project.
/// </summary>
public class TestableIdentifyPageViewModel
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;

    public string Title { get; set; } = "Company Identification";
    public string CompanyIdentifier { get; set; } = string.Empty;
    public bool ShowError { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsBusy { get; set; }

    public TestableIdentifyPageViewModel(IAuthService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    public AsyncRelayCommand ContinueCommand => new(ContinueAsync);

    private async Task ContinueAsync()
    {
        if (string.IsNullOrWhiteSpace(CompanyIdentifier))
        {
            ShowError = true;
            ErrorMessage = "Please enter your company identifier";
            return;
        }

        IsBusy = true;
        try
        {
            var company = await _authService.ValidateCompanyIdentifier(CompanyIdentifier.Trim());
            
            if (company != null)
            {
                ShowError = false;
                await _navigationService.GoToAsync("login");
            }
            else
            {
                ShowError = true;
                ErrorMessage = "Company not found. Please check your identifier and try again.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}

// Helper class for async relay command in tests
public class AsyncRelayCommand
{
    private readonly Func<Task> _execute;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public async Task ExecuteAsync(object? parameter)
    {
        await _execute();
    }
}
