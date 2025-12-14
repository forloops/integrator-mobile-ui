namespace IntegratorMobile.UI.Controls;

public partial class AppInput : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AppInput), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(nameof(Label), typeof(string), typeof(AppInput), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AppInput), string.Empty);

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(AppInput), false);

    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(AppInput), Keyboard.Default);

    public static readonly BindableProperty ReturnTypeProperty =
        BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(AppInput), ReturnType.Default);

    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(AppInput), int.MaxValue);

    public static readonly BindableProperty ErrorMessageProperty =
        BindableProperty.Create(nameof(ErrorMessage), typeof(string), typeof(AppInput), string.Empty,
            propertyChanged: OnErrorMessageChanged);

    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.Create(nameof(HasError), typeof(bool), typeof(AppInput), false,
            propertyChanged: OnHasErrorChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    public event EventHandler<FocusEventArgs>? InputFocused;
    public event EventHandler<FocusEventArgs>? InputUnfocused;
    public event EventHandler? Completed;

    public AppInput()
    {
        InitializeComponent();
    }

    private static void OnErrorMessageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AppInput input)
        {
            input.HasError = !string.IsNullOrEmpty((string)newValue);
        }
    }

    private static void OnHasErrorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AppInput input)
        {
            input.InputBorder.Stroke = (bool)newValue 
                ? Color.FromArgb("#DC2626") 
                : Color.FromArgb("#CBD5E1");
        }
    }

    private void OnEntryFocused(object? sender, FocusEventArgs e)
    {
        InputBorder.Stroke = HasError 
            ? Color.FromArgb("#DC2626") 
            : Color.FromArgb("#4338CA");
        InputBorder.StrokeThickness = 2;
        InputFocused?.Invoke(this, e);
    }

    private void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        InputBorder.Stroke = HasError 
            ? Color.FromArgb("#DC2626") 
            : Color.FromArgb("#CBD5E1");
        InputBorder.StrokeThickness = 1;
        InputUnfocused?.Invoke(this, e);
    }

    private void OnEntryCompleted(object? sender, EventArgs e)
    {
        Completed?.Invoke(this, e);
    }

    public new void Focus()
    {
        InputEntry.Focus();
    }
}
