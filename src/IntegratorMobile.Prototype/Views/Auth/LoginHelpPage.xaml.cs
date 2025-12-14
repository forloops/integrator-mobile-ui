namespace IntegratorMobile.Views.Auth;

public partial class LoginHelpPage : ContentPage
{
    public LoginHelpPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSetupAccountClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("setup-account");
    }

    private async void OnForgotPasswordClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("forgot-password");
    }

    private async void OnResetPasswordClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("reset-password");
    }

    private async void OnForgotUsernameClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("get-username");
    }
}
