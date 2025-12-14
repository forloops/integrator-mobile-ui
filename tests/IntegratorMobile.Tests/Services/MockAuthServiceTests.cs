using FluentAssertions;
using IntegratorMobile.MockData.Services;
using Xunit;

namespace IntegratorMobile.Tests.Services;

public class MockAuthServiceTests
{
    private readonly MockAuthService _sut;

    public MockAuthServiceTests()
    {
        _sut = new MockAuthService();
    }

    #region ValidateCompanyIdentifier Tests

    [Theory]
    [InlineData("crowther", "Crowther Roofing")]
    [InlineData("demo", "Demo Company")]
    [InlineData("acme", "ACME Services")]
    public async Task ValidateCompanyIdentifier_WithValidIdentifier_ReturnsCompany(string identifier, string expectedName)
    {
        // Act
        var result = await _sut.ValidateCompanyIdentifier(identifier);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expectedName);
        result.Identifier.Should().BeEquivalentTo(identifier);
    }

    [Theory]
    [InlineData("CROWTHER")]
    [InlineData("Crowther")]
    [InlineData("DEMO")]
    public async Task ValidateCompanyIdentifier_IsCaseInsensitive(string identifier)
    {
        // Act
        var result = await _sut.ValidateCompanyIdentifier(identifier);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("nonexistent")]
    public async Task ValidateCompanyIdentifier_WithInvalidIdentifier_ReturnsNull(string identifier)
    {
        // Act
        var result = await _sut.ValidateCompanyIdentifier(identifier);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ValidateCompanyIdentifier_Crowther_SupportsAzureLogin()
    {
        // Act
        var result = await _sut.ValidateCompanyIdentifier("crowther");

        // Assert
        result.Should().NotBeNull();
        result!.SupportsAzureLogin.Should().BeTrue();
        result.SupportsManualLogin.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateCompanyIdentifier_Acme_DoesNotSupportAzureLogin()
    {
        // Act
        var result = await _sut.ValidateCompanyIdentifier("acme");

        // Assert
        result.Should().NotBeNull();
        result!.SupportsAzureLogin.Should().BeFalse();
        result.SupportsManualLogin.Should().BeTrue();
    }

    #endregion

    #region LoginWithCredentials Tests

    [Fact]
    public async Task LoginWithCredentials_WithValidUsername_ReturnsUser()
    {
        // Arrange
        var username = "jsmith";
        var password = "anypassword"; // Mock accepts any password

        // Act
        var result = await _sut.LoginWithCredentials(username, password);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("jsmith");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task LoginWithCredentials_IsCaseInsensitive()
    {
        // Act
        var result = await _sut.LoginWithCredentials("JSMITH", "password");

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("jsmith");
    }

    [Fact]
    public async Task LoginWithCredentials_WithInvalidUsername_ReturnsNull()
    {
        // Act
        var result = await _sut.LoginWithCredentials("invaliduser", "password");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginWithCredentials_SetsIsAuthenticated()
    {
        // Arrange
        _sut.IsAuthenticated.Should().BeFalse();

        // Act
        await _sut.LoginWithCredentials("jsmith", "password");

        // Assert
        _sut.IsAuthenticated.Should().BeTrue();
    }

    #endregion

    #region LoginWithMicrosoft Tests

    [Fact]
    public async Task LoginWithMicrosoft_ReturnsFirstUser()
    {
        // Act
        var result = await _sut.LoginWithMicrosoft();

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("jsmith");
    }

    [Fact]
    public async Task LoginWithMicrosoft_SetsIsAuthenticated()
    {
        // Arrange
        _sut.IsAuthenticated.Should().BeFalse();

        // Act
        await _sut.LoginWithMicrosoft();

        // Assert
        _sut.IsAuthenticated.Should().BeTrue();
    }

    #endregion

    #region Logout Tests

    [Fact]
    public async Task Logout_ClearsAuthentication()
    {
        // Arrange
        await _sut.LoginWithCredentials("jsmith", "password");
        _sut.IsAuthenticated.Should().BeTrue();

        // Act
        await _sut.Logout();

        // Assert
        _sut.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public async Task Logout_GetCurrentUser_ReturnsNull()
    {
        // Arrange
        await _sut.LoginWithCredentials("jsmith", "password");
        var userBefore = await _sut.GetCurrentUser();
        userBefore.Should().NotBeNull();

        // Act
        await _sut.Logout();

        // Assert
        var userAfter = await _sut.GetCurrentUser();
        userAfter.Should().BeNull();
    }

    #endregion

    #region GetCurrentUser Tests

    [Fact]
    public async Task GetCurrentUser_WhenNotLoggedIn_ReturnsNull()
    {
        // Act
        var result = await _sut.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCurrentUser_WhenLoggedIn_ReturnsUser()
    {
        // Arrange
        await _sut.LoginWithCredentials("jsmith", "password");

        // Act
        var result = await _sut.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("jsmith");
    }

    #endregion
}
