# Integrator Mobile - MAUI Prototype

A .NET MAUI clickable prototype for the Integrator Mobile field service application. This prototype mirrors the Vue/Vite prototype's UI and workflows, providing a native mobile reference implementation.

## Purpose

This prototype serves as:
1. **Reference Implementation** — A clickable native prototype demonstrating UI/UX patterns
2. **Code Sharing Foundation** — Reusable UI components that can be integrated into the production ServiceTech app
3. **Design System in MAUI** — Translation of the TailwindCSS design tokens to XAML styles

## Project Structure

```
new-mobile-app/
├── IntegratorMobile.sln
├── run-mac.sh                          # Build & run on macOS
├── run-tests.sh                        # Run unit tests
├── src/
│   ├── IntegratorMobile.Prototype/     # Main MAUI app
│   │   ├── Views/                      # Pages organized by feature
│   │   │   ├── Auth/                   # Login flow
│   │   │   ├── Home/                   # Dashboard
│   │   │   ├── Appointments/           # Appointment workflow
│   │   │   ├── Profile/               
│   │   │   └── Settings/              
│   │   ├── ViewModels/                 # MVVM ViewModels
│   │   ├── Converters/                 # Value converters
│   │   └── Resources/Styles/           # Design system
│   │
│   ├── IntegratorMobile.UI/            # Shareable UI library
│   │   ├── Controls/                   # Reusable controls
│   │   │   ├── AppButton.xaml          # Multi-variant button
│   │   │   ├── AppCard.xaml            # Elevated card
│   │   │   ├── AppInput.xaml           # Labeled input
│   │   │   ├── AppBadge.xaml           # Status badges
│   │   │   └── PunchListItem.xaml      # Workflow step
│   │   └── Converters/                 # Shared converters
│   │
│   └── IntegratorMobile.MockData/      # Mock data library
│       ├── Models/                     # Data models
│       │   ├── Appointment.cs
│       │   ├── WorkItem.cs
│       │   ├── Building.cs
│       │   └── User.cs
│       └── Services/                   # Mock services
│           ├── MockAuthService.cs
│           └── MockAppointmentService.cs
│
└── tests/
    └── IntegratorMobile.Tests/         # Unit tests (xUnit)
        ├── Services/                   # Service tests
        └── Models/                     # Model tests
```

## Key Features Implemented

### Authentication Flow
- Company identification (multi-tenant)
- Microsoft SSO login
- Manual login (username/password)
- Login help

### Appointments
- Today/Unresolved/Future tabs
- Appointment detail with tabbed view (Summary/Job/Customer/Photos)
- Punch List workflow
- "I'm Done for the Day" functionality

### Appointment Workflow (Punch List)
1. **Drive to Appointment** — En Route tracking with navigation
2. **Appointment Arrival** — Required arrival photo capture
3. **Survey Buildings & Systems** — Building/system hierarchy
4. **Complete Appointment** — Completion checklist

### Work Items
- Work item types: Inspection, Survey, Estimate, Adhoc Repair, Line Item Repair
- Milestone photos (Before, In Progress, Completed)
- Status management (Ready, In Progress, Completed, Need to Return)

## Design System

Colors are translated from `tailwind.config.js`:

| Token | Color | Usage |
|-------|-------|-------|
| Primary600 | #4338CA | CTAs, links, active states |
| Slate700 | #323F4B | Headers, navigation |
| Error | #DC2626 | Error states, destructive actions |
| Success | #16A34A | Success states, completed |
| Warning | #D97706 | Warnings, need attention |

## Mock Data

The prototype uses mock data for demonstration:

**Valid Test Credentials:**
- Company: `crowther`, `demo`, `acme`
- Username: `jsmith` (any password)

## Building the Project

### Prerequisites
- .NET 10.0 SDK (preview)
- MAUI workload installed: `sudo dotnet workload install maui`
- Visual Studio 2022/2024 or VS Code with MAUI extension
- For iOS: Xcode 26.1+ on macOS
- For Android: Android SDK with API 24+

### Build & Run

```bash
# Restore packages
cd new-mobile-app
dotnet restore

# Build and run for macOS Catalyst (recommended for quick testing on Mac)
./run-mac.sh
# Or manually:
dotnet build -f net10.0-maccatalyst src/IntegratorMobile.Prototype/IntegratorMobile.Prototype.csproj
open "src/IntegratorMobile.Prototype/bin/Debug/net10.0-maccatalyst/maccatalyst-arm64/Integrator Mobile.app"

# Build for iOS
dotnet build -f net10.0-ios

# Build for Android  
dotnet build -f net10.0-android

# Run on iOS Simulator
dotnet build -t:Run -f net10.0-ios

# Run on Android Emulator
dotnet build -t:Run -f net10.0-android
```

### Running Tests

```bash
# Run all tests
./run-tests.sh

# Or manually:
dotnet test tests/IntegratorMobile.Tests/IntegratorMobile.Tests.csproj

# Run with verbose output
dotnet test tests/IntegratorMobile.Tests/IntegratorMobile.Tests.csproj --verbosity normal

# Run specific test category
dotnet test --filter "FullyQualifiedName~MockAuthService"

# Run with coverage (requires coverlet)
dotnet test --collect:"XPlat Code Coverage"
```

The test suite includes:
- **Service Tests** — MockAuthService, MockAppointmentService logic
- **Model Tests** — Data model validation, display formatting
- **Data Integrity Tests** — Mock data consistency checks

### Troubleshooting

**Xcode version mismatch:** If you see "requires Xcode 26.1 but found 26.2", the `Directory.Build.props` includes `<ValidateXcodeVersion>false</ValidateXcodeVersion>` to bypass this check.

**iOS Platform not installed:** Run `xcode-select --install` and ensure the iOS simulator SDK is installed via Xcode.

## Sharing Components with ServiceTech

The `IntegratorMobile.UI` library can be referenced by the production ServiceTech app:

```xml
<!-- In ServiceTech.csproj -->
<ProjectReference Include="..\..\new-mobile-app\src\IntegratorMobile.UI\IntegratorMobile.UI.csproj" />
```

Then use the controls:

```xml
<ContentPage xmlns:ui="clr-namespace:IntegratorMobile.UI.Controls;assembly=IntegratorMobile.UI">
    <ui:AppButton Text="Click Me" Variant="Primary" />
    <ui:AppCard>
        <ui:AppBadge Text="COMPLETED" BadgeType="Success" />
    </ui:AppCard>
</ContentPage>
```

## File Mapping (Vue → MAUI)

| Vue Prototype | MAUI Prototype |
|---------------|----------------|
| `src/views/auth/IdentifyPage.vue` | `Views/Auth/IdentifyPage.xaml` |
| `src/views/auth/LoginPage.vue` | `Views/Auth/LoginPage.xaml` |
| `src/views/app/AppointmentsPage.vue` | `Views/Appointments/AppointmentsPage.xaml` |
| `src/components/ui/AppButton.vue` | `IntegratorMobile.UI/Controls/AppButton.xaml` |
| `tailwind.config.js` | `Resources/Styles/Colors.xaml` |
| `src/data/appointments.ts` | `IntegratorMobile.MockData/Services/MockAppointmentService.cs` |

## Documentation

See the main project documentation:
- `/docs/requirements/` — Functional requirements
- `/docs/design-system/` — UI components and patterns
- `/docs/api/` — API reference (for production implementation)
