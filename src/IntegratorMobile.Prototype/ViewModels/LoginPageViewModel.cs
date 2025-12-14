using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public LoginPageViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginWithMicrosoftAsync()
    {
        await ExecuteAsync(async () =>
        {
            var user = await _authService.LoginWithMicrosoft();
            
            if (user != null)
            {
                // Navigate to home - replace the navigation stack
                await Shell.Current.GoToAsync("//home");
            }
            else
            {
                ErrorMessage = "Login failed. Please try again.";
            }
        });
    }

    [RelayCommand]
    private async Task GoToManualLoginAsync()
    {
        await Shell.Current.GoToAsync("login/manual");
    }

    [RelayCommand]
    private async Task GoToLoginHelpAsync()
    {
        // For prototype, just show an alert
        await Shell.Current.DisplayAlert("Login Help", 
            "Need help logging in?\n\n• Forgot Username\n• Forgot Password\n• Setup Account\n\nContact your administrator for assistance.", 
            "OK");
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
