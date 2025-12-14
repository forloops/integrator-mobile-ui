namespace IntegratorMobile.MockData.Models;

public class Building
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SystemInfo> Systems { get; set; } = new();
}

public class SystemInfo
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string BuildingId { get; set; } = string.Empty;
    
    public string TypeIcon => Type.ToLower() switch
    {
        "hvac" => "hvac.png",
        "electrical" => "electrical.png",
        "plumbing" => "plumbing.png",
        "fire" => "fire.png",
        _ => "system.png"
    };
}
