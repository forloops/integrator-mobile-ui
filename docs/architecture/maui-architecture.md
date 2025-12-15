# MAUI Architecture - Integrator Mobile

## Overview

The Integrator Mobile MAUI application follows a clean architecture pattern with clear separation of concerns between UI, business logic, and data access layers.

## Solution Structure

```
IntegratorMobile.sln
│
├── src/
│   ├── IntegratorMobile.Prototype/     # Main MAUI Application
│   │   ├── App.xaml                    # Application entry
│   │   ├── AppShell.xaml               # Shell navigation
│   │   ├── MauiProgram.cs              # DI configuration
│   │   ├── Views/                      # XAML pages
│   │   ├── ViewModels/                 # MVVM ViewModels
│   │   ├── Converters/                 # Value converters
│   │   ├── Resources/                  # Styles, fonts, images
│   │   └── Platforms/                  # Platform-specific code
│   │
│   ├── IntegratorMobile.UI/            # Shared UI Library
│   │   ├── Controls/                   # Reusable controls
│   │   └── Converters/                 # Shared converters
│   │
│   └── IntegratorMobile.MockData/      # Mock Data Layer
│       ├── Models/                     # Data models
│       └── Services/                   # Mock services
│
└── tests/
    └── IntegratorMobile.Tests/         # Unit Tests
```

## Layer Responsibilities

### Presentation Layer (IntegratorMobile.Prototype)

**Views/** - XAML pages defining UI layout
- Organized by feature area (Auth, Appointments, Home, etc.)
- Use data binding to ViewModels
- No business logic - pure UI

**ViewModels/** - MVVM ViewModels
- Implement `INotifyPropertyChanged` via CommunityToolkit.Mvvm
- Expose properties and commands to views
- Coordinate between views and services
- Handle navigation logic

**Converters/** - Value converters for data binding
- `TabSelectedConverter` - Tab selection styling
- `IntEqualConverter` - Integer comparisons
- `InverseBoolConverter` - Boolean negation

### UI Library (IntegratorMobile.UI)

Shareable component library designed for reuse in production app.

**Controls/**
- `AppButton` - Multi-variant button
- `AppCard` - Elevated card container
- `AppInput` - Labeled text input
- `AppBadge` - Status badges
- `PunchListItem` - Workflow step display

### Data Layer (IntegratorMobile.MockData)

**Models/** - Data transfer objects
- `Appointment`, `WorkItem`, `Building`, `User`
- Enums for status values
- Display formatters

**Services/** - Data access
- `IAuthService` / `MockAuthService`
- `IAppointmentService` / `MockAppointmentService`
- Interface-based for future API integration

## MVVM Pattern

### ViewModel Base Class

```csharp
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;
}
```

### Command Implementation

Using CommunityToolkit.Mvvm source generators:

```csharp
public partial class LoginPageViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _username = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        IsBusy = true;
        try
        {
            var user = await _authService.LoginWithCredentials(Username, Password);
            if (user != null)
                await Shell.Current.GoToAsync("//home");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

## Navigation

### Shell-Based Navigation

The app uses .NET MAUI Shell for navigation, providing:
- URI-based routing
- Flyout menu
- Tab bars (where needed)
- Navigation stack management

**Route Registration (AppShell.xaml):**

```xml
<Shell>
    <!-- Auth Flow -->
    <ShellContent Route="identify" ContentTemplate="{DataTemplate auth:IdentifyPage}" />
    
    <!-- Main Navigation -->
    <FlyoutItem Title="Home">
        <ShellContent Route="home" ContentTemplate="{DataTemplate home:HomePage}" />
    </FlyoutItem>
    
    <FlyoutItem Title="Appointments">
        <ShellContent Route="appointments" ContentTemplate="{DataTemplate appt:AppointmentsPage}" />
    </FlyoutItem>
</Shell>
```

**Programmatic Navigation:**

```csharp
// Navigate to route
await Shell.Current.GoToAsync("//appointments");

// Navigate with parameters
await Shell.Current.GoToAsync($"appointmentDetail?id={appointmentId}");

// Go back
await Shell.Current.GoToAsync("..");
```

### Query Parameters

```csharp
[QueryProperty(nameof(AppointmentId), "id")]
public partial class AppointmentDetailPageViewModel : BaseViewModel
{
    private string _appointmentId;
    public string AppointmentId
    {
        get => _appointmentId;
        set
        {
            _appointmentId = value;
            LoadAppointment(value);
        }
    }
}
```

## Dependency Injection

### Service Registration (MauiProgram.cs)

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
    // Services
    builder.Services.AddSingleton<IAuthService, MockAuthService>();
    builder.Services.AddSingleton<IAppointmentService, MockAppointmentService>();
    
    // ViewModels
    builder.Services.AddTransient<LoginPageViewModel>();
    builder.Services.AddTransient<AppointmentsPageViewModel>();
    
    // Pages
    builder.Services.AddTransient<LoginPage>();
    builder.Services.AddTransient<AppointmentsPage>();
    
    return builder.Build();
}
```

### Constructor Injection

```csharp
public partial class AppointmentsPage : ContentPage
{
    public AppointmentsPage(AppointmentsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

## Data Binding

### Property Binding

```xml
<Label Text="{Binding CustomerName}" />
<Entry Text="{Binding SearchQuery, Mode=TwoWay}" />
<ActivityIndicator IsRunning="{Binding IsBusy}" />
```

### Command Binding

```xml
<Button Text="Login" Command="{Binding LoginCommand}" />
<Button Command="{Binding SelectCommand}" CommandParameter="{Binding .}" />
```

### Collection Binding

```xml
<CollectionView ItemsSource="{Binding Appointments}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="models:Appointment">
            <Label Text="{Binding CustomerName}" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

## Resource Management

### Static Resources

**Colors.xaml:**
```xml
<ResourceDictionary>
    <Color x:Key="Primary600">#4338CA</Color>
    <Color x:Key="Slate700">#323F4B</Color>
</ResourceDictionary>
```

**Usage:**
```xml
<Label TextColor="{StaticResource Primary600}" />
```

### Styles

```xml
<Style x:Key="HeadingMedium" TargetType="Label">
    <Setter Property="FontFamily" Value="InterSemiBold" />
    <Setter Property="FontSize" Value="20" />
    <Setter Property="TextColor" Value="{StaticResource TextPrimary}" />
</Style>
```

## Platform Specifics

### Platforms Folder Structure

```
Platforms/
├── Android/
│   ├── AndroidManifest.xml
│   ├── MainActivity.cs
│   └── MainApplication.cs
├── iOS/
│   ├── AppDelegate.cs
│   ├── Info.plist
│   └── Program.cs
└── MacCatalyst/
    ├── AppDelegate.cs
    ├── Info.plist
    └── Program.cs
```

### Platform-Specific Code

Using conditional compilation:

```csharp
#if IOS
    // iOS-specific code
#elif ANDROID
    // Android-specific code
#endif
```

Or platform handlers:

```csharp
Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
{
#if ANDROID
    handler.PlatformView.Background = null;
#endif
});
```

## Future Architecture (Production)

### API Integration Layer

```
IntegratorMobile.ApiClient/
├── Clients/
│   ├── AuthClient.cs
│   └── AppointmentClient.cs
├── Models/
│   ├── ApiResponse.cs
│   └── ApiError.cs
└── Configuration/
    └── ApiConfiguration.cs
```

### Offline Support Layer

```
IntegratorMobile.Data/
├── Database/
│   ├── LocalDatabase.cs
│   └── Migrations/
├── Repositories/
│   ├── AppointmentRepository.cs
│   └── SyncRepository.cs
└── Sync/
    ├── SyncManager.cs
    └── ConflictResolver.cs
```

### Recommended Package Additions

| Package | Purpose |
|---------|---------|
| SQLite-net | Local database |
| Refit | API client generation |
| Polly | Retry policies |
| Serilog | Structured logging |
| AppCenter | Analytics & crashes |

## Testing Strategy

### Unit Tests

```csharp
public class MockAuthServiceTests
{
    [Fact]
    public async Task ValidateCompany_ValidIdentifier_ReturnsCompany()
    {
        var service = new MockAuthService();
        var result = await service.ValidateCompanyIdentifier("crowther");
        Assert.NotNull(result);
        Assert.Equal("Crowther Roofing", result.Name);
    }
}
```

### ViewModel Tests

```csharp
public class LoginPageViewModelTests
{
    [Fact]
    public async Task Login_ValidCredentials_NavigatesToHome()
    {
        var mockAuth = new Mock<IAuthService>();
        mockAuth.Setup(x => x.LoginWithCredentials(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new User { Username = "test" });
        
        var vm = new LoginPageViewModel(mockAuth.Object);
        vm.Username = "test";
        vm.Password = "pass";
        
        await vm.LoginCommand.ExecuteAsync(null);
        
        // Assert navigation occurred
    }
}
```

## Build Configuration

### Directory.Build.props

```xml
<Project>
    <PropertyGroup>
        <TargetFrameworks>net10.0-ios;net10.0-android;net10.0-maccatalyst</TargetFrameworks>
        <UseMaui>true</UseMaui>
        <ValidateXcodeVersion>false</ValidateXcodeVersion>
    </PropertyGroup>
</Project>
```

### Project References

```xml
<!-- IntegratorMobile.Prototype.csproj -->
<ProjectReference Include="..\IntegratorMobile.UI\IntegratorMobile.UI.csproj" />
<ProjectReference Include="..\IntegratorMobile.MockData\IntegratorMobile.MockData.csproj" />
```
