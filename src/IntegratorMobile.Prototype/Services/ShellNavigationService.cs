using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.Services;

/// <summary>
/// Production implementation of INavigationService using MAUI Shell.
/// </summary>
public class ShellNavigationService : INavigationService
{
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync(string route, IDictionary<string, object> parameters)
    {
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public async Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

    public async Task DisplayAlertAsync(string title, string message, string ok)
    {
        await Shell.Current.DisplayAlert(title, message, ok);
    }

    public async Task<string?> DisplayActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
    {
        return await Shell.Current.DisplayActionSheet(title, cancel, destruction, buttons);
    }

    public bool FlyoutIsPresented
    {
        get => Shell.Current.FlyoutIsPresented;
        set => Shell.Current.FlyoutIsPresented = value;
    }
}
