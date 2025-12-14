namespace IntegratorMobile.Views.Auth;

public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnConfirmPasswordCompleted(object? sender, EventArgs e)
    {
        await ResetPassword();
    }

    private async void OnResetClicked(object? sender, EventArgs e)
    {
        await ResetPassword();
    }

    private async Task ResetPassword()
    {
        // Clear previous errors
        UsernameInput.HasError = false;
        ResetCodeInput.HasError = false;
        NewPasswordInput.HasError = false;
        ConfirmPasswordInput.HasError = false;

        var username = UsernameInput.Text?.Trim();
        var resetCode = ResetCodeInput.Text?.Trim();
        var newPassword = NewPasswordInput.Text;
        var confirmPassword = ConfirmPasswordInput.Text;

        // Validate username
        if (string.IsNullOrEmpty(username))
        {
            UsernameInput.HasError = true;
            UsernameInput.ErrorMessage = "Please enter your username";
            return;
        }

        // Validate reset code
        if (string.IsNullOrEmpty(resetCode))
        {
            ResetCodeInput.HasError = true;
            ResetCodeInput.ErrorMessage = "Please enter the reset code";
            return;
        }

        // Validate new password
        if (string.IsNullOrEmpty(newPassword))
        {
            NewPasswordInput.HasError = true;
            NewPasswordInput.ErrorMessage = "Please enter a new password";
            return;
        }

        // Validate password match
        if (newPassword != confirmPassword)
        {
            ConfirmPasswordInput.HasError = true;
            ConfirmPasswordInput.ErrorMessage = "Passwords do not match";
            return;
        }

        ResetButton.IsLoading = true;

        try
        {
            // Simulate API call
            await Task.Delay(800);

            // Show success message
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#DCFCE7");
            AlertMessage.TextColor = Color.FromArgb("#16A34A");
            AlertMessage.Text = "Password reset successfully. Redirecting to login...";

            // Navigate to manual login after delay
            await Task.Delay(2000);
            await Shell.Current.GoToAsync("//identify/login/manual");
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
            ResetButton.IsLoading = false;
        }
    }
}
