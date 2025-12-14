using IntegratorMobile.MockData.Models;

namespace IntegratorMobile.MockData.Services;

public interface IAuthService
{
    Task<Company?> ValidateCompanyIdentifier(string identifier);
    Task<User?> LoginWithCredentials(string username, string password);
    Task<User?> LoginWithMicrosoft();
    Task Logout();
    Task<User?> GetCurrentUser();
    bool IsAuthenticated { get; }
}
