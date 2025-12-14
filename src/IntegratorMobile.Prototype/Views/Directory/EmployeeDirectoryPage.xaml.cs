namespace IntegratorMobile.Views.Directory;

public partial class EmployeeDirectoryPage : ContentPage
{
    public EmployeeDirectoryPage()
    {
        InitializeComponent();
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        // In a production app, this would filter the employee list
        // For the prototype, we just update the count label
        var query = e.NewTextValue?.ToLowerInvariant() ?? "";
        
        if (string.IsNullOrEmpty(query))
        {
            ResultsCount.Text = "5 employees found";
        }
        else
        {
            // Simulate filtering
            ResultsCount.Text = "Searching...";
        }
    }
}
