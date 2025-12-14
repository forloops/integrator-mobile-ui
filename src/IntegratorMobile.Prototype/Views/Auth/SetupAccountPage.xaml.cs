namespace IntegratorMobile.Views.Auth;

public partial class SetupAccountPage : ContentPage
{
    public SetupAccountPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnPhoneCompleted(object? sender, EventArgs e)
    {
        await RequestAccountInfo();
    }

    private async void OnRequestClicked(object? sender, EventArgs e)
    {
        await RequestAccountInfo();
    }

    private async Task RequestAccountInfo()
    {
        var phone = PhoneInput.Text?.Trim();

        // Validate phone number
        if (string.IsNullOrEmpty(phone))
        {
            PhoneInput.HasError = true;
            PhoneInput.ErrorMessage = "Please enter your phone number";
            return;
        }

        PhoneInput.HasError = false;
        RequestButton.IsLoading = true;

        try
        {
            // Simulate API call
            await Task.Delay(800);

            // Show success message
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#DCFCE7");
            AlertMessage.TextColor = Color.FromArgb("#16A34A");
            AlertMessage.Text = "Your account information has been sent via text message. Please check your phone.";
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
            RequestButton.IsLoading = false;
        }
    }
}
