using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IAppointmentService _appointmentService;

    [ObservableProperty]
    private string _userName = "John Smith";

    [ObservableProperty]
    private int _todayAppointmentCount;

    [ObservableProperty]
    private int _unresolvedCount;

    public HomePageViewModel(IAuthService authService, IAppointmentService appointmentService)
    {
        _authService = authService;
        _appointmentService = appointmentService;
        Title = "Home";
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var user = await _authService.GetCurrentUser();
            if (user != null)
            {
                UserName = user.FullName;
            }

            var todayAppts = await _appointmentService.GetTodayAppointments();
            TodayAppointmentCount = todayAppts.Count;

            var pastAppts = await _appointmentService.GetPastAppointments();
            UnresolvedCount = pastAppts.Count(a => a.Status != MockData.Models.AppointmentStatus.Completed);
        });
    }

    [RelayCommand]
    private async Task GoToAppointmentsAsync()
    {
        await Shell.Current.GoToAsync("appointments");
    }

    [RelayCommand]
    private async Task GoToOperationJobsAsync()
    {
        await Shell.Current.DisplayAlert("Operations Jobs", "Operations Jobs page coming soon!", "OK");
    }

    [RelayCommand]
    private async Task GoToDirectoryAsync()
    {
        await Shell.Current.DisplayAlert("Employee Directory", "Employee Directory page coming soon!", "OK");
    }

    [RelayCommand]
    private async Task GoToProfileAsync()
    {
        await Shell.Current.GoToAsync("profile");
    }

    [RelayCommand]
    private async Task GoToSettingsAsync()
    {
        await Shell.Current.GoToAsync("settings");
    }

    [RelayCommand]
    private async Task GoToDiagnosticsAsync()
    {
        await Shell.Current.DisplayAlert("Diagnostics", "Diagnostics page coming soon!", "OK");
    }

    [RelayCommand]
    private async Task GoToHelpAsync()
    {
        await Shell.Current.DisplayAlert("Help", "Help page coming soon!", "OK");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirm = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirm)
        {
            await _authService.Logout();
            await Shell.Current.GoToAsync("//identify");
        }
    }
}
