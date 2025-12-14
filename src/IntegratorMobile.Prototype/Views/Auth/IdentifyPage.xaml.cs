using IntegratorMobile.ViewModels;

namespace IntegratorMobile.Views.Auth;

public partial class IdentifyPage : ContentPage
{
    public IdentifyPage()
    {
        InitializeComponent();
        BindingContext = App.Current?.Handler?.MauiContext?.Services.GetService<IdentifyPageViewModel>()
            ?? new IdentifyPageViewModel(new MockData.Services.MockAuthService());
    }

    private void OnEntryCompleted(object? sender, EventArgs e)
    {
        if (BindingContext is IdentifyPageViewModel vm)
        {
            vm.ContinueCommand.Execute(null);
        }
    }
}
