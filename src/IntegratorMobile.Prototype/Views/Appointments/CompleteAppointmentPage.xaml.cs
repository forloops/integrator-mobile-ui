namespace IntegratorMobile.Views.Appointments;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class CompleteAppointmentPage : ContentPage
{
    public string AppointmentId { get; set; } = string.Empty;

    public CompleteAppointmentPage()
    {
        InitializeComponent();
    }

    private async void OnCompleteClicked(object? sender, EventArgs e)
    {
        // Show return scheduling prompt
        var scheduleReturn = await DisplayAlert(
            "Schedule Return Visit",
            "One or more work items require a return visit.\n\nWould you like to schedule the return visit now?",
            "Schedule Now",
            "Schedule Later");

        if (scheduleReturn)
        {
            // In full app, this would open a date picker
            await DisplayAlert(
                "Return Visit Scheduled",
                "A return visit has been scheduled for next week.\n\nYou can modify this in the appointments list.",
                "OK");
        }

        await DisplayAlert(
            "Appointment Completed",
            "The appointment has been marked as completed.\n\nAll photos and data have been saved.",
            "OK");

        // Navigate back to appointments list
        await Shell.Current.GoToAsync("//appointments");
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
