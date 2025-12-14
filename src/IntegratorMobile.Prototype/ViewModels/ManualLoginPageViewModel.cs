using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class ManualLoginPageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _showError;

    public ManualLoginPageViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Manual Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
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

        await ExecuteAsync(async () =>
        {
            var user = await _authService.LoginWithCredentials(Username.Trim(), Password);
            
            if (user != null)
            {
                ShowError = false;
                // Navigate to home - replace the navigation stack
                await Shell.Current.GoToAsync("//home");
            }
            else
            {
                ShowError = true;
                ErrorMessage = "Invalid username or password. Please try again.";
            }
        });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    partial void OnUsernameChanged(string value)
    {
        ShowError = false;
        ErrorMessage = string.Empty;
    }

    partial void OnPasswordChanged(string value)
    {
        ShowError = false;
        ErrorMessage = string.Empty;
    }
}
