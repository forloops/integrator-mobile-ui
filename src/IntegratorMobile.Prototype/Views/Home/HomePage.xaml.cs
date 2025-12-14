namespace IntegratorMobile.Views.Home;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnMenuClicked(object? sender, EventArgs e)
    {
        // Show menu options as action sheet
        var action = await DisplayActionSheet("Menu", "Cancel", null,
            "Home", "Appointments", "Operation Jobs", "Employee Directory",
            "My Profile", "Settings", "Diagnostics", "Help", "Logout");
        
        switch (action)
        {
            case "Appointments":
                await Shell.Current.GoToAsync("//appointments");
                break;
            case "My Profile":
                await Shell.Current.GoToAsync("//profile");
                break;
            case "Settings":
                await Shell.Current.GoToAsync("//settings");
                break;
            case "Logout":
                await Shell.Current.GoToAsync("//identify");
                break;
        }
    }

    private async void OnAppointmentsTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//appointments");
    }
}
