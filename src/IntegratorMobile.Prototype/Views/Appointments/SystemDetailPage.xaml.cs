namespace IntegratorMobile.Views.Appointments;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class SystemDetailPage : ContentPage
{
    public string AppointmentId { get; set; } = string.Empty;

    public SystemDetailPage()
    {
        InitializeComponent();
    }

    private async void OnBuildingATapped(object? sender, TappedEventArgs e)
    {
        await DisplayAlert("Building A", "Showing systems for Building A", "OK");
    }

    private async void OnBuildingBTapped(object? sender, TappedEventArgs e)
    {
        await DisplayAlert("Building B", "Showing systems for Building B", "OK");
    }

    private async void OnSystemTapped(object? sender, TappedEventArgs e)
    {
        // Navigate to work item detail for this system
        await Shell.Current.GoToAsync($"../workitem?id=wi-001");
    }

    private async void OnAddWorkItemClicked(object? sender, EventArgs e)
    {
        var workItemType = await DisplayActionSheet(
            "Select Work Item Type",
            "Cancel",
            null,
            "Inspection",
            "Survey",
            "Estimate",
            "Adhoc Repair",
            "Line Item Repair");

        if (!string.IsNullOrEmpty(workItemType) && workItemType != "Cancel")
        {
            await DisplayAlert(
                "Add Work Item",
                $"Creating new {workItemType}...\n\nIn the full app, you would now select a building/system and take before photos.",
                "OK");
        }
    }

    private async void OnCompleteSurveyClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"../complete?id={AppointmentId}");
    }
}
