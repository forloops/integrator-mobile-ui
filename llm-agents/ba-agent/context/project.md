# BA Agent - Project Context

## Project Overview

**Project:** Integrator Mobile  
**Type:** Field Service Mobile Application  
**Current Phase:** Prototype Development

## Business Objectives

1. **Improve Field Technician Efficiency**
   - Reduce time spent on paperwork
   - Streamline appointment workflows
   - Enable offline work capability

2. **Enhance Data Capture Quality**
   - Required arrival photos
   - Milestone documentation
   - System inventory tracking

3. **Provide Better Customer Communication**
   - Real-time status updates
   - Customer notifications
   - Digital documentation

## User Personas

### Primary: Service Technician (John)
- **Role:** Field technician performing HVAC service calls
- **Goals:** Complete appointments efficiently, document work accurately
- **Pain Points:** Paper forms, poor connectivity, unclear job details
- **Tech Comfort:** Moderate, uses smartphone daily

### Secondary: Surveyor (Maria)
- **Role:** Conducts building and system surveys
- **Goals:** Thorough documentation, accurate estimates
- **Pain Points:** Managing photos, tracking multiple systems
- **Tech Comfort:** High, uses tablets and specialized software

### Tertiary: Lead Technician (Tom)
- **Role:** Supervises field team, handles complex issues
- **Goals:** Team visibility, quality oversight
- **Pain Points:** Tracking team progress, reviewing work quality
- **Tech Comfort:** Moderate-High

## Core Workflows

### 1. Authentication Flow
```
Company ID → Login Options → SSO/Manual → Home
```
- Multi-tenant identification
- Microsoft SSO or username/password
- Session persistence

### 2. Appointment Workflow (Punch List)
```
Start → Drive → Arrive (Photo) → Survey → Complete
```
- Four sequential steps
- Gated progression
- Required documentation

### 3. Work Item Lifecycle
```
Created → Ready → In Progress → Completed/Need to Return
```
- Multiple work item types
- Milestone photos
- Status tracking

## Feature Priorities (MoSCoW)

### Must Have (MVP)
- Authentication (company + user)
- Appointment list and detail
- Punch list workflow
- Arrival photo capture
- Work item status updates

### Should Have
- Offline mode (view appointments)
- Building/system hierarchy
- Customer contact actions
- Navigation integration

### Could Have
- Push notifications
- Photo gallery
- Signature capture
- Time tracking

### Won't Have (This Phase)
- Dispatch features
- Scheduling
- Invoice generation
- Parts inventory

## Current Status

### Prototype Completion
| Area | Vue | MAUI | Notes |
|------|-----|------|-------|
| Auth | 100% | 100% | Complete |
| Appointments | 100% | 98% | Minor polish |
| Punch List | 100% | 100% | Complete |
| Work Items | 100% | 100% | Complete |
| Settings | 90% | 90% | Basic |

### Outstanding Items
1. Photo capture implementation (mock in prototype)
2. Offline mode architecture
3. Real API integration (post-prototype)

## Requirements Reference

- Full requirements: `@docs/requirements/functional-requirements.md`
- Feature matrix: `@docs/requirements/feature-matrix.md`
- MAUI status: `@docs/progress/maui-prototype-status.md`
