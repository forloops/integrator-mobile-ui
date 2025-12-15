# Functional Requirements - Integrator Mobile

## 1. Overview

Integrator Mobile is a field service application designed for technicians to manage appointments, document work, and communicate with customers. The application supports multi-tenant deployment with company-specific branding.

## 2. User Roles

| Role | Description | Access Level |
|------|-------------|--------------|
| Service Technician | Field worker performing service calls | Standard |
| Surveyor | Performs building/system surveys | Standard |
| Lead Technician | Supervises field teams | Elevated |
| Dispatcher | Manages scheduling (web only) | Admin |

## 3. Functional Areas

### 3.1 Authentication

#### 3.1.1 Company Identification
- **FR-AUTH-001**: User must identify their company before login
- **FR-AUTH-002**: System validates company identifier against known companies
- **FR-AUTH-003**: Invalid company shows error message
- **FR-AUTH-004**: Valid company proceeds to login options

#### 3.1.2 Login Methods
- **FR-AUTH-010**: Microsoft SSO login (if company supports)
- **FR-AUTH-011**: Manual username/password login (if company supports)
- **FR-AUTH-012**: Remember company identifier for subsequent logins
- **FR-AUTH-013**: Secure token storage for session management

#### 3.1.3 Password Recovery
- **FR-AUTH-020**: Forgot password flow via email
- **FR-AUTH-021**: Username lookup by email
- **FR-AUTH-022**: Password reset with validation rules
- **FR-AUTH-023**: Setup new account flow

### 3.2 Dashboard / Home

#### 3.2.1 Statistics Display
- **FR-HOME-001**: Show today's appointment count
- **FR-HOME-002**: Show open jobs count
- **FR-HOME-003**: Show pending tasks count
- **FR-HOME-004**: Show completed today count

#### 3.2.2 Quick Actions
- **FR-HOME-010**: Quick navigation to Appointments
- **FR-HOME-011**: Quick navigation to Jobs
- **FR-HOME-012**: Quick navigation to Employee Directory

### 3.3 Appointments

#### 3.3.1 Appointment List
- **FR-APPT-001**: View appointments by category (Today/Unresolved/Future)
- **FR-APPT-002**: Display appointment card with key info
- **FR-APPT-003**: Status badge (Scheduled, En Route, On Site, In Progress, Completed)
- **FR-APPT-004**: Filter and search appointments
- **FR-APPT-005**: "I'm Done for the Day" functionality

#### 3.3.2 Appointment Detail
- **FR-APPT-010**: Tabbed view (Job Summary, Job, Customer, Photos)
- **FR-APPT-011**: Display punch list progress
- **FR-APPT-012**: Display building/system hierarchy
- **FR-APPT-013**: Display work items
- **FR-APPT-014**: Cancel or reschedule appointment

### 3.4 Punch List Workflow

#### 3.4.1 Drive to Appointment
- **FR-PUNCH-001**: Mark as En Route
- **FR-PUNCH-002**: Navigation integration (Apple/Google Maps)
- **FR-PUNCH-003**: Display estimated travel time
- **FR-PUNCH-004**: Skip to arrival option

#### 3.4.2 Appointment Arrival
- **FR-PUNCH-010**: Required arrival photo capture
- **FR-PUNCH-011**: Review scope of work
- **FR-PUNCH-012**: Mark arrived timestamp
- **FR-PUNCH-013**: Validate photo before proceeding

#### 3.4.3 Survey Buildings & Systems
- **FR-PUNCH-020**: Building hierarchy display
- **FR-PUNCH-021**: System listing per building
- **FR-PUNCH-022**: Document with photos/videos/notes
- **FR-PUNCH-023**: Add work items to systems

#### 3.4.4 Complete Appointment
- **FR-PUNCH-030**: Completion checklist
- **FR-PUNCH-031**: Customer signature (optional)
- **FR-PUNCH-032**: Final notes
- **FR-PUNCH-033**: Submit completion

### 3.5 Work Items

#### 3.5.1 Work Item Types
- **FR-WORK-001**: Inspection
- **FR-WORK-002**: Survey
- **FR-WORK-003**: Estimate
- **FR-WORK-004**: Adhoc Repair
- **FR-WORK-005**: Line Item Repair

#### 3.5.2 Work Item Workflow
- **FR-WORK-010**: Status tracking (Ready, In Progress, Completed, Need to Return)
- **FR-WORK-011**: Milestone photos (Before, In Progress, Completed)
- **FR-WORK-012**: Notes and documentation
- **FR-WORK-013**: Time tracking

### 3.6 Profile & Settings

#### 3.6.1 User Profile
- **FR-PROF-001**: View user information
- **FR-PROF-002**: Edit profile photo
- **FR-PROF-003**: Contact information display

#### 3.6.2 Settings
- **FR-SET-001**: Notification preferences
- **FR-SET-002**: Theme selection (light/dark)
- **FR-SET-003**: Offline mode settings
- **FR-SET-004**: Sync settings

### 3.7 Additional Features

#### 3.7.1 Employee Directory
- **FR-DIR-001**: Search employees
- **FR-DIR-002**: View employee details
- **FR-DIR-003**: Direct call/message

#### 3.7.2 Operation Jobs
- **FR-JOBS-001**: View assigned jobs
- **FR-JOBS-002**: Job status overview
- **FR-JOBS-003**: Navigate to job appointments

#### 3.7.3 Diagnostics
- **FR-DIAG-001**: View app version
- **FR-DIAG-002**: Network status
- **FR-DIAG-003**: Sync status
- **FR-DIAG-004**: Debug logs

#### 3.7.4 Help
- **FR-HELP-001**: FAQ access
- **FR-HELP-002**: Contact support
- **FR-HELP-003**: Video tutorials

## 4. Non-Functional Requirements

### 4.1 Performance
- **NFR-PERF-001**: App launch time < 3 seconds
- **NFR-PERF-002**: Screen navigation < 200ms
- **NFR-PERF-003**: Photo capture < 1 second

### 4.2 Offline Support
- **NFR-OFF-001**: View appointments offline
- **NFR-OFF-002**: Queue photos for sync
- **NFR-OFF-003**: Background sync when connected

### 4.3 Platform Support
- **NFR-PLAT-001**: iOS 15.0+
- **NFR-PLAT-002**: Android 8.0+
- **NFR-PLAT-003**: macOS Catalyst (development)

## 5. Data Models

### 5.1 Appointment
- ID, Job ID, Job Number
- Customer Name, Site Name
- Service Job Type
- Location (Address, City, State, Zip, Lat/Long)
- Scope of Work
- Scheduled Start/End
- Status, Punch List Progress
- Assigned Users, Buildings, Work Items
- Customer Info

### 5.2 Work Item
- ID, Appointment ID, Building ID, System ID
- Type (Inspection/Survey/Estimate/Repair)
- Status, Title, Description
- Milestones, Notes
- Created At/By, Completed At

### 5.3 Building/System
- Building: ID, Name, Description, Systems[]
- System: ID, Name, Type, Manufacturer, Model
