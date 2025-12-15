using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

public partial class AppointmentsPageViewModel : BaseViewModel
{
    private readonly IAppointmentService _appointmentService;
    
    // Original lists for filtering
    private List<Appointment> _allTodayAppointments = new();
    private List<Appointment> _allUnresolvedAppointments = new();
    private List<Appointment> _allFutureAppointments = new();

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
    
    [ObservableProperty]
    private bool _isSearchVisible;
    
    [ObservableProperty]
    private string _searchQuery = string.Empty;

    public AppointmentsPageViewModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
        Title = "My Appointments";
    }
    
    partial void OnSearchQueryChanged(string value)
    {
        FilterAppointments(value);
    }

    [RelayCommand]
    private async Task LoadAppointmentsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Load today's appointments
            var today = await _appointmentService.GetTodayAppointments();
            _allTodayAppointments = today;
            TodayAppointments = new ObservableCollection<Appointment>(today);
            HasTodayAppointments = today.Any();

            // Load unresolved (past incomplete)
            var past = await _appointmentService.GetPastAppointments();
            var unresolved = past.Where(a => a.Status != AppointmentStatus.Completed).ToList();
            _allUnresolvedAppointments = unresolved;
            UnresolvedAppointments = new ObservableCollection<Appointment>(unresolved);
            HasUnresolvedAppointments = unresolved.Any();

            // Load future
            var future = await _appointmentService.GetFutureAppointments();
            _allFutureAppointments = future;
            FutureAppointments = new ObservableCollection<Appointment>(future);
            HasFutureAppointments = future.Any();
        });
    }
    
    [RelayCommand]
    private void ToggleSearch()
    {
        IsSearchVisible = !IsSearchVisible;
        if (!IsSearchVisible)
        {
            SearchQuery = string.Empty;
        }
    }
    
    [RelayCommand]
    private void ClearSearch()
    {
        SearchQuery = string.Empty;
        IsSearchVisible = false;
    }
    
    [RelayCommand]
    private void Search()
    {
        FilterAppointments(SearchQuery);
    }
    
    private void FilterAppointments(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            // Reset to original lists
            TodayAppointments = new ObservableCollection<Appointment>(_allTodayAppointments);
            UnresolvedAppointments = new ObservableCollection<Appointment>(_allUnresolvedAppointments);
            FutureAppointments = new ObservableCollection<Appointment>(_allFutureAppointments);
        }
        else
        {
            var lowerQuery = query.ToLowerInvariant();
            
            TodayAppointments = new ObservableCollection<Appointment>(
                _allTodayAppointments.Where(a => MatchesSearch(a, lowerQuery)));
            
            UnresolvedAppointments = new ObservableCollection<Appointment>(
                _allUnresolvedAppointments.Where(a => MatchesSearch(a, lowerQuery)));
            
            FutureAppointments = new ObservableCollection<Appointment>(
                _allFutureAppointments.Where(a => MatchesSearch(a, lowerQuery)));
        }
        
        HasTodayAppointments = TodayAppointments.Any();
        HasUnresolvedAppointments = UnresolvedAppointments.Any();
        HasFutureAppointments = FutureAppointments.Any();
    }
    
    private static bool MatchesSearch(Appointment appointment, string query)
    {
        return appointment.CustomerName.ToLowerInvariant().Contains(query) ||
               appointment.SiteName.ToLowerInvariant().Contains(query) ||
               appointment.JobNumber.ToLowerInvariant().Contains(query) ||
               appointment.Location.Address.ToLowerInvariant().Contains(query) ||
               appointment.Location.City.ToLowerInvariant().Contains(query) ||
               appointment.ServiceJobType.ToLowerInvariant().Contains(query);
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
