namespace IntegratorMobile.MockData.Services;

/// <summary>
/// Abstraction for navigation operations, enabling ViewModel testability.
/// In production, this is implemented by ShellNavigationService which wraps Shell.Current.
/// In tests, this can be mocked to verify navigation behavior.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigate to a route using Shell navigation.
    /// </summary>
    /// <param name="route">The route to navigate to (e.g., "login", "//home", "detail?id=123")</param>
    Task GoToAsync(string route);

    /// <summary>
    /// Navigate to a route with parameters.
    /// </summary>
    /// <param name="route">The route to navigate to</param>
    /// <param name="parameters">Dictionary of parameters to pass</param>
    Task GoToAsync(string route, IDictionary<string, object> parameters);

    /// <summary>
    /// Navigate back (pop from navigation stack).
    /// </summary>
    Task GoBackAsync();

    /// <summary>
    /// Display an alert dialog with accept/cancel options.
    /// </summary>
    /// <param name="title">Alert title</param>
    /// <param name="message">Alert message</param>
    /// <param name="accept">Accept button text</param>
    /// <param name="cancel">Cancel button text</param>
    /// <returns>True if accept was pressed, false if cancel</returns>
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

    /// <summary>
    /// Display an alert dialog with OK button only.
    /// </summary>
    /// <param name="title">Alert title</param>
    /// <param name="message">Alert message</param>
    /// <param name="ok">OK button text</param>
    Task DisplayAlertAsync(string title, string message, string ok);

    /// <summary>
    /// Display an action sheet with multiple options.
    /// </summary>
    /// <param name="title">Sheet title</param>
    /// <param name="cancel">Cancel button text</param>
    /// <param name="destruction">Destructive action text (optional)</param>
    /// <param name="buttons">Array of button options</param>
    /// <returns>The selected button text</returns>
    Task<string?> DisplayActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons);

    /// <summary>
    /// Open/close the flyout menu.
    /// </summary>
    bool FlyoutIsPresented { get; set; }
}
