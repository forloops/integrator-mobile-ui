namespace IntegratorMobile.Views.Appointments;

public partial class AddMilestonePage : ContentPage
{
    private string _selectedType = "completed";
    private int _photoCount = 0;

    public AddMilestonePage()
    {
        InitializeComponent();
        UpdateTypeSelection();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void OnMilestoneTypeSelected(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is string type)
        {
            _selectedType = type;
            UpdateTypeSelection();
            UpdatePreview();
        }
    }

    private void UpdateTypeSelection()
    {
        // Reset all options
        BeforeOption.Stroke = Color.FromArgb("#E2E8F0");
        InProgressOption.Stroke = Color.FromArgb("#E2E8F0");
        CompletedOption.Stroke = Color.FromArgb("#E2E8F0");
        CustomOption.Stroke = Color.FromArgb("#E2E8F0");

        // Highlight selected
        var selectedColor = Color.FromArgb("#4338CA");
        switch (_selectedType)
        {
            case "before":
                BeforeOption.Stroke = selectedColor;
                CustomNameSection.IsVisible = false;
                break;
            case "in_progress":
                InProgressOption.Stroke = selectedColor;
                CustomNameSection.IsVisible = false;
                break;
            case "completed":
                CompletedOption.Stroke = selectedColor;
                CustomNameSection.IsVisible = false;
                break;
            case "custom":
                CustomOption.Stroke = selectedColor;
                CustomNameSection.IsVisible = true;
                break;
        }
    }

    private void OnAddPhotoClicked(object? sender, TappedEventArgs e)
    {
        // Simulate adding a photo
        _photoCount++;
        
        // Add a mock photo to the grid
        var photoView = new Border
        {
            WidthRequest = 80,
            HeightRequest = 80,
            BackgroundColor = Color.FromArgb("#E2E8F0"),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            Margin = new Thickness(0, 0, 8, 8),
            Content = new Label
            {
                Text = "📷",
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };

        // Insert before the "Add" button
        PhotoGrid.Insert(PhotoGrid.Children.Count - 1, photoView);

        UpdatePreview();
        PhotoError.IsVisible = false;
    }

    private void UpdatePreview()
    {
        if (_photoCount > 0)
        {
            PreviewCard.IsVisible = true;
            PreviewPhotoCount.Text = $"{_photoCount} photo{(_photoCount != 1 ? "s" : "")}";
            
            // Update name based on type
            PreviewName.Text = _selectedType switch
            {
                "before" => "Before",
                "in_progress" => "In Progress",
                "completed" => "Completed",
                "custom" => string.IsNullOrEmpty(CustomNameInput.Text) ? "Custom" : CustomNameInput.Text,
                _ => "Milestone"
            };

            SaveButton.IsEnabled = true;
        }
        else
        {
            PreviewCard.IsVisible = false;
            SaveButton.IsEnabled = false;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (_photoCount == 0)
        {
            PhotoError.IsVisible = true;
            return;
        }

        if (_selectedType == "custom" && string.IsNullOrWhiteSpace(CustomNameInput.Text))
        {
            CustomNameInput.HasError = true;
            CustomNameInput.ErrorMessage = "Please enter a milestone name";
            return;
        }

        // Simulate saving
        SaveButton.IsLoading = true;
        await Task.Delay(500);
        
        // Navigate back
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
