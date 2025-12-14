namespace IntegratorMobile.Views.Appointments;

public partial class AddWorkItemPage : ContentPage
{
    private int _photoCount = 0;
    private bool _typeSelected = false;
    private bool _buildingSelected = false;
    private bool _systemSelected = false;

    public AddWorkItemPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void OnTypeSelected(object? sender, EventArgs e)
    {
        _typeSelected = TypePicker.SelectedIndex >= 0;
        ValidateForm();
    }

    private void OnBuildingSelected(object? sender, EventArgs e)
    {
        _buildingSelected = BuildingPicker.SelectedIndex >= 0;
        SystemPicker.IsEnabled = _buildingSelected;
        
        if (!_buildingSelected)
        {
            SystemPicker.SelectedIndex = -1;
            _systemSelected = false;
        }
        
        ValidateForm();
    }

    private void OnSystemSelected(object? sender, EventArgs e)
    {
        _systemSelected = SystemPicker.SelectedIndex >= 0;
        ValidateForm();
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

        PhotoError.IsVisible = false;
        ValidateForm();
    }

    private void ValidateForm()
    {
        var titleValid = !string.IsNullOrWhiteSpace(TitleInput.Text);
        var hasPhotos = _photoCount > 0;

        SaveButton.IsEnabled = _typeSelected && _buildingSelected && _systemSelected && titleValid && hasPhotos;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        // Validate title
        if (string.IsNullOrWhiteSpace(TitleInput.Text))
        {
            TitleInput.HasError = true;
            TitleInput.ErrorMessage = "Please enter a title";
            return;
        }

        // Validate photos
        if (_photoCount == 0)
        {
            PhotoError.IsVisible = true;
            return;
        }

        // Simulate saving
        SaveButton.IsLoading = true;
        await Task.Delay(500);
        
        // Navigate back to appointment detail
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
