using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Models;

public class BuildingModelTests
{
    #region Building Tests

    [Fact]
    public void Building_HasDefaultValues()
    {
        // Arrange & Act
        var building = new Building();

        // Assert
        building.Id.Should().BeEmpty();
        building.Name.Should().BeEmpty();
        building.Description.Should().BeEmpty();
        building.Systems.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Building_CanHaveMultipleSystems()
    {
        // Arrange
        var building = new Building
        {
            Id = "bld-001",
            Name = "Building A",
            Systems = new List<SystemInfo>
            {
                new SystemInfo { Id = "sys-001", Name = "HVAC Unit 1" },
                new SystemInfo { Id = "sys-002", Name = "HVAC Unit 2" },
                new SystemInfo { Id = "sys-003", Name = "Electrical Panel" }
            }
        };

        // Assert
        building.Systems.Should().HaveCount(3);
    }

    #endregion

    #region SystemInfo Tests

    [Fact]
    public void SystemInfo_HasDefaultValues()
    {
        // Arrange & Act
        var system = new SystemInfo();

        // Assert
        system.Id.Should().BeEmpty();
        system.Name.Should().BeEmpty();
        system.Type.Should().BeEmpty();
        system.Manufacturer.Should().BeEmpty();
        system.Model.Should().BeEmpty();
        system.BuildingId.Should().BeEmpty();
    }

    [Fact]
    public void SystemInfo_CanStoreAllProperties()
    {
        // Arrange & Act
        var system = new SystemInfo
        {
            Id = "sys-001",
            Name = "Rooftop AC Unit",
            Type = "HVAC",
            Manufacturer = "Carrier",
            Model = "24ACC636A003",
            BuildingId = "bld-001"
        };

        // Assert
        system.Id.Should().Be("sys-001");
        system.Name.Should().Be("Rooftop AC Unit");
        system.Type.Should().Be("HVAC");
        system.Manufacturer.Should().Be("Carrier");
        system.Model.Should().Be("24ACC636A003");
        system.BuildingId.Should().Be("bld-001");
    }

    #endregion
}
