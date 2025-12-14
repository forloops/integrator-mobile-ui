namespace IntegratorMobile.Views.Jobs;

public partial class OperationJobsPage : ContentPage
{
    private Button? _activeFilter;

    public OperationJobsPage()
    {
        InitializeComponent();
        _activeFilter = AllFilter;
    }

    private void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is not Button clickedButton) return;

        // Reset all filters to ghost style
        ResetFilterStyle(AllFilter);
        ResetFilterStyle(PendingFilter);
        ResetFilterStyle(InProgressFilter);
        ResetFilterStyle(CompletedFilter);

        // Apply active style to clicked filter
        clickedButton.BackgroundColor = Color.FromArgb("#4338CA");
        clickedButton.TextColor = Colors.White;

        _activeFilter = clickedButton;

        // Update job count based on filter (mock data)
        if (clickedButton == AllFilter)
            JobCountLabel.Text = "4 jobs";
        else if (clickedButton == PendingFilter)
            JobCountLabel.Text = "2 jobs";
        else if (clickedButton == InProgressFilter)
            JobCountLabel.Text = "1 job";
        else if (clickedButton == CompletedFilter)
            JobCountLabel.Text = "1 job";
    }

    private void ResetFilterStyle(Button button)
    {
        button.BackgroundColor = Color.FromArgb("#F1F5F9");
        button.TextColor = Color.FromArgb("#475569");
    }
}
