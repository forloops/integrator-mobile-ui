using IntegratorMobile.MockData.Services;

namespace IntegratorMobile.Tests.Mocks;

/// <summary>
/// Mock implementation of INavigationService for unit testing ViewModels.
/// Records navigation calls for verification in tests.
/// </summary>
public class MockNavigationService : INavigationService
{
    // Track navigation history
    public List<string> NavigationHistory { get; } = new();
    public List<(string Route, IDictionary<string, object> Parameters)> NavigationWithParamsHistory { get; } = new();
    public int GoBackCallCount { get; private set; }
    
    // Track alert history
    public List<(string Title, string Message, string Accept, string Cancel)> AlertHistory { get; } = new();
    public List<(string Title, string Message, string Ok)> SimpleAlertHistory { get; } = new();
    public List<(string Title, string Cancel, string? Destruction, string[] Buttons)> ActionSheetHistory { get; } = new();
    
    // Configurable responses
    public bool AlertResponse { get; set; } = true;
    public string? ActionSheetResponse { get; set; }
    
    // Flyout state
    private bool _flyoutIsPresented;
    public bool FlyoutIsPresented
    {
        get => _flyoutIsPresented;
        set => _flyoutIsPresented = value;
    }

    public Task GoToAsync(string route)
    {
        NavigationHistory.Add(route);
        return Task.CompletedTask;
    }

    public Task GoToAsync(string route, IDictionary<string, object> parameters)
    {
        NavigationHistory.Add(route);
        NavigationWithParamsHistory.Add((route, parameters));
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        GoBackCallCount++;
        NavigationHistory.Add("..");
        return Task.CompletedTask;
    }

    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
    {
        AlertHistory.Add((title, message, accept, cancel));
        return Task.FromResult(AlertResponse);
    }

    public Task DisplayAlertAsync(string title, string message, string ok)
    {
        SimpleAlertHistory.Add((title, message, ok));
        return Task.CompletedTask;
    }

    public Task<string?> DisplayActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
    {
        ActionSheetHistory.Add((title, cancel, destruction, buttons));
        return Task.FromResult(ActionSheetResponse);
    }

    /// <summary>
    /// Reset all recorded history for test isolation.
    /// </summary>
    public void Reset()
    {
        NavigationHistory.Clear();
        NavigationWithParamsHistory.Clear();
        GoBackCallCount = 0;
        AlertHistory.Clear();
        SimpleAlertHistory.Clear();
        ActionSheetHistory.Clear();
        AlertResponse = true;
        ActionSheetResponse = null;
        _flyoutIsPresented = false;
    }

    /// <summary>
    /// Verify the last navigation was to the expected route.
    /// </summary>
    public bool LastNavigationWas(string expectedRoute)
    {
        return NavigationHistory.Count > 0 && NavigationHistory[^1] == expectedRoute;
    }

    /// <summary>
    /// Verify navigation occurred to a specific route (at any point).
    /// </summary>
    public bool NavigatedTo(string route)
    {
        return NavigationHistory.Contains(route);
    }
}
