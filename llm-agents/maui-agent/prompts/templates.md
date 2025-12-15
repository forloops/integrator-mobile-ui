# MAUI Agent - Prompt Templates

## Page Implementation

### New Page Template
```
## Implement Page: [Name]

### Requirements
- Reference: FR-XXX
- [Functional requirement description]

### Page Structure
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:IntegratorMobile.ViewModels"
             x:Class="IntegratorMobile.Views.[Folder].[Name]Page"
             x:DataType="vm:[Name]PageViewModel"
             Title="{Binding Title}"
             BackgroundColor="{StaticResource BackgroundSecondary}">
    
    <!-- Content here -->
    
</ContentPage>
```

### ViewModel
- Properties needed: [list]
- Commands needed: [list]
- Services needed: [list]

### Navigation
- Route: [route name]
- Parameters: [if any]
- Called from: [source page]
```

## ViewModel Implementation

### New ViewModel Template
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IntegratorMobile.ViewModels;

public partial class [Name]PageViewModel : BaseViewModel
{
    private readonly I[Service] _service;

    [ObservableProperty]
    private [Type] _[propertyName];

    public [Name]PageViewModel(I[Service] service)
    {
        _service = service;
        Title = "[Page Title]";
    }

    [RelayCommand]
    private async Task [Action]Async()
    {
        if (IsBusy) return;
        
        try
        {
            IsBusy = true;
            // Implementation
        }
        catch (Exception ex)
        {
            // Error handling
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

## Component Implementation

### New Control Template
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="IntegratorMobile.UI.Controls.[Name]"
             x:Name="this">
    
    <!-- Control template -->
    <Border>
        <Label Text="{Binding Source={x:Reference this}, Path=[Property]}" />
    </Border>
    
</ContentView>
```

```csharp
namespace IntegratorMobile.UI.Controls;

public partial class [Name] : ContentView
{
    public static readonly BindableProperty [Property]Property =
        BindableProperty.Create(
            nameof([Property]),
            typeof([Type]),
            typeof([Name]),
            default([Type]));

    public [Type] [Property]
    {
        get => ([Type])GetValue([Property]Property);
        set => SetValue([Property]Property, value);
    }

    public [Name]()
    {
        InitializeComponent();
    }
}
```

## Service Implementation

### New Service Template
```csharp
// Interface
namespace IntegratorMobile.MockData.Services;

public interface I[Name]Service
{
    Task<[ReturnType]> [Method]Async([Parameters]);
}

// Implementation
public class Mock[Name]Service : I[Name]Service
{
    public async Task<[ReturnType]> [Method]Async([Parameters])
    {
        await Task.Delay(500); // Simulate network
        // Implementation
    }
}

// Registration in MauiProgram.cs
builder.Services.AddSingleton<I[Name]Service, Mock[Name]Service>();
```

## Common Patterns

### Collection View with Selection
```xml
<CollectionView ItemsSource="{Binding Items}"
                SelectionMode="Single"
                SelectionChangedCommand="{Binding SelectCommand}"
                SelectionChangedCommandParameter="{Binding SelectedItem, Source={RelativeSource Self}}">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="models:[Type]">
            <!-- Item template -->
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

### Tab Navigation
```xml
<Grid RowDefinitions="Auto,*">
    <!-- Tab Header -->
    <HorizontalStackLayout Grid.Row="0">
        <Button Text="Tab 1" Command="{Binding SetTabCommand}" CommandParameter="0" />
        <Button Text="Tab 2" Command="{Binding SetTabCommand}" CommandParameter="1" />
    </HorizontalStackLayout>
    
    <!-- Tab Content -->
    <Grid Grid.Row="1">
        <ScrollView IsVisible="{Binding SelectedTab, Converter={StaticResource IntEqual}, ConverterParameter=0}">
            <!-- Tab 1 Content -->
        </ScrollView>
        <ScrollView IsVisible="{Binding SelectedTab, Converter={StaticResource IntEqual}, ConverterParameter=1}">
            <!-- Tab 2 Content -->
        </ScrollView>
    </Grid>
</Grid>
```

### Form with Validation
```xml
<VerticalStackLayout Spacing="16">
    <controls:AppInput Label="Email"
                       Text="{Binding Email}"
                       Keyboard="Email"
                       IsError="{Binding HasEmailError}"
                       ErrorMessage="{Binding EmailError}" />
    
    <controls:AppInput Label="Password"
                       Text="{Binding Password}"
                       IsPassword="True"
                       IsError="{Binding HasPasswordError}"
                       ErrorMessage="{Binding PasswordError}" />
    
    <controls:AppButton Text="SUBMIT"
                        Variant="Primary"
                        IsBlock="True"
                        Command="{Binding SubmitCommand}" />
</VerticalStackLayout>
```

### Loading State
```xml
<Grid>
    <!-- Main Content -->
    <ScrollView IsVisible="{Binding IsBusy, Converter={StaticResource InverseBool}}">
        <!-- Content -->
    </ScrollView>
    
    <!-- Loading Indicator -->
    <ActivityIndicator IsRunning="{Binding IsBusy}"
                       IsVisible="{Binding IsBusy}"
                       Color="{StaticResource Primary600}"
                       VerticalOptions="Center"
                       HorizontalOptions="Center" />
</Grid>
```

## Code Review Checklist

```markdown
## MAUI Code Review: [File]

### XAML
- [ ] Uses x:DataType for compiled bindings
- [ ] Uses StaticResource for colors/styles
- [ ] Avoids nested StackLayouts (uses Grid)
- [ ] Has loading state
- [ ] Has error state
- [ ] Touch targets >= 44px

### ViewModel
- [ ] Inherits BaseViewModel
- [ ] Uses [ObservableProperty]
- [ ] Uses [RelayCommand]
- [ ] Has IsBusy guards
- [ ] Has try/catch for async
- [ ] Uses constructor injection

### Navigation
- [ ] Route registered in AppShell
- [ ] Uses Shell.GoToAsync
- [ ] QueryProperties documented

### Registration
- [ ] ViewModel registered as Transient
- [ ] Page registered as Transient
- [ ] Services registered appropriately
```
