namespace IntegratorMobile.Views.Auth;

public partial class GetUsernamePage : ContentPage
{
    public GetUsernamePage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnConfirmPhoneCompleted(object? sender, EventArgs e)
    {
        await RequestUsername();
    }

    private async void OnGetUsernameClicked(object? sender, EventArgs e)
    {
        await RequestUsername();
    }

    private async Task RequestUsername()
    {
        var phone = PhoneInput.Text?.Trim();
        var confirmPhone = ConfirmPhoneInput.Text?.Trim();

        // Validate phone number
        if (string.IsNullOrEmpty(phone))
        {
            PhoneInput.HasError = true;
            PhoneInput.ErrorMessage = "Please enter your phone number";
            return;
        }

        PhoneInput.HasError = false;

        // Validate confirmation
        if (phone != confirmPhone)
        {
            ConfirmPhoneInput.HasError = true;
            ConfirmPhoneInput.ErrorMessage = "Phone numbers do not match";
            return;
        }

        ConfirmPhoneInput.HasError = false;
        GetUsernameButton.IsLoading = true;

        try
        {
            // Simulate API call
            await Task.Delay(800);

            // Show success message
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#DCFCE7");
            AlertTitle.TextColor = Color.FromArgb("#16A34A");
            AlertTitle.Text = "Success";
            AlertMessage.TextColor = Color.FromArgb("#16A34A");
            AlertMessage.Text = "Your username has been sent via text message.";
        }
        catch
        {
            AlertBox.IsVisible = true;
            AlertBox.BackgroundColor = Color.FromArgb("#FEE2E2");
            AlertTitle.TextColor = Color.FromArgb("#DC2626");
            AlertTitle.Text = "Account Not Found";
            AlertMessage.TextColor = Color.FromArgb("#DC2626");
            AlertMessage.Text = "No account found with this phone number.";
        }
        finally
        {
            GetUsernameButton.IsLoading = false;
        }
    }
}
