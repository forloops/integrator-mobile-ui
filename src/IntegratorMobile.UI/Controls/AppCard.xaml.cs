namespace IntegratorMobile.UI.Controls;

public partial class AppCard : ContentView
{
    public static readonly BindableProperty CardPaddingProperty =
        BindableProperty.Create(nameof(CardPadding), typeof(Thickness), typeof(AppCard), new Thickness(16));

    public Thickness CardPadding
    {
        get => (Thickness)GetValue(CardPaddingProperty);
        set => SetValue(CardPaddingProperty, value);
    }

    public AppCard()
    {
        InitializeComponent();
    }
}
