# MAUI Agent - Technical Decision Log

## Decision Record

### MAUI-001: Target Framework Selection
- **Date:** 2024-Q4
- **Decision:** Use .NET 10.0 Preview for MAUI
- **Rationale:**
  - Latest MAUI improvements
  - Better performance
  - Aligned with production timeline
- **Trade-offs:** Preview stability, Xcode compatibility warnings
- **Mitigation:** `ValidateXcodeVersion=false` in Directory.Build.props

### MAUI-002: MVVM Toolkit Selection
- **Date:** 2024-Q4
- **Decision:** Use CommunityToolkit.Mvvm
- **Rationale:**
  - Source generators reduce boilerplate
  - Well-maintained by Microsoft
  - Excellent documentation
  - `[ObservableProperty]` and `[RelayCommand]` attributes
- **Alternatives:** Prism, ReactiveUI, MVVMLight

### MAUI-003: Navigation Pattern
- **Date:** 2024-Q4
- **Decision:** Shell-based navigation
- **Rationale:**
  - Built-in flyout support
  - URI-based routing
  - Deep linking ready
  - Standard MAUI pattern
- **Trade-offs:** Less flexibility than manual NavigationPage

### MAUI-004: Component Library Architecture
- **Date:** 2024-Q4
- **Decision:** Separate `IntegratorMobile.UI` project
- **Rationale:**
  - Reusable in production ServiceTech app
  - Clear separation of concerns
  - Independent versioning possible
  - Easier testing
- **Components:**
  - AppButton, AppCard, AppInput, AppBadge, PunchListItem

### MAUI-005: Mock Data Layer
- **Date:** 2024-Q4
- **Decision:** Separate `IntegratorMobile.MockData` project
- **Rationale:**
  - Interface-based services (IAuthService, IAppointmentService)
  - Easy to swap with real API client
  - Prototype can run standalone
  - Shared between prototype and tests
- **Pattern:** Repository-like services returning Task<T>

### MAUI-006: Page-ViewModel Registration
- **Date:** 2024-Q4
- **Decision:** Register both pages and ViewModels in DI
- **Pattern:**
  ```csharp
  builder.Services.AddTransient<MyViewModel>();
  builder.Services.AddTransient<MyPage>();
  ```
- **Rationale:**
  - Constructor injection in pages
  - ViewModel can have dependencies
  - Testable ViewModels
- **Alternative:** ServiceLocator (rejected as anti-pattern)

### MAUI-007: Compiled Bindings
- **Date:** 2024-Q4
- **Decision:** Use `x:DataType` for compiled bindings
- **Implementation:**
  ```xml
  <ContentPage x:DataType="vm:MyViewModel">
  ```
- **Rationale:**
  - Compile-time binding validation
  - Better performance
  - IntelliSense support
- **Trade-offs:** Requires DataTemplate x:DataType for collections

### MAUI-008: Font Selection
- **Date:** 2024-Q4
- **Decision:** Inter font family
- **Registration:**
  ```csharp
  fonts.AddFont("Inter-Regular.ttf", "InterRegular");
  fonts.AddFont("Inter-Medium.ttf", "InterMedium");
  fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
  fonts.AddFont("Inter-Bold.ttf", "InterBold");
  ```
- **Rationale:** Matches Vue prototype, excellent readability

### MAUI-009: Layout Strategy
- **Date:** 2024-Q4
- **Decision:** Prefer Grid over nested StackLayouts
- **Rationale:**
  - Better performance
  - More predictable sizing
  - Easier responsive adaptation
- **Pattern:**
  ```xml
  <Grid RowDefinitions="Auto,*,Auto" ColumnDefinitions="*,Auto">
  ```

### MAUI-010: Color Resource Strategy
- **Date:** 2024-Q4
- **Decision:** Define colors in Resources/Styles/Colors.xaml
- **Pattern:**
  ```xml
  <Color x:Key="Primary600">#4338CA</Color>
  ```
- **Usage:**
  ```xml
  <Label TextColor="{StaticResource Primary600}" />
  ```
- **Rationale:** Centralized theming, dark mode ready

## Pending Decisions

### MAUI-PEND-001: Offline Storage
- **Options:**
  - SQLite (sqlite-net-pcl)
  - LiteDB
  - Realm
- **Considerations:** Performance, size, sync complexity
- **Status:** Evaluating for Phase 2

### MAUI-PEND-002: API Client
- **Options:**
  - Refit (interface-based)
  - HttpClient (manual)
  - RestSharp
- **Considerations:** Maintainability, testing, features
- **Status:** Deferred to production phase

### MAUI-PEND-003: Photo Handling
- **Options:**
  - MAUI Essentials MediaPicker
  - Platform-specific implementations
- **Considerations:** Compression, metadata, storage
- **Status:** Needs technical spike

### MAUI-PEND-004: State Management
- **Question:** Need global state beyond services?
- **Options:**
  - Singleton services (current)
  - Redux-like state
  - Event aggregator
- **Status:** Monitoring complexity

## Technical Debt

1. **Missing Unit Tests** - ViewModels need test coverage
2. **Hardcoded Strings** - Should use resources for localization
3. **Photo Capture Mock** - Needs real implementation
4. **Error Handling** - Needs comprehensive try/catch
5. **Loading States** - Some pages missing busy indicators
