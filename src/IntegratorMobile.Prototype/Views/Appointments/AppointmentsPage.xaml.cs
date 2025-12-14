using IntegratorMobile.ViewModels;

namespace IntegratorMobile.Views.Appointments;

public partial class AppointmentsPage : ContentPage
{
    private AppointmentsPageViewModel? _viewModel;

    public AppointmentsPage()
    {
        InitializeComponent();
        _viewModel = App.Current?.Handler?.MauiContext?.Services.GetService<AppointmentsPageViewModel>();
        if (_viewModel != null)
            BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
            await _viewModel.LoadAppointmentsCommand.ExecuteAsync(null);
    }
}
