namespace IntegratorMobile.UI.Controls;

public partial class AppBadge : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AppBadge), string.Empty);

    public static readonly BindableProperty BadgeTypeProperty =
        BindableProperty.Create(nameof(BadgeType), typeof(BadgeType), typeof(AppBadge), BadgeType.Default,
            propertyChanged: OnBadgeTypeChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public BadgeType BadgeType
    {
        get => (BadgeType)GetValue(BadgeTypeProperty);
        set => SetValue(BadgeTypeProperty, value);
    }

    public AppBadge()
    {
        InitializeComponent();
        ApplyBadgeStyle();
    }

    private static void OnBadgeTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AppBadge badge)
        {
            badge.ApplyBadgeStyle();
        }
    }

    private void ApplyBadgeStyle()
    {
        switch (BadgeType)
        {
            case BadgeType.Success:
                BadgeBorder.BackgroundColor = Color.FromArgb("#DCFCE7");
                BadgeLabel.TextColor = Color.FromArgb("#166534");
                break;
            case BadgeType.Warning:
                BadgeBorder.BackgroundColor = Color.FromArgb("#FEF3C7");
                BadgeLabel.TextColor = Color.FromArgb("#92400E");
                break;
            case BadgeType.Error:
                BadgeBorder.BackgroundColor = Color.FromArgb("#FEE2E2");
                BadgeLabel.TextColor = Color.FromArgb("#991B1B");
                break;
            case BadgeType.Info:
                BadgeBorder.BackgroundColor = Color.FromArgb("#E0F2FE");
                BadgeLabel.TextColor = Color.FromArgb("#075985");
                break;
            case BadgeType.Ready:
                BadgeBorder.BackgroundColor = Color.FromArgb("#DCFCE7");
                BadgeLabel.TextColor = Color.FromArgb("#166534");
                break;
            case BadgeType.InProgress:
                BadgeBorder.BackgroundColor = Color.FromArgb("#E0F2FE");
                BadgeLabel.TextColor = Color.FromArgb("#075985");
                break;
            case BadgeType.Completed:
                BadgeBorder.BackgroundColor = Color.FromArgb("#DCFCE7");
                BadgeLabel.TextColor = Color.FromArgb("#166534");
                break;
            case BadgeType.NeedToReturn:
                BadgeBorder.BackgroundColor = Color.FromArgb("#FEF3C7");
                BadgeLabel.TextColor = Color.FromArgb("#92400E");
                break;
            case BadgeType.Scheduled:
                BadgeBorder.BackgroundColor = Color.FromArgb("#F1F5F9");
                BadgeLabel.TextColor = Color.FromArgb("#475569");
                break;
            case BadgeType.EnRoute:
                BadgeBorder.BackgroundColor = Color.FromArgb("#E0F2FE");
                BadgeLabel.TextColor = Color.FromArgb("#075985");
                break;
            default:
                BadgeBorder.BackgroundColor = Color.FromArgb("#F1F5F9");
                BadgeLabel.TextColor = Color.FromArgb("#475569");
                break;
        }
    }
}

public enum BadgeType
{
    Default,
    Success,
    Warning,
    Error,
    Info,
    // Status-specific badges
    Ready,
    InProgress,
    Completed,
    NeedToReturn,
    Scheduled,
    EnRoute
}
