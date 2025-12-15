using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Logic;

/// <summary>
/// Tests for search/filter logic extracted from ViewModels.
/// These can run without MAUI dependencies.
/// </summary>
public class SearchFilterTests
{
    #region Appointment Search Tests

    [Fact]
    public void MatchesSearch_ByCustomerName_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment("ABC Company", "Main Site");
        
        // Act
        var result = MatchesSearch(appointment, "abc");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_BySiteName_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment("Test Co", "Downtown Office");
        
        // Act
        var result = MatchesSearch(appointment, "downtown");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_ByJobNumber_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment();
        appointment.JobNumber = "JOB-12345";
        
        // Act
        var result = MatchesSearch(appointment, "12345");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_ByAddress_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment();
        appointment.Location = new Location { Address = "123 Main Street", City = "Tampa" };
        
        // Act
        var result = MatchesSearch(appointment, "main street");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_ByCity_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment();
        appointment.Location = new Location { Address = "123 Test St", City = "Los Angeles" };
        
        // Act
        var result = MatchesSearch(appointment, "angeles");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_ByServiceJobType_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment();
        appointment.ServiceJobType = "HVAC Inspection";
        
        // Act
        var result = MatchesSearch(appointment, "hvac");
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_NoMatch_ReturnsFalse()
    {
        // Arrange
        var appointment = CreateTestAppointment("ABC Company", "Main Site");
        
        // Act
        var result = MatchesSearch(appointment, "xyz");
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MatchesSearch_IsCaseInsensitive()
    {
        // Arrange
        var appointment = CreateTestAppointment("ABC COMPANY", "MAIN SITE");
        
        // Act & Assert
        MatchesSearch(appointment, "abc company").Should().BeTrue();
        MatchesSearch(appointment, "ABC COMPANY").Should().BeTrue();
        MatchesSearch(appointment, "Abc Company").Should().BeTrue();
    }

    [Fact]
    public void MatchesSearch_PartialMatch_ReturnsTrue()
    {
        // Arrange
        var appointment = CreateTestAppointment("Crowther Roofing & Sheet Metal", "Headquarters");
        
        // Act & Assert
        MatchesSearch(appointment, "crowther").Should().BeTrue();
        MatchesSearch(appointment, "roofing").Should().BeTrue();
        MatchesSearch(appointment, "sheet").Should().BeTrue();
    }

    #endregion

    #region Filter List Tests

    [Fact]
    public void FilterAppointments_EmptyQuery_ReturnsAll()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            CreateTestAppointment("A", "Site A"),
            CreateTestAppointment("B", "Site B"),
            CreateTestAppointment("C", "Site C")
        };
        
        // Act
        var result = FilterAppointments(appointments, "");
        
        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public void FilterAppointments_WithQuery_ReturnsMatches()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            CreateTestAppointment("ABC Company", "Main"),
            CreateTestAppointment("XYZ Corp", "Branch"),
            CreateTestAppointment("ABC Holdings", "Office")
        };
        
        // Act
        var result = FilterAppointments(appointments, "abc");
        
        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(a => a.CustomerName.ToLower().Should().Contain("abc"));
    }

    [Fact]
    public void FilterAppointments_NoMatches_ReturnsEmpty()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            CreateTestAppointment("ABC", "Site"),
            CreateTestAppointment("DEF", "Site")
        };
        
        // Act
        var result = FilterAppointments(appointments, "xyz");
        
        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FilterAppointments_WhitespaceQuery_ReturnsAll()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            CreateTestAppointment("A", "Site"),
            CreateTestAppointment("B", "Site")
        };
        
        // Act
        var result = FilterAppointments(appointments, "   ");
        
        // Assert
        result.Should().HaveCount(2);
    }

    #endregion

    #region Helper Methods (Extracted from ViewModel)

    /// <summary>
    /// Search matching logic extracted from AppointmentsPageViewModel.
    /// </summary>
    private static bool MatchesSearch(Appointment appointment, string query)
    {
        var lowerQuery = query.ToLowerInvariant();
        return appointment.CustomerName.ToLowerInvariant().Contains(lowerQuery) ||
               appointment.SiteName.ToLowerInvariant().Contains(lowerQuery) ||
               appointment.JobNumber.ToLowerInvariant().Contains(lowerQuery) ||
               appointment.Location.Address.ToLowerInvariant().Contains(lowerQuery) ||
               appointment.Location.City.ToLowerInvariant().Contains(lowerQuery) ||
               appointment.ServiceJobType.ToLowerInvariant().Contains(lowerQuery);
    }

    /// <summary>
    /// Filter logic extracted from AppointmentsPageViewModel.
    /// </summary>
    private static List<Appointment> FilterAppointments(List<Appointment> appointments, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return appointments;
        
        var lowerQuery = query.ToLowerInvariant();
        return appointments.Where(a => MatchesSearch(a, lowerQuery)).ToList();
    }

    private static Appointment CreateTestAppointment(string customerName = "Test Customer", string siteName = "Test Site")
    {
        return new Appointment
        {
            Id = Guid.NewGuid().ToString(),
            CustomerName = customerName,
            SiteName = siteName,
            JobNumber = "TEST-001",
            ServiceJobType = "Service",
            Location = new Location
            {
                Address = "123 Test St",
                City = "Test City",
                State = "TS",
                Zip = "12345"
            }
        };
    }

    #endregion
}
