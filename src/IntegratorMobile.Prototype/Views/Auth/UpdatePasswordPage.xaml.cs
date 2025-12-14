namespace IntegratorMobile.Views.Auth;

public partial class UpdatePasswordPage : ContentPage
{
    public UpdatePasswordPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnConfirmPasswordCompleted(object? sender, EventArgs e)
    {
        await UpdatePassword();
    }

    private async void OnUpdateClicked(object? sender, EventArgs e)
    {
        await UpdatePassword();
    }

    private async Task UpdatePassword()
    {
        // Clear previous errors
        PasswordInput.HasError = false;
        ConfirmPasswordInput.HasError = false;

        var password = PasswordInput.Text;
        var confirmPassword = ConfirmPasswordInput.Text;

        // Validate password
        if (string.IsNullOrEmpty(password))
        {
            PasswordInput.HasError = true;
            PasswordInput.ErrorMessage = "Please enter a new password";
            return;
        }

        // Validate password length
        if (password.Length < 8)
        {
            PasswordInput.HasError = true;
            PasswordInput.ErrorMessage = "Password must be at least 8 characters";
            return;
        }

        // Validate password match
        if (password != confirmPassword)
        {
            ConfirmPasswordInput.HasError = true;
            ConfirmPasswordInput.ErrorMessage = "Passwords do not match";
            return;
        }

        UpdateButton.IsLoading = true;

        try
        {
            // Simulate API call
            await Task.Delay(800);

            // Show success message
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#DCFCE7");
            AlertMessage.TextColor = Color.FromArgb("#16A34A");
            AlertMessage.Text = "Password updated successfully. Redirecting to login...";

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
            UpdateButton.IsLoading = false;
        }
    }
}
