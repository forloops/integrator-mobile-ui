using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntegratorMobile.MockData.Models;
using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.ViewModels;

[QueryProperty(nameof(AppointmentId), "id")]
public partial class AppointmentDetailPageViewModel : BaseViewModel
{
    private readonly IAppointmentService _appointmentService;

    [ObservableProperty]
    private string _appointmentId = string.Empty;

    [ObservableProperty]
    private Appointment? _appointment;

    [ObservableProperty]
    private ObservableCollection<WorkItem> _workItems = new();

    [ObservableProperty]
    private int _selectedTabIndex = 0;

    // Punch List Step States
    [ObservableProperty]
    private string _driveStepStatus = "Available";

    [ObservableProperty]
    private string _arrivalStepStatus = "Locked";

    [ObservableProperty]
    private string _surveyStepStatus = "Locked";

    [ObservableProperty]
    private string _completeStepStatus = "Locked";

    public AppointmentDetailPageViewModel(IAppointmentService appointmentService, INavigationService navigationService)
        : base(navigationService)
    {
        _appointmentService = appointmentService;
        Title = "Appointment Details";
    }

    partial void OnAppointmentIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            LoadAppointmentCommand.Execute(null);
        }
    }

    [RelayCommand]
    private async Task LoadAppointmentAsync()
    {
        await ExecuteAsync(async () =>
        {
            Appointment = await _appointmentService.GetAppointmentById(AppointmentId);
            
            if (Appointment != null)
            {
                Title = Appointment.CustomerName;
                
                var items = await _appointmentService.GetWorkItemsForAppointment(AppointmentId);
                WorkItems = new ObservableCollection<WorkItem>(items);

                // Update punch list step states
                UpdatePunchListStates();
            }
        });
    }

    private void UpdatePunchListStates()
    {
        if (Appointment == null) return;

        DriveStepStatus = Appointment.PunchListProgress.DriveToAppointment.ToString();
        ArrivalStepStatus = Appointment.PunchListProgress.AppointmentArrival.ToString();
        SurveyStepStatus = Appointment.PunchListProgress.SurveyBuildingsSystems.ToString();
        CompleteStepStatus = Appointment.PunchListProgress.CompleteAppointment.ToString();
    }

    [RelayCommand]
    private async Task BeginAppointmentAsync()
    {
        var result = await NavigationService!.DisplayAlertAsync(
            "Begin Appointment?",
            "Enable En Route Time to track your travel time to the job site.",
            "CONFIRM",
            "SKIP AND GO TO APPOINTMENT");

        if (Appointment != null)
        {
            if (result)
            {
                // User wants En Route tracking
                await _appointmentService.UpdateAppointmentStatus(AppointmentId, AppointmentStatus.EnRoute);
                await NavigationService.GoToAsync($"appointments/drive?id={AppointmentId}");
            }
            else
            {
                // Skip to arrival
                await _appointmentService.UpdateAppointmentStatus(AppointmentId, AppointmentStatus.OnSite);
                await NavigationService.GoToAsync($"appointments/arrival?id={AppointmentId}");
            }
        }
    }

    [RelayCommand]
    private async Task GoToDriveAsync()
    {
        await NavigationService!.GoToAsync($"appointments/drive?id={AppointmentId}");
    }

    [RelayCommand]
    private async Task GoToArrivalAsync()
    {
        await NavigationService!.GoToAsync($"appointments/arrival?id={AppointmentId}");
    }

    [RelayCommand]
    private async Task GoToSurveyAsync()
    {
        // Navigate to system/building list
        await NavigationService!.GoToAsync($"appointments/system?id={AppointmentId}");
    }

    [RelayCommand]
    private async Task GoToCompleteAsync()
    {
        await NavigationService!.GoToAsync($"appointments/complete?id={AppointmentId}");
    }

    [RelayCommand]
    private async Task SelectWorkItemAsync(WorkItem workItem)
    {
        if (workItem == null) return;
        await NavigationService!.GoToAsync($"appointments/workitem?id={workItem.Id}");
    }

    [RelayCommand]
    private async Task CancelOrRescheduleAsync()
    {
        var action = await NavigationService!.DisplayActionSheetAsync(
            "Cancel or Reschedule",
            "Cancel",
            null,
            "Reschedule Appointment",
            "Cancel Appointment");

        if (action == "Reschedule Appointment")
        {
            await NavigationService.DisplayAlertAsync("Reschedule", "Reschedule functionality coming soon!", "OK");
        }
        else if (action == "Cancel Appointment")
        {
            var confirm = await NavigationService.DisplayAlertAsync(
                "Cancel Appointment",
                "Are you sure you want to cancel this appointment?",
                "Yes, Cancel",
                "No");

            if (confirm && Appointment != null)
            {
                await _appointmentService.UpdateAppointmentStatus(AppointmentId, AppointmentStatus.Cancelled);
                await NavigationService.GoBackAsync();
            }
        }
    }

    [RelayCommand]
    private async Task NavigateToAddressAsync()
    {
        if (Appointment?.Location == null) return;

        var address = Uri.EscapeDataString(Appointment.Location.FullAddress);
        
        // Try to open in maps
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await Launcher.OpenAsync($"maps://?address={address}");
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Launcher.OpenAsync($"geo:0,0?q={address}");
            }
        }
        catch
        {
            await NavigationService!.DisplayAlertAsync("Navigation", 
                $"Navigate to:\n{Appointment.Location.FullAddress}", "OK");
        }
    }

    [RelayCommand]
    private async Task CallContactAsync(string phone)
    {
        if (string.IsNullOrEmpty(phone)) return;

        try
        {
            PhoneDialer.Open(phone);
        }
        catch
        {
            await NavigationService!.DisplayAlertAsync("Call", $"Call {phone}", "OK");
        }
    }

    [RelayCommand]
    private void SetTab(string tabIndex)
    {
        if (int.TryParse(tabIndex, out int index))
        {
            SelectedTabIndex = index;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await NavigationService!.GoBackAsync();
    }
}
