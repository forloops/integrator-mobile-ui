# Design Tokens - Integrator Mobile

This document defines the design tokens used across both Vue and MAUI prototypes, ensuring visual consistency.

## Color Palette

### Primary Colors

| Token | Hex | Usage | MAUI Resource |
|-------|-----|-------|---------------|
| Primary50 | `#EEF2FF` | Light backgrounds, hover states | `Primary50` |
| Primary100 | `#E0E7FF` | Subtle backgrounds | `Primary100` |
| Primary200 | `#C7D2FE` | Borders, light accents | `Primary200` |
| Primary500 | `#6366F1` | Secondary actions | `Primary500` |
| Primary600 | `#4338CA` | **Primary CTAs, links, active states** | `Primary600` |
| Primary700 | `#4338CA` | Pressed states | `Primary700` |

### Neutral Colors (Slate)

| Token | Hex | Usage | MAUI Resource |
|-------|-----|-------|---------------|
| Slate50 | `#F8FAFC` | Page backgrounds | `BackgroundSecondary` |
| Slate100 | `#F1F5F9` | Card backgrounds | `BackgroundPrimary` |
| Slate200 | `#E2E8F0` | Borders, dividers | `BorderLight` |
| Slate300 | `#CBD5E1` | Disabled borders | `BorderMedium` |
| Slate400 | `#94A3B8` | Placeholder text | `TextTertiary` |
| Slate500 | `#64748B` | Secondary text | `TextSecondary` |
| Slate700 | `#323F4B` | **Headers, navigation bars** | `Slate700` |
| Slate800 | `#1E293B` | Primary text | `TextPrimary` |
| Slate900 | `#0F172A` | Headings | `TextHeading` |

### Semantic Colors

| Token | Hex | Usage | MAUI Resource |
|-------|-----|-------|---------------|
| Success | `#16A34A` | Success states, completed | `Success` |
| SuccessLight | `#DCFCE7` | Success backgrounds | `SuccessLight` |
| Warning | `#D97706` | Warnings, attention needed | `Warning` |
| WarningLight | `#FEF3C7` | Warning backgrounds | `WarningLight` |
| Error | `#DC2626` | Errors, destructive | `Error` |
| ErrorLight | `#FEE2E2` | Error backgrounds | `ErrorLight` |
| Info | `#3B82F6` | Information | `Info` |
| InfoLight | `#DBEAFE` | Info backgrounds | `InfoLight` |

## Typography

### Font Family

The Inter font family is used throughout both prototypes:

| Weight | Name | MAUI Font Family |
|--------|------|------------------|
| 400 | Regular | `InterRegular` |
| 500 | Medium | `InterMedium` |
| 600 | SemiBold | `InterSemiBold` |
| 700 | Bold | `InterBold` |

### Type Scale

| Style | Size | Weight | Line Height | Usage |
|-------|------|--------|-------------|-------|
| HeadingLarge | 24px | Bold | 32px | Page titles |
| HeadingMedium | 20px | SemiBold | 28px | Section headers |
| HeadingSmall | 18px | SemiBold | 24px | Card titles |
| BodyLarge | 16px | Regular | 24px | Primary content |
| BodyMedium | 14px | Regular | 20px | Standard body |
| BodySmall | 12px | Regular | 16px | Secondary text |
| Caption | 10px | SemiBold | 14px | Labels, captions |

## Spacing

### Base Unit: 4px

| Token | Value | Usage |
|-------|-------|-------|
| space-1 | 4px | Tight spacing |
| space-2 | 8px | Default spacing |
| space-3 | 12px | Component padding |
| space-4 | 16px | Section spacing |
| space-5 | 20px | Large gaps |
| space-6 | 24px | Section margins |
| space-8 | 32px | Page margins |

## Border Radius

| Token | Value | Usage |
|-------|-------|-------|
| radius-sm | 4px | Badges, small elements |
| radius-md | 8px | Buttons, inputs |
| radius-lg | 12px | Cards, containers |
| radius-xl | 16px | Large cards |
| radius-full | 9999px | Pills, avatars |

## Shadows

| Token | Value | Usage |
|-------|-------|-------|
| shadow-sm | `0 1px 2px rgba(0,0,0,0.05)` | Subtle elevation |
| shadow-md | `0 4px 6px rgba(0,0,0,0.1)` | Cards, buttons |
| shadow-lg | `0 10px 15px rgba(0,0,0,0.1)` | Modals, dropdowns |

## MAUI Implementation

### Colors.xaml Example

```xml
<ResourceDictionary>
    <!-- Primary -->
    <Color x:Key="Primary50">#EEF2FF</Color>
    <Color x:Key="Primary600">#4338CA</Color>
    
    <!-- Slate -->
    <Color x:Key="Slate700">#323F4B</Color>
    <Color x:Key="BackgroundSecondary">#F8FAFC</Color>
    
    <!-- Semantic -->
    <Color x:Key="Success">#16A34A</Color>
    <Color x:Key="Error">#DC2626</Color>
    <Color x:Key="Warning">#D97706</Color>
</ResourceDictionary>
```

### Usage in XAML

```xml
<Label Text="Hello" 
       TextColor="{StaticResource TextPrimary}"
       FontFamily="InterSemiBold"
       FontSize="16" />

<Border BackgroundColor="{StaticResource Primary600}"
        StrokeShape="RoundRectangle 8">
    <Label Text="Button" TextColor="White" />
</Border>
```

## Tailwind to MAUI Mapping

| Tailwind Class | MAUI Equivalent |
|----------------|-----------------|
| `bg-primary-600` | `BackgroundColor="{StaticResource Primary600}"` |
| `text-slate-700` | `TextColor="{StaticResource Slate700}"` |
| `rounded-lg` | `StrokeShape="RoundRectangle 12"` |
| `p-4` | `Padding="16"` |
| `shadow-md` | `Shadow="..."` |
| `font-semibold` | `FontFamily="InterSemiBold"` |
