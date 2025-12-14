using IntegratorMobile.MockData.Services;
using IntegratorMobile.ViewModels;
using IntegratorMobile.Views.Auth;
using IntegratorMobile.Views.Home;
using IntegratorMobile.Views.Appointments;
using IntegratorMobile.Views.Profile;
using IntegratorMobile.Views.Settings;
using Microsoft.Extensions.Logging;

namespace IntegratorMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
            });

        // Register services
        builder.Services.AddSingleton<IAuthService, MockAuthService>();
        builder.Services.AddSingleton<IAppointmentService, MockAppointmentService>();

        // Register ViewModels
        builder.Services.AddTransient<IdentifyPageViewModel>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<ManualLoginPageViewModel>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddTransient<AppointmentsPageViewModel>();
        builder.Services.AddTransient<AppointmentDetailPageViewModel>();

        // Register Pages - Auth
        builder.Services.AddTransient<IdentifyPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ManualLoginPage>();
        
        // Register Pages - Home
        builder.Services.AddTransient<HomePage>();
        
        // Register Pages - Appointments
        builder.Services.AddTransient<AppointmentsPage>();
        builder.Services.AddTransient<AppointmentDetailPage>();
        builder.Services.AddTransient<DriveToAppointmentPage>();
        builder.Services.AddTransient<ArrivalPhotosPage>();
        builder.Services.AddTransient<SystemDetailPage>();
        builder.Services.AddTransient<WorkItemDetailPage>();
        builder.Services.AddTransient<CompleteAppointmentPage>();
        
        // Register Pages - Settings & Profile
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<SettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
