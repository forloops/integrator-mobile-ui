using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class IdentifyPageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _companyIdentifier = string.Empty;

    [ObservableProperty]
    private bool _showError;

    public IdentifyPageViewModel(IAuthService authService, INavigationService navigationService)
        : base(navigationService)
    {
        _authService = authService;
        Title = "Company Identification";
    }

    [RelayCommand]
    private async Task ContinueAsync()
    {
        if (string.IsNullOrWhiteSpace(CompanyIdentifier))
        {
            ShowError = true;
            ErrorMessage = "Please enter your company identifier";
            return;
        }

        await ExecuteAsync(async () =>
        {
            var company = await _authService.ValidateCompanyIdentifier(CompanyIdentifier.Trim());
            
            if (company != null)
            {
                ShowError = false;
                // Navigate to login page
                await NavigationService!.GoToAsync("login");
            }
            else
            {
                ShowError = true;
                ErrorMessage = "Company not found. Please check your identifier and try again.";
            }
        });
    }

    partial void OnCompanyIdentifierChanged(string value)
    {
        ShowError = false;
        ErrorMessage = string.Empty;
    }
}
