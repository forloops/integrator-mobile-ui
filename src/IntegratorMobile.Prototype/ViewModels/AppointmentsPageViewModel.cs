using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class AppointmentsPageViewModel : BaseViewModel
{
    private readonly IAppointmentService _appointmentService;

    [ObservableProperty]
    private ObservableCollection<Appointment> _todayAppointments = new();

    [ObservableProperty]
    private ObservableCollection<Appointment> _unresolvedAppointments = new();

    [ObservableProperty]
    private ObservableCollection<Appointment> _futureAppointments = new();

    [ObservableProperty]
    private int _selectedTabIndex = 1; // Default to Today tab

    [ObservableProperty]
    private bool _hasTodayAppointments;

    [ObservableProperty]
    private bool _hasUnresolvedAppointments;

    [ObservableProperty]
    private bool _hasFutureAppointments;

    public AppointmentsPageViewModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
        Title = "My Appointments";
    }

    [RelayCommand]
    private async Task LoadAppointmentsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Load today's appointments
            var today = await _appointmentService.GetTodayAppointments();
            TodayAppointments = new ObservableCollection<Appointment>(today);
            HasTodayAppointments = today.Any();

            // Load unresolved (past incomplete)
            var past = await _appointmentService.GetPastAppointments();
            var unresolved = past.Where(a => a.Status != AppointmentStatus.Completed).ToList();
            UnresolvedAppointments = new ObservableCollection<Appointment>(unresolved);
            HasUnresolvedAppointments = unresolved.Any();

            // Load future
            var future = await _appointmentService.GetFutureAppointments();
            FutureAppointments = new ObservableCollection<Appointment>(future);
            HasFutureAppointments = future.Any();
        });
    }

    [RelayCommand]
    private async Task SelectAppointmentAsync(Appointment appointment)
    {
        if (appointment == null) return;

        await Shell.Current.GoToAsync($"appointments/detail?id={appointment.Id}");
    }

    [RelayCommand]
    private async Task DoneForDayAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Complete My Day",
            "This will mark all remaining appointments as 'Need to Return'. Are you sure?",
            "Yes, I'm Done",
            "Cancel");

        if (confirm)
        {
            await ExecuteAsync(async () =>
            {
                foreach (var appointment in TodayAppointments.Where(a => a.Status != AppointmentStatus.Completed))
                {
                    await _appointmentService.UpdateAppointmentStatus(appointment.Id, AppointmentStatus.Rescheduled);
                }
                
                await LoadAppointmentsAsync();
                await Shell.Current.DisplayAlert("Done", "Your day has been completed. Remaining appointments have been marked for rescheduling.", "OK");
            });
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadAppointmentsAsync();
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void SetTab(string tabIndex)
    {
        if (int.TryParse(tabIndex, out int index))
        {
            SelectedTabIndex = index;
        }
    }
}
