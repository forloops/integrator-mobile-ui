# UI Components - Integrator Mobile

This document describes the reusable UI components in the `IntegratorMobile.UI` library.

## Overview

The component library is designed to be shared between the prototype and the production ServiceTech application. All components follow the design system tokens and support common variants.

## Components

### AppButton

A multi-variant button control supporting different styles and sizes.

**Location:** `IntegratorMobile.UI/Controls/AppButton.xaml`

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Text` | string | "" | Button label |
| `Variant` | string | "Primary" | Style variant |
| `IsBlock` | bool | false | Full-width button |
| `IsEnabled` | bool | true | Interactive state |
| `Command` | ICommand | null | Click command |
| `CommandParameter` | object | null | Command parameter |

#### Variants

| Variant | Background | Text | Border |
|---------|------------|------|--------|
| Primary | Primary600 | White | None |
| Secondary | White | Primary600 | Primary600 |
| Outline | Transparent | Slate700 | Slate300 |
| Danger | Error | White | None |
| Ghost | Transparent | Primary600 | None |

#### Usage

```xml
<controls:AppButton Text="SAVE" Variant="Primary" Command="{Binding SaveCommand}" />
<controls:AppButton Text="CANCEL" Variant="Outline" IsBlock="True" />
<controls:AppButton Text="DELETE" Variant="Danger" />
```

---

### AppCard

An elevated card container with consistent styling.

**Location:** `IntegratorMobile.UI/Controls/AppCard.xaml`

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `CardPadding` | Thickness | 16 | Internal padding |
| `CornerRadius` | int | 12 | Border radius |
| `HasShadow` | bool | true | Show shadow |
| `Content` | View | null | Card content |

#### Usage

```xml
<controls:AppCard>
    <VerticalStackLayout>
        <Label Text="Card Title" Style="{StaticResource HeadingMedium}" />
        <Label Text="Card content goes here" />
    </VerticalStackLayout>
</controls:AppCard>

<controls:AppCard CardPadding="0">
    <!-- Content without padding (e.g., for lists) -->
</controls:AppCard>
```

---

### AppInput

A labeled text input with validation support.

**Location:** `IntegratorMobile.UI/Controls/AppInput.xaml`

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Label` | string | "" | Field label |
| `Placeholder` | string | "" | Input placeholder |
| `Text` | string | "" | Current value |
| `IsPassword` | bool | false | Password masking |
| `Keyboard` | Keyboard | Default | Input type |
| `IsError` | bool | false | Error state |
| `ErrorMessage` | string | "" | Error text |

#### Usage

```xml
<controls:AppInput 
    Label="Email Address"
    Placeholder="Enter your email"
    Keyboard="Email"
    Text="{Binding Email}" />

<controls:AppInput 
    Label="Password"
    IsPassword="True"
    Text="{Binding Password}"
    IsError="{Binding HasPasswordError}"
    ErrorMessage="Password is required" />
```

---

### AppBadge

A status badge for displaying state information.

**Location:** `IntegratorMobile.UI/Controls/AppBadge.xaml`

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Text` | string | "" | Badge text |
| `BadgeType` | string | "Default" | Color variant |

#### Badge Types

| Type | Background | Text |
|------|------------|------|
| Default | Slate100 | Slate700 |
| Primary | Primary50 | Primary600 |
| Success | SuccessLight | Success |
| Warning | WarningLight | Warning |
| Error | ErrorLight | Error |
| Info | InfoLight | Info |

#### Usage

```xml
<controls:AppBadge Text="SCHEDULED" BadgeType="Primary" />
<controls:AppBadge Text="COMPLETED" BadgeType="Success" />
<controls:AppBadge Text="NEED TO RETURN" BadgeType="Warning" />
<controls:AppBadge Text="CANCELLED" BadgeType="Error" />
```

---

### PunchListItem

A workflow step item for the punch list UI.

**Location:** `IntegratorMobile.UI/Controls/PunchListItem.xaml`

#### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Title` | string | "" | Step title |
| `Description` | string | "" | Step description |
| `StepNumber` | int | 1 | Step number |
| `Status` | string | "Locked" | Step status |
| `Command` | ICommand | null | Tap command |

#### Status States

| Status | Icon | Background | Interaction |
|--------|------|------------|-------------|
| Locked | 🔒 | Slate200 | Disabled |
| Available | Step # | White | Enabled |
| InProgress | ⏳ | Primary50 | Enabled |
| Completed | ✓ | Success | Enabled |

#### Usage

```xml
<controls:PunchListItem 
    Title="Drive to Appointment"
    Description="Driving or need directions? Start here."
    StepNumber="1"
    Status="Available"
    Command="{Binding GoToDriveCommand}" />

<controls:PunchListItem 
    Title="Appointment Arrival"
    Description="Take your arrival photo."
    StepNumber="2"
    Status="Locked" />
```

---

## Integration Example

Complete example showing all components together:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:controls="clr-namespace:IntegratorMobile.UI.Controls;assembly=IntegratorMobile.UI">
    
    <ScrollView>
        <VerticalStackLayout Padding="16" Spacing="16">
            
            <!-- Status Badge -->
            <controls:AppBadge Text="IN PROGRESS" BadgeType="Primary" />
            
            <!-- Card with Content -->
            <controls:AppCard>
                <VerticalStackLayout Spacing="12">
                    <Label Text="Appointment Details" FontAttributes="Bold" />
                    
                    <controls:AppInput 
                        Label="Customer Name"
                        Text="{Binding CustomerName}" />
                    
                    <controls:AppInput 
                        Label="Notes"
                        Placeholder="Enter notes..."
                        Text="{Binding Notes}" />
                </VerticalStackLayout>
            </controls:AppCard>
            
            <!-- Punch List -->
            <controls:AppCard CardPadding="0">
                <VerticalStackLayout>
                    <controls:PunchListItem 
                        Title="Step 1"
                        Status="Completed" />
                    <controls:PunchListItem 
                        Title="Step 2"
                        Status="InProgress" />
                    <controls:PunchListItem 
                        Title="Step 3"
                        Status="Locked" />
                </VerticalStackLayout>
            </controls:AppCard>
            
            <!-- Action Buttons -->
            <controls:AppButton Text="SAVE" Variant="Primary" IsBlock="True" />
            <controls:AppButton Text="CANCEL" Variant="Outline" IsBlock="True" />
            
        </VerticalStackLayout>
    </ScrollView>
    
</ContentPage>
```

## Adding to ServiceTech

To use these components in the production app:

```xml
<!-- In ServiceTech.csproj -->
<ProjectReference Include="path/to/IntegratorMobile.UI.csproj" />
```

```xml
<!-- In XAML -->
xmlns:ui="clr-namespace:IntegratorMobile.UI.Controls;assembly=IntegratorMobile.UI"

<ui:AppButton Text="Click Me" Variant="Primary" />
```
