# Integrator Mobile Testing Strategy

**Last Updated:** December 2024  
**Status:** Implementation Guide

## Current Test Coverage

### What We Have (58+ Tests)

| Test Category | Test Count | Coverage |
|---------------|------------|----------|
| Model Tests | 25 | ✅ Excellent |
| Service Tests | 18 | ✅ Excellent |
| Integration Tests | 8 | ✅ Good |
| Feature Parity | 7 | ✅ Good |
| **Total** | **58+** | **~40%** |

### What's Missing

| Test Category | Files Needing Tests | Priority |
|---------------|---------------------|----------|
| ViewModel Tests | 6 ViewModels | 🔴 High |
| Converter Tests | 4 converters | 🟡 Medium |
| UI Control Tests | 5 controls | 🟡 Medium |
| Navigation Tests | Shell routes | 🟢 Low |
| UI Automation | End-to-end | 🟢 Low |

---

## Achieving Better Coverage

### Option 1: ViewModel Unit Tests (Recommended First)

**Effort:** Medium  
**Coverage Gain:** +30%

#### Requirements

1. **Create separate test project targeting MAUI**
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <TargetFrameworks>net10.0-ios;net10.0-android;net10.0-maccatalyst</TargetFrameworks>
       <UseMaui>true</UseMaui>
     </PropertyGroup>
   </Project>
   ```

2. **Mock Shell navigation**
   ```csharp
   public interface INavigationService
   {
       Task GoToAsync(string route);
       Task GoBackAsync();
       Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
   }
   ```

3. **Refactor ViewModels to use INavigationService**
   - Currently: `Shell.Current.GoToAsync()`
   - Better: `_navigationService.GoToAsync()`

#### Example ViewModel Test

```csharp
public class AppointmentsPageViewModelTests
{
    private readonly Mock<IAppointmentService> _mockAppointmentService;
    private readonly Mock<INavigationService> _mockNavigationService;
    private readonly AppointmentsPageViewModel _sut;

    public AppointmentsPageViewModelTests()
    {
        _mockAppointmentService = new Mock<IAppointmentService>();
        _mockNavigationService = new Mock<INavigationService>();
        _sut = new AppointmentsPageViewModel(
            _mockAppointmentService.Object,
            _mockNavigationService.Object);
    }

    [Fact]
    public async Task LoadAppointments_PopulatesTodayAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new() { Id = "1", CustomerName = "Test Customer" }
        };
        _mockAppointmentService
            .Setup(x => x.GetTodayAppointments())
            .ReturnsAsync(appointments);
        
        // Act
        await _sut.LoadAppointmentsCommand.ExecuteAsync(null);
        
        // Assert
        _sut.TodayAppointments.Should().HaveCount(1);
        _sut.HasTodayAppointments.Should().BeTrue();
    }

    [Fact]
    public void FilterAppointments_WithQuery_FiltersResults()
    {
        // Arrange - Pre-populate
        _sut.TodayAppointments.Add(new Appointment 
        { 
            CustomerName = "ABC Company",
            SiteName = "Main Office"
        });
        _sut.TodayAppointments.Add(new Appointment 
        { 
            CustomerName = "XYZ Corp",
            SiteName = "Branch"
        });
        
        // Act
        _sut.SearchQuery = "ABC";
        
        // Assert
        _sut.TodayAppointments.Should().HaveCount(1);
        _sut.TodayAppointments.First().CustomerName.Should().Contain("ABC");
    }

    [Fact]
    public void ToggleSearch_TogglesVisibility()
    {
        // Arrange
        _sut.IsSearchVisible.Should().BeFalse();
        
        // Act
        _sut.ToggleSearchCommand.Execute(null);
        
        // Assert
        _sut.IsSearchVisible.Should().BeTrue();
    }

    [Fact]
    public async Task SelectAppointment_NavigatesToDetail()
    {
        // Arrange
        var appointment = new Appointment { Id = "apt-001" };
        
        // Act
        await _sut.SelectAppointmentCommand.ExecuteAsync(appointment);
        
        // Assert
        _mockNavigationService.Verify(
            x => x.GoToAsync("appointments/detail?id=apt-001"),
            Times.Once);
    }
}
```

### Option 2: Converter Tests

**Effort:** Low  
**Coverage Gain:** +5%

#### Example Tests (Requires MAUI)

```csharp
public class StatusToColorConverterTests
{
    private readonly StatusToColorConverter _sut = new();

    [Theory]
    [InlineData("SCHEDULED", "#64748B")]
    [InlineData("EN ROUTE", "#0284C7")]
    [InlineData("ON SITE", "#16A34A")]
    [InlineData("IN PROGRESS", "#0284C7")]
    [InlineData("COMPLETED", "#16A34A")]
    [InlineData("NEED TO RETURN", "#D97706")]
    [InlineData("CANCELLED", "#DC2626")]
    public void Convert_ReturnsCorrectColor(string status, string expectedHex)
    {
        // Act
        var result = _sut.Convert(status, typeof(Color), null, CultureInfo.InvariantCulture);
        
        // Assert
        result.Should().BeOfType<Color>();
        ((Color)result).ToArgbHex().Should().Be(expectedHex);
    }

    [Fact]
    public void Convert_WithUnknownStatus_ReturnsDefaultColor()
    {
        // Act
        var result = _sut.Convert("UNKNOWN", typeof(Color), null, CultureInfo.InvariantCulture);
        
        // Assert
        ((Color)result).ToArgbHex().Should().Be("#64748B");
    }
}
```

### Option 3: UI Control Logic Tests

**Effort:** Medium  
**Coverage Gain:** +10%

#### Example: AppBadge Tests

```csharp
public class AppBadgeTests
{
    [Fact]
    public void BadgeType_Success_SetsCorrectColors()
    {
        // Arrange
        var badge = new AppBadge();
        
        // Act
        badge.BadgeType = BadgeType.Success;
        
        // Assert - Would need access to internal controls
        // badge.BadgeBorder.BackgroundColor should be green
    }
}
```

### Option 4: UI Automation Tests (Appium/MAUI Test)

**Effort:** High  
**Coverage Gain:** +25%

```csharp
[Test]
public async Task LoginFlow_ValidCredentials_NavigatesToHome()
{
    // Arrange
    await App.EnterText("CompanyIdentifierEntry", "crowther");
    await App.Tap("ContinueButton");
    
    await App.WaitForElement("MicrosoftLoginButton");
    await App.Tap("MicrosoftLoginButton");
    
    // Assert
    await App.WaitForElement("HomeWelcomeLabel");
    var welcomeText = await App.GetText("HomeWelcomeLabel");
    welcomeText.Should().Contain("Welcome");
}
```

---

## Recommended Implementation Order

### Phase 1: ViewModel Tests (Week 1-2)

1. Create `IntegratorMobile.Tests.ViewModels` project with MAUI support
2. Refactor ViewModels to accept `INavigationService`
3. Write tests for:
   - `IdentifyPageViewModel` (5 tests)
   - `ManualLoginPageViewModel` (5 tests)
   - `LoginPageViewModel` (3 tests)
   - `AppointmentsPageViewModel` (10 tests)
   - `AppointmentDetailPageViewModel` (8 tests)
   - `BaseViewModel` (5 tests)

**Expected Coverage:** +30% → **~70%**

### Phase 2: Converter & Control Tests (Week 3)

1. Add converter tests (8 tests)
2. Add UI control behavioral tests (10 tests)

**Expected Coverage:** +15% → **~85%**

### Phase 3: UI Automation (Week 4+)

1. Set up Appium or MAUI Test framework
2. Write E2E tests for critical paths:
   - Authentication flow
   - Appointment workflow
   - Punch list completion

**Expected Coverage:** +10% → **~95%**

---

## Code Changes Required

### 1. Create INavigationService Interface

```csharp
// src/IntegratorMobile.Core/Services/INavigationService.cs
public interface INavigationService
{
    Task GoToAsync(string route);
    Task GoToAsync(string route, IDictionary<string, object> parameters);
    Task GoBackAsync();
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    Task DisplayAlertAsync(string title, string message, string ok);
}
```

### 2. Create Shell Navigation Service Implementation

```csharp
// src/IntegratorMobile.Prototype/Services/ShellNavigationService.cs
public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route) => Shell.Current.GoToAsync(route);
    
    public Task GoToAsync(string route, IDictionary<string, object> parameters)
        => Shell.Current.GoToAsync(route, parameters);
    
    public Task GoBackAsync() => Shell.Current.GoToAsync("..");
    
    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
        => Shell.Current.DisplayAlert(title, message, accept, cancel);
    
    public Task DisplayAlertAsync(string title, string message, string ok)
        => Shell.Current.DisplayAlert(title, message, ok);
}
```

### 3. Update ViewModels

```csharp
// Before
public class AppointmentsPageViewModel : BaseViewModel
{
    public AppointmentsPageViewModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    private async Task SelectAppointmentAsync(Appointment appointment)
    {
        await Shell.Current.GoToAsync($"appointments/detail?id={appointment.Id}");
    }
}

// After
public class AppointmentsPageViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    
    public AppointmentsPageViewModel(
        IAppointmentService appointmentService,
        INavigationService navigationService)
    {
        _appointmentService = appointmentService;
        _navigationService = navigationService;
    }
    
    private async Task SelectAppointmentAsync(Appointment appointment)
    {
        await _navigationService.GoToAsync($"appointments/detail?id={appointment.Id}");
    }
}
```

### 4. Register in DI

```csharp
// MauiProgram.cs
builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
```

---

## Summary

| Approach | Effort | Coverage Gain | Recommendation |
|----------|--------|---------------|----------------|
| ViewModel Tests | Medium | +30% | ✅ Start here |
| Converter Tests | Low | +5% | ✅ Quick win |
| UI Control Tests | Medium | +10% | 🔶 After ViewModels |
| UI Automation | High | +10% | 🔶 Production phase |

**Total Potential Coverage:** ~95%

The key architectural change needed is abstracting `Shell.Current` behind an `INavigationService` interface, which enables full testability of ViewModels without MAUI runtime dependencies.
