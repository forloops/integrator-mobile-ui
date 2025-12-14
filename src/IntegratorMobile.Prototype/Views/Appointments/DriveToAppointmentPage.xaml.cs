namespace IntegratorMobile.Views.Appointments;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class DriveToAppointmentPage : ContentPage
{
    public string AppointmentId { get; set; } = string.Empty;

    public DriveToAppointmentPage()
    {
        InitializeComponent();
    }

    private async void OnNavigateClicked(object? sender, EventArgs e)
    {
        // Open maps with address
        var address = "1234 Sunset Boulevard, Los Angeles, CA 90028";
        var encodedAddress = Uri.EscapeDataString(address);
        
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Launcher.OpenAsync($"maps://?address={encodedAddress}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Launcher.OpenAsync($"geo:0,0?q={encodedAddress}");
            }
        }
        catch
        {
            await DisplayAlert("Navigation", $"Navigate to:\n{address}", "OK");
        }
    }

    private async void OnSkipClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"../arrival?id={AppointmentId}");
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        var action = await DisplayActionSheet(
            "Cancel or Reschedule",
            "Cancel",
            null,
            "Reschedule Appointment",
            "Cancel Appointment");

        if (action == "Cancel Appointment")
        {
            var confirm = await DisplayAlert(
                "Cancel Appointment",
                "Are you sure you want to cancel this appointment?",
                "Yes",
                "No");

            if (confirm)
            {
                await Shell.Current.GoToAsync("../..");
            }
        }
    }
}
