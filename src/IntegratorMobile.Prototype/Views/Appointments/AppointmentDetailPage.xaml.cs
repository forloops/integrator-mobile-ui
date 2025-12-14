using IntegratorMobile.ViewModels;

namespace IntegratorMobile.Views.Appointments;

public partial class AppointmentDetailPage : ContentPage
{
    public AppointmentDetailPage()
    {
        InitializeComponent();
        BindingContext = App.Current?.Handler?.MauiContext?.Services.GetService<AppointmentDetailPageViewModel>();
    }
}
