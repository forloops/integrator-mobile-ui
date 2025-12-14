using IntegratorMobile.MockData.Models;

namespace IntegratorMobile.MockData.Services;

public class MockAuthService : IAuthService
{
    private User? _currentUser;
    private Company? _currentCompany;

    public bool IsAuthenticated => _currentUser != null;

    // Mock companies (matching Vue prototype src/data/mock.ts)
    private readonly List<Company> _companies = new()
    {
        new Company
        {
            Id = "1",
            Identifier = "crowther",
            Name = "Crowther Roofing",
            LogoUrl = "crowther_logo.png",
            SupportsAzureLogin = true,
            SupportsManualLogin = true
        },
        new Company
        {
            Id = "2",
            Identifier = "demo",
            Name = "Demo Company",
            LogoUrl = "demo_logo.png",
            SupportsAzureLogin = true,
            SupportsManualLogin = true
        },
        new Company
        {
            Id = "3",
            Identifier = "acme",
            Name = "ACME Services",
            LogoUrl = "acme_logo.png",
            SupportsAzureLogin = false,
            SupportsManualLogin = true
        }
    };

    // Mock users
    private readonly List<User> _users = new()
    {
        new User
        {
            Id = "1",
            Username = "jsmith",
            FirstName = "John",
            LastName = "Smith",
            Email = "john.smith@crowther.com",
            Phone = "(555) 123-4567",
            Role = "Service Tech",
            AvatarUrl = "avatar_jsmith.png"
        },
        new User
        {
            Id = "2",
            Username = "mjohnson",
            FirstName = "Maria",
            LastName = "Johnson",
            Email = "maria.johnson@crowther.com",
            Phone = "(555) 234-5678",
            Role = "Surveyor",
            AvatarUrl = "avatar_mjohnson.png"
        }
    };

    public Task<Company?> ValidateCompanyIdentifier(string identifier)
    {
        // Simulate network delay
        return Task.Run(async () =>
        {
            await Task.Delay(500);
            var company = _companies.FirstOrDefault(c => 
                c.Identifier.Equals(identifier, StringComparison.OrdinalIgnoreCase));
            
            if (company != null)
            {
                _currentCompany = company;
            }
            
            return company;
        });
    }

    public Task<User?> LoginWithCredentials(string username, string password)
    {
        return Task.Run(async () =>
        {
            await Task.Delay(800);
            
            // For prototype, accept any password for known usernames
            var user = _users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            if (user != null)
            {
                _currentUser = user;
            }
            
            return user;
        });
    }

    public Task<User?> LoginWithMicrosoft()
    {
        return Task.Run(async () =>
        {
            await Task.Delay(1000);
            
            // For prototype, simulate successful Microsoft login
            _currentUser = _users.First();
            return _currentUser;
        });
    }

    public Task Logout()
    {
        _currentUser = null;
        _currentCompany = null;
        return Task.CompletedTask;
    }

    public Task<User?> GetCurrentUser()
    {
        return Task.FromResult(_currentUser);
    }
}
