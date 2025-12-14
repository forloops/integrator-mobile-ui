using System.Windows.Input;

namespace IntegratorMobile.UI.Controls;

public partial class PunchListItem : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(PunchListItem), string.Empty);

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(PunchListItem), string.Empty);

    public static readonly BindableProperty StepNumberProperty =
        BindableProperty.Create(nameof(StepNumber), typeof(int), typeof(PunchListItem), 1);

    public static readonly BindableProperty StatusProperty =
        BindableProperty.Create(nameof(Status), typeof(PunchListStatus), typeof(PunchListItem), PunchListStatus.Locked,
            propertyChanged: OnStatusChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(PunchListItem));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(PunchListItem));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public int StepNumber
    {
        get => (int)GetValue(StepNumberProperty);
        set => SetValue(StepNumberProperty, value);
    }

    public PunchListStatus Status
    {
        get => (PunchListStatus)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
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

    public event EventHandler? Tapped;

    public PunchListItem()
    {
        InitializeComponent();
        ApplyStatusStyle();
    }

    private static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PunchListItem item)
        {
            item.ApplyStatusStyle();
        }
    }

    private void ApplyStatusStyle()
    {
        switch (Status)
        {
            case PunchListStatus.Locked:
                StatusIcon.BackgroundColor = Color.FromArgb("#F1F5F9");
                StatusIcon.Stroke = Color.FromArgb("#CBD5E1");
                IconLabel.Text = StepNumber.ToString();
                IconLabel.TextColor = Color.FromArgb("#94A3B8");
                TitleLabel.TextColor = Color.FromArgb("#94A3B8");
                DescriptionLabel.TextColor = Color.FromArgb("#CBD5E1");
                break;
                
            case PunchListStatus.Available:
                StatusIcon.BackgroundColor = Colors.White;
                StatusIcon.Stroke = Color.FromArgb("#4338CA");
                IconLabel.Text = StepNumber.ToString();
                IconLabel.TextColor = Color.FromArgb("#4338CA");
                TitleLabel.TextColor = Color.FromArgb("#0F172A");
                DescriptionLabel.TextColor = Color.FromArgb("#64748B");
                break;
                
            case PunchListStatus.InProgress:
                StatusIcon.BackgroundColor = Color.FromArgb("#4338CA");
                StatusIcon.Stroke = Color.FromArgb("#4338CA");
                IconLabel.Text = "⚡";
                IconLabel.TextColor = Colors.White;
                TitleLabel.TextColor = Color.FromArgb("#0F172A");
                DescriptionLabel.TextColor = Color.FromArgb("#64748B");
                break;
                
            case PunchListStatus.Completed:
                StatusIcon.BackgroundColor = Color.FromArgb("#16A34A");
                StatusIcon.Stroke = Color.FromArgb("#16A34A");
                IconLabel.Text = "✓";
                IconLabel.TextColor = Colors.White;
                TitleLabel.TextColor = Color.FromArgb("#0F172A");
                DescriptionLabel.TextColor = Color.FromArgb("#64748B");
                break;
                
            case PunchListStatus.Warning:
                StatusIcon.BackgroundColor = Color.FromArgb("#D97706");
                StatusIcon.Stroke = Color.FromArgb("#D97706");
                IconLabel.Text = "!";
                IconLabel.TextColor = Colors.White;
                TitleLabel.TextColor = Color.FromArgb("#0F172A");
                DescriptionLabel.TextColor = Color.FromArgb("#64748B");
                break;
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (Status != PunchListStatus.Locked)
        {
            Command?.Execute(CommandParameter);
            Tapped?.Invoke(this, EventArgs.Empty);
        }
    }
}

public enum PunchListStatus
{
    Locked,
    Available,
    InProgress,
    Completed,
    Warning
}
