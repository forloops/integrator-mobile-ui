namespace IntegratorMobile.MockData.Models;

public class WorkItem
{
    public string Id { get; set; } = string.Empty;
    public string AppointmentId { get; set; } = string.Empty;
    public string BuildingId { get; set; } = string.Empty;
    public string SystemId { get; set; } = string.Empty;
    
    public WorkItemType Type { get; set; }
    public WorkItemStatus Status { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string BuildingName { get; set; } = string.Empty;
    public string SystemName { get; set; } = string.Empty;
    
    public List<Milestone> Milestones { get; set; } = new();
    
    public string? NeedToReturnReason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? CompletedAt { get; set; }

    public string StatusDisplay => Status switch
    {
        WorkItemStatus.Created => "CREATED",
        WorkItemStatus.Ready => "READY",
        WorkItemStatus.InProgress => "IN PROGRESS",
        WorkItemStatus.Completed => "COMPLETED",
        WorkItemStatus.NeedToReturn => "NEED TO RETURN",
        _ => "UNKNOWN"
    };

    public string TypeDisplay => Type switch
    {
        WorkItemType.Inspection => "Inspection",
        WorkItemType.Survey => "Survey",
        WorkItemType.Estimate => "Estimate",
        WorkItemType.AdhocRepair => "Adhoc Repair",
        WorkItemType.LineItemRepair => "Line Item Repair",
        _ => "Unknown"
    };
}

public enum WorkItemType
{
    Inspection,
    Survey,
    Estimate,
    AdhocRepair,
    LineItemRepair
}

public enum WorkItemStatus
{
    Created,
    Ready,
    InProgress,
    Completed,
    NeedToReturn
}

public class Milestone
{
    public string Id { get; set; } = string.Empty;
    public string WorkItemId { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Media> Media { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

public enum MilestoneType
{
    Before,
    InProgress,
    Completed,
    Custom
}
