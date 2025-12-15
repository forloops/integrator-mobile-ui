# UX Agent - Project Context

## Design Philosophy

### Target Users
Field service technicians working:
- Outdoors in varying light conditions
- Often wearing work gloves
- Under time pressure
- With intermittent connectivity

### Key Design Decisions

1. **High Contrast Colors**
   - Primary: #4338CA (Indigo) for visibility
   - Dark headers: #323F4B for navigation
   - White/light backgrounds for content

2. **Large Touch Targets**
   - Minimum 44px for buttons
   - Generous padding on list items
   - Clear spacing between interactive elements

3. **Clear Visual Hierarchy**
   - Bold headings
   - Status badges for quick scanning
   - Icons + text for clarity

## Current UI Inventory

### Pages (25 total)

**Auth Flow (9 pages)**
- IdentifyPage - Company entry
- LoginPage - Auth method selection
- ManualLoginPage - Username/password
- LoginHelpPage - Help options
- ForgotPasswordPage - Recovery start
- GetUsernamePage - Email lookup
- ResetPasswordPage - New password
- UpdatePasswordPage - Change password
- SetupAccountPage - New account

**Main App (16 pages)**
- HomePage - Dashboard
- AppointmentsPage - List with tabs
- AppointmentDetailPage - 4-tab detail view
- DriveToAppointmentPage - En route
- ArrivalPhotosPage - Photo capture
- SystemDetailPage - System info
- WorkItemDetailPage - Work item view
- AddWorkItemPage - Create work item
- AddMilestonePage - Add milestone photo
- CompleteAppointmentPage - Completion flow
- ProfilePage - User profile
- SettingsPage - App settings
- OperationJobsPage - Jobs list
- EmployeeDirectoryPage - Contacts
- DiagnosticsPage - Debug info
- HelpPage - Support

### Components (5 reusable)

1. **AppButton**
   - Variants: Primary, Secondary, Outline, Danger, Ghost
   - Props: Text, Command, IsBlock, IsEnabled

2. **AppCard**
   - Elevated white card
   - Configurable padding
   - Optional shadow

3. **AppInput**
   - Label + input
   - Error state
   - Keyboard types

4. **AppBadge**
   - Types: Default, Primary, Success, Warning, Error, Info
   - Compact status display

5. **PunchListItem**
   - Status: Locked, Available, InProgress, Completed
   - Step number + title + description

## Design Patterns in Use

### Navigation
- Shell-based flyout menu
- Tab navigation within pages
- Stack-based detail views

### Lists
- CollectionView with DataTemplate
- Pull-to-refresh (planned)
- Empty states

### Forms
- Stacked labels + inputs
- Validation feedback
- Loading states

### Actions
- Primary CTA at bottom
- Destructive actions in red
- Confirmation for irreversible actions

## Accessibility Status

### Implemented
- [x] Semantic color usage
- [x] Text hierarchy
- [x] Touch target sizes
- [x] Loading indicators

### Needs Work
- [ ] Screen reader labels
- [ ] Focus management
- [ ] Reduced motion support
- [ ] Font scaling

## Design References

- Colors: `@docs/design-system/design-tokens.md`
- Components: `@docs/design-system/components.md`
- Vue comparison: `@docs/progress/vue-prototype-comparison.md`
