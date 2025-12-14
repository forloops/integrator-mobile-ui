namespace IntegratorMobile.Views.Appointments;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class ArrivalPhotosPage : ContentPage
{
    public string AppointmentId { get; set; } = string.Empty;
    private int _photoCount = 0;

    public ArrivalPhotosPage()
    {
        InitializeComponent();
    }

    private async void OnTakePhotoClicked(object? sender, EventArgs e)
    {
        // For prototype, simulate taking a photo
        var result = await DisplayAlert(
            "Camera",
            "In the full app, this would open the camera.\n\nSimulate taking a photo?",
            "Take Photo",
            "Cancel");

        if (result)
        {
            _photoCount++;
            UpdatePhotoUI();
        }
    }

    private void UpdatePhotoUI()
    {
        // Update UI to show photo was captured
        PhotoPlaceholder.IsVisible = false;
        
        // Show a placeholder "captured" image
        CapturedPhoto.IsVisible = true;
        CapturedPhoto.BackgroundColor = Color.FromArgb("#E2E8F0");
        
        TakePhotoButton.IsVisible = false;
        TakeAnotherButton.IsVisible = true;
        
        PhotoCountLabel.Text = $"{_photoCount} photo{(_photoCount > 1 ? "s" : "")} captured";
        PhotoCountLabel.IsVisible = true;
        
        CompleteButton.IsEnabled = true;
    }

    private async void OnCompleteClicked(object? sender, EventArgs e)
    {
        if (_photoCount == 0)
        {
            await DisplayAlert("Photo Required", "Please take at least one arrival photo to continue.", "OK");
            return;
        }

        // Navigate to survey/work area
        await Shell.Current.GoToAsync($"../system?id={AppointmentId}");
    }
}
