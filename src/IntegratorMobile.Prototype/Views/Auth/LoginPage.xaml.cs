namespace IntegratorMobile.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnMicrosoftLoginClicked(object? sender, EventArgs e)
    {
        // Simulate Microsoft login - go to home
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnManualLoginClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("login/manual");
    }
}
