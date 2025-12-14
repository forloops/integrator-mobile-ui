using IntegratorMobile.ViewModels;

namespace IntegratorMobile.Views.Auth;

public partial class ManualLoginPage : ContentPage
{
    public ManualLoginPage()
    {
        InitializeComponent();
        BindingContext = App.Current?.Handler?.MauiContext?.Services.GetService<ManualLoginPageViewModel>();
    }

    private void OnPasswordCompleted(object? sender, EventArgs e)
    {
        if (BindingContext is ManualLoginPageViewModel vm)
        {
            vm.LoginCommand.Execute(null);
        }
    }
}
