namespace IntegratorMobile.Views.Auth;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnUsernameCompleted(object? sender, EventArgs e)
    {
        await RequestResetCode();
    }

    private async void OnGetCodeClicked(object? sender, EventArgs e)
    {
        await RequestResetCode();
    }

    private async Task RequestResetCode()
    {
        var username = UsernameInput.Text?.Trim();
        if (string.IsNullOrEmpty(username))
        {
            UsernameInput.HasError = true;
            UsernameInput.ErrorMessage = "Please enter your username";
            return;
        }

        UsernameInput.HasError = false;
        GetCodeButton.IsLoading = true;

        try
        {
            // Simulate API call
            await Task.Delay(800);

            // Show success message
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#DCFCE7");
            AlertMessage.TextColor = Color.FromArgb("#16A34A");
            AlertMessage.Text = "A reset code has been sent to your phone. Check your text messages.";
        }
        catch
        {
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#FEE2E2");
            AlertMessage.TextColor = Color.FromArgb("#DC2626");
            AlertMessage.Text = "An error occurred. Please try again.";
        }
        finally
        {
            GetCodeButton.IsLoading = false;
        }
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
