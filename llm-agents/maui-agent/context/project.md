# MAUI Agent - Project Context

## Project Overview

**Project:** Integrator Mobile MAUI Prototype  
**Framework:** .NET MAUI 10.0 (Preview)  
**Target Platforms:** iOS, Android, macOS Catalyst

## Solution Structure

```
IntegratorMobile.sln
├── src/
│   ├── IntegratorMobile.Prototype/     # Main MAUI app
│   │   ├── App.xaml                    # Application resources
│   │   ├── AppShell.xaml               # Navigation structure
│   │   ├── MauiProgram.cs              # DI configuration
│   │   ├── Views/                      # 25 XAML pages
│   │   │   ├── Auth/                   # 9 auth pages
│   │   │   ├── Appointments/           # 9 appointment pages
│   │   │   ├── Home/
│   │   │   ├── Profile/
│   │   │   ├── Settings/
│   │   │   ├── Jobs/
│   │   │   ├── Directory/
│   │   │   ├── Diagnostics/
│   │   │   └── Help/
│   │   ├── ViewModels/                 # 7 ViewModels
│   │   ├── Converters/                 # Value converters
│   │   ├── Resources/                  # Styles, fonts
│   │   └── Platforms/                  # Platform code
│   │
│   ├── IntegratorMobile.UI/            # Component library
│   │   └── Controls/                   # 5 reusable controls
│   │
│   └── IntegratorMobile.MockData/      # Mock data
│       ├── Models/                     # 4 model files
│       └── Services/                   # 4 service files
│
├── tests/
│   └── IntegratorMobile.Tests/         # xUnit tests (100+ tests)
│       ├── Models/                     # Model unit tests
│       ├── Services/                   # Service unit tests
│       ├── Logic/                      # Extracted logic tests
│       ├── Integration/                # Workflow integration tests
│       └── FeatureParity/              # Vue-MAUI parity validation
│
└── docs/
    ├── requirements/                   # Functional requirements
    ├── design-system/                  # Design tokens & components
    ├── progress/                       # Implementation status
    ├── architecture/                   # MAUI architecture
    └── testing/                        # Testing strategy
```

## Key Files

### Entry Points
- `MauiProgram.cs` - Application bootstrap, DI configuration
- `App.xaml` - Application resources
- `AppShell.xaml` - Navigation routes

### Models
- `Appointment.cs` - Appointment, Location, PunchListProgress
- `WorkItem.cs` - WorkItem, Milestone
- `Building.cs` - Building, SystemInfo
- `User.cs` - User, Company

### Services (Interfaces)
- `IAuthService` - Authentication operations
- `IAppointmentService` - Appointment/WorkItem CRUD

### ViewModels
- `BaseViewModel` - IsBusy, Title
- `IdentifyPageViewModel` - Company validation
- `LoginPageViewModel` - Login options
- `ManualLoginPageViewModel` - Credential login
- `HomePageViewModel` - Dashboard
- `AppointmentsPageViewModel` - List management
- `AppointmentDetailPageViewModel` - Detail with tabs

## Dependencies

### NuGet Packages
```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.x" />
<PackageReference Include="CommunityToolkit.Maui" Version="x.x" />
```

### Project References
```xml
<ProjectReference Include="..\IntegratorMobile.UI\IntegratorMobile.UI.csproj" />
<ProjectReference Include="..\IntegratorMobile.MockData\IntegratorMobile.MockData.csproj" />
```

## Architecture Patterns

### MVVM Implementation
```csharp
// ViewModel with CommunityToolkit.Mvvm
public partial class MyViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [RelayCommand]
    private async Task SaveAsync()
    {
        // Implementation
    }
}
```

### Page-ViewModel Binding
```csharp
// Page constructor injection
public MyPage(MyViewModel viewModel)
{
    InitializeComponent();
    BindingContext = viewModel;
}
```

### Service Registration
```csharp
// In MauiProgram.cs
builder.Services.AddSingleton<IAuthService, MockAuthService>();
builder.Services.AddTransient<MyViewModel>();
builder.Services.AddTransient<MyPage>();
```

## Navigation Routes

### Registered Routes
| Route | Page | Type |
|-------|------|------|
| `identify` | IdentifyPage | ShellContent |
| `login` | LoginPage | Stack |
| `manualLogin` | ManualLoginPage | Stack |
| `home` | HomePage | FlyoutItem |
| `appointments` | AppointmentsPage | FlyoutItem |
| `appointmentDetail` | AppointmentDetailPage | Stack |
| `driveToAppointment` | DriveToAppointmentPage | Stack |
| `arrivalPhotos` | ArrivalPhotosPage | Stack |
| `systemDetail` | SystemDetailPage | Stack |
| `workItemDetail` | WorkItemDetailPage | Stack |
| `completeAppointment` | CompleteAppointmentPage | Stack |
| `profile` | ProfilePage | FlyoutItem |
| `settings` | SettingsPage | FlyoutItem |

### Navigation Examples
```csharp
// Root navigation (replaces stack)
await Shell.Current.GoToAsync("//home");

// Push to stack
await Shell.Current.GoToAsync("appointmentDetail?id=apt-001");

// Pop from stack
await Shell.Current.GoToAsync("..");
```

## Build Commands

```bash
# Restore packages
dotnet restore

# Build for macOS Catalyst
dotnet build -f net10.0-maccatalyst

# Build for iOS
dotnet build -f net10.0-ios

# Build for Android
dotnet build -f net10.0-android

# Run tests
dotnet test tests/IntegratorMobile.Tests/

# Run on macOS
./run-mac.sh
```

## Testing

### Current Test Coverage (~55%)
| Category | Tests | Location |
|----------|-------|----------|
| Model Tests | 25 | `tests/.../Models/` |
| Service Tests | 18 | `tests/.../Services/` |
| Logic Tests | 45 | `tests/.../Logic/` |
| Integration | 8 | `tests/.../Integration/` |
| Feature Parity | 7 | `tests/.../FeatureParity/` |

### Test Commands
```bash
# Run all tests
dotnet test tests/IntegratorMobile.Tests/

# Run specific category
dotnet test --filter "FullyQualifiedName~Models"
dotnet test --filter "FullyQualifiedName~Integration"
```

### Testing Strategy Reference
- Full strategy: `@docs/testing/testing-strategy.md`
- Key insight: ViewModels need `INavigationService` abstraction for testability

### Path to Higher Coverage
1. **Refactor ViewModels** - Abstract `Shell.Current` behind `INavigationService`
2. **Add ViewModel Tests** - Mock navigation and services
3. **Add Converter Tests** - Requires MAUI test project
4. **Add UI Automation** - Appium or MAUI Test framework

## Known Issues

1. **Xcode Version Warning** - Suppressed via `Directory.Build.props`
2. **iOS Signing** - Not configured for physical devices
3. **Hot Reload** - May require restart for XAML changes
4. **ViewModel Testability** - Coupled to `Shell.Current` (see testing strategy)

## Performance Considerations

- Use `x:DataType` for compiled bindings
- Avoid nested StackLayouts (use Grid)
- Use `CollectionView` instead of `ListView`
- Implement `INotifyPropertyChanged` correctly
- Consider virtualization for large lists

## Key Documentation References

- Testing Strategy: `@docs/testing/testing-strategy.md`
- MAUI Architecture: `@docs/architecture/maui-architecture.md`
- Design System: `@docs/design-system/design-tokens.md`
- Feature Matrix: `@docs/requirements/feature-matrix.md`
- Technical Decisions: `@llm-agents/maui-agent/memory/decisions.md`
