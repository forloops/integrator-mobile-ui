using System.Windows.Input;

namespace IntegratorMobile.UI.Controls;

public partial class AppButton : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AppButton), string.Empty);

    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(ButtonVariant), typeof(AppButton), ButtonVariant.Primary,
            propertyChanged: OnVariantChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AppButton));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(AppButton));

    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(AppButton), false,
            propertyChanged: OnIsLoadingChanged);

    public static readonly BindableProperty IsBlockProperty =
        BindableProperty.Create(nameof(IsBlock), typeof(bool), typeof(AppButton), false);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ButtonVariant Variant
    {
        get => (ButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public bool IsBlock
    {
        get => (bool)GetValue(IsBlockProperty);
        set => SetValue(IsBlockProperty, value);
    }

    public event EventHandler? Clicked;

    public AppButton()
    {
        InitializeComponent();
        ApplyVariantStyle();
    }

    private static void OnVariantChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AppButton button)
        {
            button.ApplyVariantStyle();
        }
    }

    private static void OnIsLoadingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AppButton button)
        {
            button.InnerButton.IsEnabled = !(bool)newValue && button.IsEnabled;
            button.InnerButton.Text = (bool)newValue ? "Loading..." : button.Text;
        }
    }

    private void ApplyVariantStyle()
    {
        var button = InnerButton;
        
        // Base styles
        button.FontFamily = "InterSemiBold";
        button.FontSize = 16;
        button.CornerRadius = 8;
        button.Padding = new Thickness(20, 14);
        button.MinimumHeightRequest = 44;

        switch (Variant)
        {
            case ButtonVariant.Primary:
                button.BackgroundColor = Color.FromArgb("#4338CA");
                button.TextColor = Colors.White;
                break;
            case ButtonVariant.Secondary:
                button.BackgroundColor = Color.FromArgb("#323F4B");
                button.TextColor = Colors.White;
                break;
            case ButtonVariant.Outline:
                button.BackgroundColor = Colors.Transparent;
                button.TextColor = Color.FromArgb("#4338CA");
                button.BorderColor = Color.FromArgb("#4338CA");
                button.BorderWidth = 2;
                break;
            case ButtonVariant.Ghost:
                button.BackgroundColor = Colors.Transparent;
                button.TextColor = Color.FromArgb("#323F4B");
                break;
            case ButtonVariant.Danger:
                button.BackgroundColor = Color.FromArgb("#DC2626");
                button.TextColor = Colors.White;
                break;
            case ButtonVariant.Success:
                button.BackgroundColor = Color.FromArgb("#16A34A");
                button.TextColor = Colors.White;
                break;
            case ButtonVariant.Microsoft:
                button.BackgroundColor = Colors.White;
                button.TextColor = Color.FromArgb("#323F4B");
                button.BorderColor = Color.FromArgb("#CBD5E1");
                button.BorderWidth = 1;
                break;
            case ButtonVariant.Link:
                button.BackgroundColor = Colors.Transparent;
                button.TextColor = Color.FromArgb("#4338CA");
                button.Padding = new Thickness(4, 2);
                break;
        }
    }

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        Clicked?.Invoke(this, e);
    }
}

public enum ButtonVariant
{
    Primary,
    Secondary,
    Outline,
    Ghost,
    Danger,
    Success,
    Microsoft,
    Link
}
