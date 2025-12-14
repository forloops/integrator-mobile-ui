namespace IntegratorMobile.MockData.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
}

public class Company
{
    public string Id { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool SupportsAzureLogin { get; set; } = true;
    public bool SupportsManualLogin { get; set; } = true;
}
