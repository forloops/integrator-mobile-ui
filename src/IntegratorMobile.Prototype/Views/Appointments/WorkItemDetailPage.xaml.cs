namespace IntegratorMobile.Views.Appointments;

[QueryProperty(nameof(WorkItemId), "id")]
public partial class WorkItemDetailPage : ContentPage
{
    public string WorkItemId { get; set; } = string.Empty;
    
    private int _beforePhotoCount = 0;
    private int _completedPhotoCount = 0;

    public WorkItemDetailPage()
    {
        InitializeComponent();
    }

    private async void OnBeforeMilestoneTapped(object? sender, TappedEventArgs e)
    {
        if (_beforePhotoCount > 0)
        {
            await DisplayAlert("Before Photos", $"Viewing {_beforePhotoCount} before photo(s)", "OK");
        }
    }

    private async void OnAddBeforePhotoClicked(object? sender, EventArgs e)
    {
        var result = await DisplayAlert(
            "Camera",
            "In the full app, this would open the camera.\n\nSimulate taking a before photo?",
            "Take Photo",
            "Cancel");

        if (result)
        {
            _beforePhotoCount++;
            UpdateUI();
        }
    }

    private async void OnAddCompletedPhotoClicked(object? sender, EventArgs e)
    {
        var result = await DisplayAlert(
            "Camera",
            "In the full app, this would open the camera.\n\nSimulate taking a completed photo?",
            "Take Photo",
            "Cancel");

        if (result)
        {
            _completedPhotoCount++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // Update Before milestone
        BeforePhotoCount.Text = $"{_beforePhotoCount} photo{(_beforePhotoCount != 1 ? "s" : "")}";
        
        if (_beforePhotoCount > 0)
        {
            BeforeIcon.BackgroundColor = Color.FromArgb("#16A34A");
            BeforeIcon.Stroke = Color.FromArgb("#16A34A");
            ((Label)BeforeIcon.Content).Text = "✓";
            ((Label)BeforeIcon.Content).TextColor = Colors.White;
            
            // Enable completed milestone
            AddCompletedButton.IsEnabled = true;
            CompletedLabel.TextColor = Color.FromArgb("#0F172A");
        }
        
        // Update Completed milestone
        CompletedPhotoCount.Text = _completedPhotoCount > 0 
            ? $"{_completedPhotoCount} photo{(_completedPhotoCount != 1 ? "s" : "")}"
            : "Required to complete work item";
        
        if (_completedPhotoCount > 0)
        {
            CompletedIcon.BackgroundColor = Color.FromArgb("#16A34A");
            CompletedIcon.Stroke = Color.FromArgb("#16A34A");
            ((Label)CompletedIcon.Content).Text = "✓";
            ((Label)CompletedIcon.Content).TextColor = Colors.White;
        }
        
        // Enable complete button if both milestones have photos
        CompleteButton.IsEnabled = _beforePhotoCount > 0 && _completedPhotoCount > 0;
        
        // Update status badge
        if (_beforePhotoCount > 0 && _completedPhotoCount == 0)
        {
            StatusBadge.Text = "IN PROGRESS";
            StatusBadge.BadgeType = IntegratorMobile.UI.Controls.BadgeType.InProgress;
        }
    }

    private async void OnMarkCompletedClicked(object? sender, EventArgs e)
    {
        if (_beforePhotoCount == 0 || _completedPhotoCount == 0)
        {
            await DisplayAlert(
                "Photos Required",
                "You need at least 1 before photo and 1 completed photo to mark this work item as completed.",
                "OK");
            return;
        }

        var confirm = await DisplayAlert(
            "Complete Work Item",
            "Are you sure you want to mark this work item as completed?",
            "Yes, Complete",
            "Cancel");

        if (confirm)
        {
            await DisplayAlert("Work Item Completed", "The work item has been marked as completed.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnNeedToReturnClicked(object? sender, EventArgs e)
    {
        var reason = await DisplayPromptAsync(
            "Need to Return",
            "Please provide a reason for needing to return:",
            placeholder: "e.g., Waiting for parts");

        if (!string.IsNullOrEmpty(reason))
        {
            await DisplayAlert(
                "Marked for Return",
                $"This work item has been marked as 'Need to Return'.\n\nReason: {reason}",
                "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
