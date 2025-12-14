namespace IntegratorMobile.MockData.Models;

public class Appointment
{
    public string Id { get; set; } = string.Empty;
    public string JobId { get; set; } = string.Empty;
    public string JobNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string ServiceJobType { get; set; } = string.Empty;
    
    public Location Location { get; set; } = new();
    public string ScopeOfWork { get; set; } = string.Empty;
    
    public DateTime ScheduledStart { get; set; }
    public DateTime ScheduledEnd { get; set; }
    
    public AppointmentStatus Status { get; set; }
    public PunchListProgress PunchListProgress { get; set; } = new();
    
    public List<AssignedUser> AssignedUsers { get; set; } = new();
    public List<Building> Buildings { get; set; } = new();
    public List<WorkItem> WorkItems { get; set; } = new();
    
    public CustomerInfo Customer { get; set; } = new();
    
    public DateTime? EnRouteStartTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public DateTime? CompletedTime { get; set; }
    
    public List<Media> ArrivalPhotos { get; set; } = new();

    public string StatusDisplay => Status switch
    {
        AppointmentStatus.Scheduled => "SCHEDULED",
        AppointmentStatus.EnRoute => "EN ROUTE",
        AppointmentStatus.OnSite => "ON SITE",
        AppointmentStatus.InProgress => "IN PROGRESS",
        AppointmentStatus.Completed => "COMPLETED",
        AppointmentStatus.Cancelled => "CANCELLED",
        AppointmentStatus.Rescheduled => "RESCHEDULED",
        _ => "UNKNOWN"
    };
}

public enum AppointmentStatus
{
    Scheduled,
    EnRoute,
    OnSite,
    InProgress,
    Completed,
    Cancelled,
    Rescheduled
}

public class PunchListProgress
{
    public PunchListStepStatus DriveToAppointment { get; set; } = PunchListStepStatus.Available;
    public PunchListStepStatus AppointmentArrival { get; set; } = PunchListStepStatus.Locked;
    public PunchListStepStatus SurveyBuildingsSystems { get; set; } = PunchListStepStatus.Locked;
    public PunchListStepStatus CompleteAppointment { get; set; } = PunchListStepStatus.Locked;
}

public enum PunchListStepStatus
{
    Locked,
    Available,
    InProgress,
    Completed
}

public class Location
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public string FullAddress => $"{Address}, {City}, {State} {Zip}";
    public string CityStateZip => $"{City}, {State} {Zip}";
}

public class AssignedUser
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class CustomerInfo
{
    public string Name { get; set; } = string.Empty;
    public string ApprovedBy { get; set; } = string.Empty;
    public Contact OnSiteContact { get; set; } = new();
    public Contact BillingContact { get; set; } = new();
}

public class Contact
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class Media
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = "photo";
    public string Uri { get; set; } = string.Empty;
    public string ThumbnailUri { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; }
    public string CapturedBy { get; set; } = string.Empty;
}
