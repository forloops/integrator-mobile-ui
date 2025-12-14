using IntegratorMobile.Views.Auth;
using IntegratorMobile.Views.Home;
using IntegratorMobile.Views.Appointments;
using IntegratorMobile.Views.Profile;
using IntegratorMobile.Views.Settings;
using IntegratorMobile.Views.Jobs;
using IntegratorMobile.Views.Directory;
using IntegratorMobile.Views.Diagnostics;
using IntegratorMobile.Views.Help;

namespace IntegratorMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for navigation
        // Auth routes
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("login/manual", typeof(ManualLoginPage));
        Routing.RegisterRoute("login-help", typeof(LoginHelpPage));
        Routing.RegisterRoute("forgot-password", typeof(ForgotPasswordPage));
        Routing.RegisterRoute("reset-password", typeof(ResetPasswordPage));
        Routing.RegisterRoute("get-username", typeof(GetUsernamePage));
        Routing.RegisterRoute("setup-account", typeof(SetupAccountPage));
        Routing.RegisterRoute("update-password", typeof(UpdatePasswordPage));
        
        // Main app routes
        Routing.RegisterRoute("home", typeof(HomePage));
        Routing.RegisterRoute("appointments", typeof(AppointmentsPage));
        Routing.RegisterRoute("appointments/detail", typeof(AppointmentDetailPage));
        Routing.RegisterRoute("appointments/drive", typeof(DriveToAppointmentPage));
        Routing.RegisterRoute("appointments/arrival", typeof(ArrivalPhotosPage));
        Routing.RegisterRoute("appointments/complete", typeof(CompleteAppointmentPage));
        Routing.RegisterRoute("appointments/system", typeof(SystemDetailPage));
        Routing.RegisterRoute("appointments/workitem", typeof(WorkItemDetailPage));
        Routing.RegisterRoute("appointments/add-workitem", typeof(AddWorkItemPage));
        Routing.RegisterRoute("appointments/add-milestone", typeof(AddMilestonePage));
        
        // Jobs routes
        Routing.RegisterRoute("jobs", typeof(OperationJobsPage));
        
        // Directory routes
        Routing.RegisterRoute("directory", typeof(EmployeeDirectoryPage));
        
        // Diagnostics routes
        Routing.RegisterRoute("diagnostics", typeof(DiagnosticsPage));
        
        // Help routes
        Routing.RegisterRoute("help", typeof(HelpPage));
        
        // Settings routes
        Routing.RegisterRoute("profile", typeof(ProfilePage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
    }

    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        // Close the flyout
        FlyoutIsPresented = false;
        
        // Navigate back to identify page
        await GoToAsync("//identify");
    }
}
