# UX Agent - Prompt Templates

## Component Design

### New Component Request
```
## Component Design: [Name]

### Purpose
[What this component does]

### Variants
- Variant A: [Description]
- Variant B: [Description]

### Properties
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| [name] | [type] | [value] | [desc] |

### States
- Default
- Hover/Pressed
- Disabled
- Loading
- Error

### Accessibility
- Touch target: [size]
- Focus indicator: [style]
- Screen reader: [label]

### Usage Example
[Code snippet or description]
```

## Page Layout

### Page Design Template
```
## Page: [Name]

### Purpose
[What this page does]

### User Flow
1. User arrives from [previous page]
2. User sees [initial state]
3. User can [actions]
4. User proceeds to [next page]

### Layout (Mobile)
┌─────────────────────┐
│ Header              │
├─────────────────────┤
│                     │
│ Content Area        │
│                     │
├─────────────────────┤
│ Action Button       │
└─────────────────────┘

### Components Used
- [Component 1]
- [Component 2]

### Data Displayed
- [Data field 1]
- [Data field 2]

### Actions Available
- Primary: [action]
- Secondary: [action]

### Empty State
[What to show when no data]

### Error State
[What to show on error]

### Loading State
[What to show while loading]
```

## Design Review

### Accessibility Review
```
## Accessibility Review: [Page/Component]

### Color Contrast
| Element | Foreground | Background | Ratio | Pass? |
|---------|------------|------------|-------|-------|
| [elem] | [color] | [color] | [x:1] | ✅/❌ |

### Touch Targets
| Element | Size | Min Required | Pass? |
|---------|------|--------------|-------|
| [elem] | [px] | 44px | ✅/❌ |

### Focus Management
- [ ] Focus visible
- [ ] Logical order
- [ ] No traps

### Screen Reader
- [ ] Labels present
- [ ] Roles correct
- [ ] States announced

### Recommendations
1. [Recommendation 1]
2. [Recommendation 2]
```

### Design System Audit
```
## Design System Audit: [Area]

### Tokens Used
| Element | Token | Correct? |
|---------|-------|----------|
| [elem] | [token] | ✅/❌ |

### Components Used
| Element | Component | Standard? |
|---------|-----------|-----------|
| [elem] | [component] | ✅/❌ |

### Deviations Found
1. [Deviation 1] - [Reason/Fix]
2. [Deviation 2] - [Reason/Fix]

### Recommendations
1. [Recommendation 1]
2. [Recommendation 2]
```

## User Flow

### User Journey Map
```
## User Journey: [Task Name]

### Persona
[User persona reference]

### Goal
[What user wants to accomplish]

### Steps
| Step | Action | Screen | Emotion | Pain Points |
|------|--------|--------|---------|-------------|
| 1 | [action] | [screen] | [emoji] | [issues] |
| 2 | [action] | [screen] | [emoji] | [issues] |

### Opportunities
1. [Improvement 1]
2. [Improvement 2]
```

## Responsive Design

### Responsive Breakpoints
```
## Responsive: [Component/Page]

### Phone (<600px)
[Layout description]

### Tablet (600-900px)
[Layout description]

### Desktop (>900px)
[Layout description]

### Key Differences
| Aspect | Phone | Tablet | Desktop |
|--------|-------|--------|---------|
| Columns | [n] | [n] | [n] |
| Navigation | [type] | [type] | [type] |
```
