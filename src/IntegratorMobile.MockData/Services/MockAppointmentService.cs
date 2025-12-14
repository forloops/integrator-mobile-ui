using IntegratorMobile.MockData.Models;

namespace IntegratorMobile.MockData.Services;

public class MockAppointmentService : IAppointmentService
{
    private readonly List<Appointment> _appointments;
    private readonly List<WorkItem> _workItems;

    public MockAppointmentService()
    {
        _appointments = GenerateMockAppointments();
        _workItems = GenerateMockWorkItems();
    }

    public Task<List<Appointment>> GetTodayAppointments()
    {
        var today = DateTime.Today;
        var appointments = _appointments
            .Where(a => a.ScheduledStart.Date == today)
            .OrderBy(a => a.ScheduledStart)
            .ToList();
        return Task.FromResult(appointments);
    }

    public Task<List<Appointment>> GetPastAppointments()
    {
        var today = DateTime.Today;
        var appointments = _appointments
            .Where(a => a.ScheduledStart.Date < today || 
                       (a.Status == AppointmentStatus.Completed && a.ScheduledStart.Date == today))
            .OrderByDescending(a => a.ScheduledStart)
            .ToList();
        return Task.FromResult(appointments);
    }

    public Task<List<Appointment>> GetFutureAppointments()
    {
        var today = DateTime.Today;
        var appointments = _appointments
            .Where(a => a.ScheduledStart.Date > today)
            .OrderBy(a => a.ScheduledStart)
            .ToList();
        return Task.FromResult(appointments);
    }

    public Task<Appointment?> GetAppointmentById(string id)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(appointment);
    }

    public Task UpdateAppointmentStatus(string id, AppointmentStatus status)
    {
        var appointment = _appointments.FirstOrDefault(a => a.Id == id);
        if (appointment != null)
        {
            appointment.Status = status;
            
            // Update timestamps based on status
            switch (status)
            {
                case AppointmentStatus.EnRoute:
                    appointment.EnRouteStartTime = DateTime.Now;
                    appointment.PunchListProgress.DriveToAppointment = PunchListStepStatus.InProgress;
                    break;
                case AppointmentStatus.OnSite:
                    appointment.ArrivalTime = DateTime.Now;
                    appointment.PunchListProgress.DriveToAppointment = PunchListStepStatus.Completed;
                    appointment.PunchListProgress.AppointmentArrival = PunchListStepStatus.InProgress;
                    break;
                case AppointmentStatus.InProgress:
                    appointment.PunchListProgress.AppointmentArrival = PunchListStepStatus.Completed;
                    appointment.PunchListProgress.SurveyBuildingsSystems = PunchListStepStatus.InProgress;
                    break;
                case AppointmentStatus.Completed:
                    appointment.CompletedTime = DateTime.Now;
                    appointment.PunchListProgress.SurveyBuildingsSystems = PunchListStepStatus.Completed;
                    appointment.PunchListProgress.CompleteAppointment = PunchListStepStatus.Completed;
                    break;
            }
        }
        return Task.CompletedTask;
    }

    public Task<List<WorkItem>> GetWorkItemsForAppointment(string appointmentId)
    {
        var workItems = _workItems.Where(w => w.AppointmentId == appointmentId).ToList();
        return Task.FromResult(workItems);
    }

    public Task<WorkItem?> GetWorkItemById(string id)
    {
        var workItem = _workItems.FirstOrDefault(w => w.Id == id);
        return Task.FromResult(workItem);
    }

    public Task UpdateWorkItemStatus(string id, WorkItemStatus status)
    {
        var workItem = _workItems.FirstOrDefault(w => w.Id == id);
        if (workItem != null)
        {
            workItem.Status = status;
            if (status == WorkItemStatus.Completed)
            {
                workItem.CompletedAt = DateTime.Now;
            }
        }
        return Task.CompletedTask;
    }

    private List<Appointment> GenerateMockAppointments()
    {
        var today = DateTime.Today;
        
        return new List<Appointment>
        {
            // Today's appointments
            new Appointment
            {
                Id = "apt-001",
                JobId = "job-001",
                JobNumber = "J-2024-0892",
                CustomerName = "Sunset Plaza HOA",
                SiteName = "Sunset Plaza Condominiums",
                ServiceJobType = "Operations Job",
                Location = new Location
                {
                    Address = "1234 Sunset Boulevard",
                    City = "Los Angeles",
                    State = "CA",
                    Zip = "90028",
                    Latitude = 34.0982,
                    Longitude = -118.3267
                },
                ScopeOfWork = "Annual HVAC maintenance and inspection for all units in Building A. Replace filters, check refrigerant levels, and document any issues found.",
                ScheduledStart = today.AddHours(8),
                ScheduledEnd = today.AddHours(12),
                Status = AppointmentStatus.Scheduled,
                PunchListProgress = new PunchListProgress
                {
                    DriveToAppointment = PunchListStepStatus.Available,
                    AppointmentArrival = PunchListStepStatus.Locked,
                    SurveyBuildingsSystems = PunchListStepStatus.Locked,
                    CompleteAppointment = PunchListStepStatus.Locked
                },
                AssignedUsers = new List<AssignedUser>
                {
                    new AssignedUser { UserId = "1", Name = "John Smith", Role = "Service Tech" },
                    new AssignedUser { UserId = "2", Name = "Maria Johnson", Role = "Surveyor" }
                },
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Id = "bld-001",
                        Name = "Building A",
                        Description = "Main residential building - 24 units",
                        Systems = new List<SystemInfo>
                        {
                            new SystemInfo { Id = "sys-001", Name = "HVAC Unit 1", Type = "HVAC", Manufacturer = "Carrier", Model = "24ACC636A003", BuildingId = "bld-001" },
                            new SystemInfo { Id = "sys-002", Name = "HVAC Unit 2", Type = "HVAC", Manufacturer = "Carrier", Model = "24ACC636A003", BuildingId = "bld-001" },
                            new SystemInfo { Id = "sys-003", Name = "Main Electrical Panel", Type = "Electrical", Manufacturer = "Square D", Model = "QO130L200PG", BuildingId = "bld-001" }
                        }
                    },
                    new Building
                    {
                        Id = "bld-002",
                        Name = "Building B",
                        Description = "Secondary residential building - 18 units",
                        Systems = new List<SystemInfo>
                        {
                            new SystemInfo { Id = "sys-004", Name = "HVAC Unit 3", Type = "HVAC", Manufacturer = "Trane", Model = "XR15", BuildingId = "bld-002" }
                        }
                    }
                },
                Customer = new CustomerInfo
                {
                    Name = "Sunset Plaza HOA",
                    ApprovedBy = "Margaret Chen",
                    OnSiteContact = new Contact { Name = "Robert Davis", Phone = "(555) 987-6543", Email = "rdavis@sunsetplaza.com" },
                    BillingContact = new Contact { Name = "Lisa Wong", Phone = "(555) 876-5432", Email = "billing@sunsetplaza.com" }
                }
            },
            new Appointment
            {
                Id = "apt-002",
                JobId = "job-002",
                JobNumber = "J-2024-0915",
                CustomerName = "Downtown Medical Center",
                SiteName = "Downtown Medical Center - West Wing",
                ServiceJobType = "Service Call",
                Location = new Location
                {
                    Address = "500 Medical Center Drive",
                    City = "Los Angeles",
                    State = "CA",
                    Zip = "90033"
                },
                ScopeOfWork = "Emergency repair - AC unit not cooling properly in patient rooms. High priority.",
                ScheduledStart = today.AddHours(13),
                ScheduledEnd = today.AddHours(15),
                Status = AppointmentStatus.Scheduled,
                PunchListProgress = new PunchListProgress(),
                AssignedUsers = new List<AssignedUser>
                {
                    new AssignedUser { UserId = "1", Name = "John Smith", Role = "Service Tech" }
                },
                Buildings = new List<Building>
                {
                    new Building
                    {
                        Id = "bld-003",
                        Name = "West Wing",
                        Systems = new List<SystemInfo>
                        {
                            new SystemInfo { Id = "sys-005", Name = "Rooftop AC Unit", Type = "HVAC", Manufacturer = "Lennox", Model = "XC21", BuildingId = "bld-003" }
                        }
                    }
                },
                Customer = new CustomerInfo
                {
                    Name = "Downtown Medical Center",
                    OnSiteContact = new Contact { Name = "Facilities Dept", Phone = "(555) 111-2222" }
                }
            },
            // Future appointment
            new Appointment
            {
                Id = "apt-003",
                JobId = "job-003",
                JobNumber = "J-2024-0920",
                CustomerName = "Pacific Heights School District",
                SiteName = "Jefferson Elementary",
                ServiceJobType = "Survey",
                Location = new Location
                {
                    Address = "789 Education Way",
                    City = "San Francisco",
                    State = "CA",
                    Zip = "94115"
                },
                ScopeOfWork = "Complete HVAC system survey for summer replacement project planning.",
                ScheduledStart = today.AddDays(2).AddHours(9),
                ScheduledEnd = today.AddDays(2).AddHours(14),
                Status = AppointmentStatus.Scheduled,
                PunchListProgress = new PunchListProgress(),
                AssignedUsers = new List<AssignedUser>
                {
                    new AssignedUser { UserId = "2", Name = "Maria Johnson", Role = "Surveyor" }
                },
                Customer = new CustomerInfo
                {
                    Name = "Pacific Heights School District"
                }
            },
            // Past incomplete appointment (Unresolved)
            new Appointment
            {
                Id = "apt-004",
                JobId = "job-004",
                JobNumber = "J-2024-0880",
                CustomerName = "Harbor View Apartments",
                SiteName = "Harbor View - Tower 1",
                ServiceJobType = "Operations Job",
                Location = new Location
                {
                    Address = "100 Harbor Boulevard",
                    City = "Long Beach",
                    State = "CA",
                    Zip = "90802"
                },
                ScopeOfWork = "Fire suppression system inspection - Requires return visit for panel replacement.",
                ScheduledStart = today.AddDays(-1).AddHours(10),
                ScheduledEnd = today.AddDays(-1).AddHours(14),
                Status = AppointmentStatus.InProgress, // Incomplete from yesterday
                PunchListProgress = new PunchListProgress
                {
                    DriveToAppointment = PunchListStepStatus.Completed,
                    AppointmentArrival = PunchListStepStatus.Completed,
                    SurveyBuildingsSystems = PunchListStepStatus.InProgress,
                    CompleteAppointment = PunchListStepStatus.Locked
                },
                AssignedUsers = new List<AssignedUser>
                {
                    new AssignedUser { UserId = "1", Name = "John Smith", Role = "Service Tech" }
                },
                Customer = new CustomerInfo
                {
                    Name = "Harbor View Apartments"
                }
            }
        };
    }

    private List<WorkItem> GenerateMockWorkItems()
    {
        return new List<WorkItem>
        {
            new WorkItem
            {
                Id = "wi-001",
                AppointmentId = "apt-001",
                BuildingId = "bld-001",
                SystemId = "sys-001",
                BuildingName = "Building A",
                SystemName = "HVAC Unit 1",
                Type = WorkItemType.Inspection,
                Status = WorkItemStatus.Ready,
                Title = "Annual HVAC Inspection",
                Description = "Complete annual inspection checklist for HVAC Unit 1",
                CreatedAt = DateTime.Now.AddDays(-7),
                CreatedBy = "System"
            },
            new WorkItem
            {
                Id = "wi-002",
                AppointmentId = "apt-001",
                BuildingId = "bld-001",
                SystemId = "sys-001",
                BuildingName = "Building A",
                SystemName = "HVAC Unit 1",
                Type = WorkItemType.LineItemRepair,
                Status = WorkItemStatus.Ready,
                Title = "Replace Air Filters",
                Description = "Replace all air filters with MERV-13 rated filters",
                CreatedAt = DateTime.Now.AddDays(-7),
                CreatedBy = "System"
            },
            new WorkItem
            {
                Id = "wi-003",
                AppointmentId = "apt-001",
                BuildingId = "bld-001",
                SystemId = "sys-002",
                BuildingName = "Building A",
                SystemName = "HVAC Unit 2",
                Type = WorkItemType.Inspection,
                Status = WorkItemStatus.Ready,
                Title = "Annual HVAC Inspection",
                Description = "Complete annual inspection checklist for HVAC Unit 2",
                CreatedAt = DateTime.Now.AddDays(-7),
                CreatedBy = "System"
            },
            new WorkItem
            {
                Id = "wi-004",
                AppointmentId = "apt-002",
                BuildingId = "bld-003",
                SystemId = "sys-005",
                BuildingName = "West Wing",
                SystemName = "Rooftop AC Unit",
                Type = WorkItemType.AdhocRepair,
                Status = WorkItemStatus.Ready,
                Title = "AC Not Cooling",
                Description = "Diagnose and repair cooling issue - unit running but not cooling",
                CreatedAt = DateTime.Now.AddHours(-2),
                CreatedBy = "Dispatch"
            },
            new WorkItem
            {
                Id = "wi-005",
                AppointmentId = "apt-004",
                BuildingId = "bld-003",
                SystemId = "sys-005",
                BuildingName = "Tower 1",
                SystemName = "Fire Panel",
                Type = WorkItemType.LineItemRepair,
                Status = WorkItemStatus.NeedToReturn,
                Title = "Replace Fire Panel Controller",
                Description = "Controller board failed - replacement part ordered",
                NeedToReturnReason = "Waiting for replacement part to arrive",
                CreatedAt = DateTime.Now.AddDays(-3),
                CreatedBy = "John Smith"
            }
        };
    }
}
