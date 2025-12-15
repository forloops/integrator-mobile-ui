# UX Agent - Design Decision Log

## Decision Record

### UX-001: Primary Color Selection
- **Date:** 2024-Q3
- **Decision:** Use Indigo (#4338CA) as primary color
- **Rationale:**
  - High visibility outdoors
  - Professional appearance
  - Accessible contrast ratios
  - Differentiates from common blue apps
- **Alternatives:** Blue (#3B82F6), Purple (#7C3AED)

### UX-002: Tab-Based Appointment Detail
- **Date:** 2024-Q3
- **Decision:** 4-tab layout for appointment detail
- **Tabs:**
  1. Job Summary - Punch list + buildings
  2. Job - Details + work items
  3. Customer - Contacts
  4. Photos - Gallery
- **Rationale:**
  - Organizes dense information
  - Quick access to specific sections
  - Familiar mobile pattern
- **Alternatives:** Single scrolling page, accordion sections

### UX-003: Punch List Step Design
- **Date:** 2024-Q4
- **Decision:** Circular numbered icons with status colors
- **States:**
  - Locked: Gray, lock icon
  - Available: White, number
  - InProgress: Primary50 background
  - Completed: Success, checkmark
- **Rationale:**
  - Clear visual progression
  - Status at a glance
  - Familiar step indicator pattern

### UX-004: Flyout Menu vs Bottom Tabs
- **Date:** 2024-Q4
- **Decision:** Flyout (hamburger) menu for main navigation
- **Rationale:**
  - More than 5 destinations
  - User profile display
  - Cleaner main UI
  - Standard for utility apps
- **Trade-offs:** One extra tap to navigate

### UX-005: Status Badge Colors
- **Date:** 2024-Q4
- **Decision:** Semantic color mapping for statuses
- **Mapping:**
  | Status | Badge Type |
  |--------|------------|
  | Scheduled | Primary |
  | En Route | Info |
  | On Site | Info |
  | In Progress | Primary |
  | Completed | Success |
  | Need to Return | Warning |
  | Cancelled | Error |
- **Rationale:** Consistent meaning across app

### UX-006: Font Selection
- **Date:** 2024-Q3
- **Decision:** Inter font family
- **Rationale:**
  - Excellent readability at small sizes
  - Full weight range (400-700)
  - Free and open source
  - Works well on all platforms
- **Alternatives:** Roboto, SF Pro

### UX-007: Card-Based Layout
- **Date:** 2024-Q4
- **Decision:** Use cards to group related content
- **Styling:**
  - White background
  - 1px border (#E2E8F0)
  - 12px border radius
  - 16px internal padding
- **Rationale:**
  - Clear content boundaries
  - Familiar mobile pattern
  - Easy to scan

## Pending Decisions

### UX-PEND-001: Dark Mode
- **Question:** Support dark mode?
- **Considerations:**
  - User preference vs field conditions
  - Development effort
  - Design system expansion
- **Status:** Deferred to Phase 2

### UX-PEND-002: Photo Gallery Layout
- **Question:** Grid vs list for photos?
- **Options:**
  - 3-column grid (compact)
  - 2-column grid (larger thumbnails)
  - List with metadata
- **Status:** Needs user testing

### UX-PEND-003: Empty State Illustrations
- **Question:** Use illustrations for empty states?
- **Considerations:**
  - Visual interest vs simplicity
  - Consistent style
  - Localization
- **Status:** Pending design review

## Pattern Library

### Successful Patterns
- Status badges for quick scanning
- Chevron (›) for navigation indication
- Gray dividers (1px) between list items
- Emoji icons for low-weight visuals

### Patterns to Avoid
- Complex gestures (users often wearing gloves)
- Small text (<12px)
- Low contrast combinations
- Hidden navigation
