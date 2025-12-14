using CommunityToolkit.Mvvm.ComponentModel;

namespace IntegratorMobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public bool IsNotBusy => !IsBusy;

    protected async Task ExecuteAsync(Func<Task> operation, string? busyMessage = null)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            System.Diagnostics.Debug.WriteLine($"Error: {ex}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        if (IsBusy) return default;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            return await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            return default;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
