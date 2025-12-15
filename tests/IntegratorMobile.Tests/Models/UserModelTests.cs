using FluentAssertions;
using IntegratorMobile.MockData.Models;
using Xunit;

namespace IntegratorMobile.Tests.Models;

public class UserModelTests
{
    #region User Tests

    [Fact]
    public void User_HasDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Id.Should().BeEmpty();
        user.Username.Should().BeEmpty();
        user.FirstName.Should().BeEmpty();
        user.LastName.Should().BeEmpty();
        user.Email.Should().BeEmpty();
        user.Phone.Should().BeEmpty();
        user.Role.Should().BeEmpty();
        user.AvatarUrl.Should().BeEmpty();
    }

    [Fact]
    public void User_CanStoreAllProperties()
    {
        // Arrange & Act
        var user = new User
        {
            Id = "1",
            Username = "jsmith",
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@crowther.com",
            Phone = "(555) 123-4567",
            Role = "Service Tech",
            AvatarUrl = "avatar_jsmith.png"
        };

        // Assert
        user.Id.Should().Be("1");
        user.Username.Should().Be("jsmith");
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Smith");
        user.Email.Should().Be("john.smith@crowther.com");
        user.Phone.Should().Be("(555) 123-4567");
        user.Role.Should().Be("Service Tech");
        user.AvatarUrl.Should().Be("avatar_jsmith.png");
    }

    #endregion

    #region Company Tests

    [Fact]
    public void Company_HasDefaultValues()
    {
        // Arrange & Act
        var company = new Company();

        // Assert
        company.Id.Should().BeEmpty();
        company.Identifier.Should().BeEmpty();
        company.Name.Should().BeEmpty();
        company.LogoUrl.Should().BeEmpty();
        company.SupportsAzureLogin.Should().BeFalse();
        company.SupportsManualLogin.Should().BeFalse();
    }

    [Fact]
    public void Company_CanStoreAllProperties()
    {
        // Arrange & Act
        var company = new Company
        {
            Id = "1",
            Identifier = "crowther",
            Name = "Crowther Roofing",
            LogoUrl = "crowther_logo.png",
            SupportsAzureLogin = true,
            SupportsManualLogin = true
        };

        // Assert
        company.Id.Should().Be("1");
        company.Identifier.Should().Be("crowther");
        company.Name.Should().Be("Crowther Roofing");
        company.LogoUrl.Should().Be("crowther_logo.png");
        company.SupportsAzureLogin.Should().BeTrue();
        company.SupportsManualLogin.Should().BeTrue();
    }

    [Fact]
    public void Company_CanSupportOnlyManualLogin()
    {
        // Arrange & Act
        var company = new Company
        {
            Identifier = "acme",
            Name = "ACME Services",
            SupportsAzureLogin = false,
            SupportsManualLogin = true
        };

        // Assert
        company.SupportsAzureLogin.Should().BeFalse();
        company.SupportsManualLogin.Should().BeTrue();
    }

    #endregion

    #region AssignedUser Tests

    [Fact]
    public void AssignedUser_HasDefaultValues()
    {
        // Arrange & Act
        var assignedUser = new AssignedUser();

        // Assert
        assignedUser.UserId.Should().BeEmpty();
        assignedUser.Name.Should().BeEmpty();
        assignedUser.Role.Should().BeEmpty();
    }

    [Fact]
    public void AssignedUser_CanStoreAllProperties()
    {
        // Arrange & Act
        var assignedUser = new AssignedUser
        {
            UserId = "1",
            Name = "John Smith",
            Role = "Service Tech"
        };

        // Assert
        assignedUser.UserId.Should().Be("1");
        assignedUser.Name.Should().Be("John Smith");
        assignedUser.Role.Should().Be("Service Tech");
    }

    #endregion
}
