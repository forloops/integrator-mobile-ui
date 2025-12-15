using FluentAssertions;
using IntegratorMobile.MockData.Services;
using Xunit;

namespace IntegratorMobile.Tests.Integration;

/// <summary>
/// Integration tests for the complete authentication flow.
/// Tests the full flow from company identification to login.
/// </summary>
public class AuthenticationFlowTests
{
    private readonly MockAuthService _authService;

    public AuthenticationFlowTests()
    {
        _authService = new MockAuthService();
    }

    #region Company Identification Flow

    [Fact]
    public async Task CompleteLoginFlow_CompanyThenManualLogin()
    {
        // Step 1: Identify company
        var company = await _authService.ValidateCompanyIdentifier("crowther");
        company.Should().NotBeNull();
        company!.Name.Should().Be("Crowther Roofing");

        // Step 2: Verify login options
        company.SupportsAzureLogin.Should().BeTrue();
        company.SupportsManualLogin.Should().BeTrue();

        // Step 3: Login with credentials
        var user = await _authService.LoginWithCredentials("jsmith", "password");
        user.Should().NotBeNull();
        user!.Username.Should().Be("jsmith");

        // Step 4: Verify authenticated
        _authService.IsAuthenticated.Should().BeTrue();

        // Step 5: Get current user
        var currentUser = await _authService.GetCurrentUser();
        currentUser.Should().NotBeNull();
        currentUser!.Username.Should().Be("jsmith");
    }

    [Fact]
    public async Task CompleteLoginFlow_CompanyThenMicrosoftSSO()
    {
        // Step 1: Identify company
        var company = await _authService.ValidateCompanyIdentifier("crowther");
        company.Should().NotBeNull();
        company!.SupportsAzureLogin.Should().BeTrue();

        // Step 2: Login with Microsoft
        var user = await _authService.LoginWithMicrosoft();
        user.Should().NotBeNull();

        // Step 3: Verify authenticated
        _authService.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public async Task LoginFlow_CompanyWithoutSSO_OnlyAllowsManualLogin()
    {
        // Step 1: Identify company that doesn't support SSO
        var company = await _authService.ValidateCompanyIdentifier("acme");
        company.Should().NotBeNull();
        company!.SupportsAzureLogin.Should().BeFalse();
        company.SupportsManualLogin.Should().BeTrue();

        // Step 2: Manual login should still work
        var user = await _authService.LoginWithCredentials("jsmith", "password");
        user.Should().NotBeNull();
    }

    #endregion

    #region Session Management Tests

    [Fact]
    public async Task Session_PersistsAcrossCalls()
    {
        // Login
        await _authService.LoginWithCredentials("jsmith", "password");
        
        // Multiple calls should return same user
        var user1 = await _authService.GetCurrentUser();
        var user2 = await _authService.GetCurrentUser();
        
        user1!.Username.Should().Be(user2!.Username);
        _authService.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public async Task Logout_ClearsSession()
    {
        // Login
        await _authService.LoginWithCredentials("jsmith", "password");
        _authService.IsAuthenticated.Should().BeTrue();

        // Logout
        await _authService.Logout();
        _authService.IsAuthenticated.Should().BeFalse();

        // GetCurrentUser should return null
        var user = await _authService.GetCurrentUser();
        user.Should().BeNull();
    }

    [Fact]
    public async Task Session_CanReloginAfterLogout()
    {
        // First login
        await _authService.LoginWithCredentials("jsmith", "password");
        _authService.IsAuthenticated.Should().BeTrue();

        // Logout
        await _authService.Logout();
        _authService.IsAuthenticated.Should().BeFalse();

        // Re-login with different user
        await _authService.LoginWithCredentials("mjohnson", "password");
        _authService.IsAuthenticated.Should().BeTrue();

        var user = await _authService.GetCurrentUser();
        user!.Username.Should().Be("mjohnson");
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task InvalidCompany_DoesNotAffectSubsequentValidation()
    {
        // First try invalid
        var invalid = await _authService.ValidateCompanyIdentifier("invalid");
        invalid.Should().BeNull();

        // Then valid should still work
        var valid = await _authService.ValidateCompanyIdentifier("crowther");
        valid.Should().NotBeNull();
    }

    [Fact]
    public async Task InvalidLogin_DoesNotSetAuthenticated()
    {
        // Try invalid login
        var user = await _authService.LoginWithCredentials("invaliduser", "password");
        user.Should().BeNull();

        // Should not be authenticated
        _authService.IsAuthenticated.Should().BeFalse();
    }

    #endregion
}
