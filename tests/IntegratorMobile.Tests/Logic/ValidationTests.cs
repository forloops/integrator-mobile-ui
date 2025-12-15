using FluentAssertions;
using Xunit;

namespace IntegratorMobile.Tests.Logic;

/// <summary>
/// Tests for input validation logic used across the application.
/// </summary>
public class ValidationTests
{
    #region Company Identifier Validation

    [Theory]
    [InlineData("crowther", true)]
    [InlineData("demo", true)]
    [InlineData("acme", true)]
    [InlineData("CROWTHER", true)]
    [InlineData("Crowther", true)]
    public void ValidateCompanyIdentifier_ValidFormats_ReturnsTrue(string identifier, bool expected)
    {
        // Act
        var result = IsValidCompanyIdentifier(identifier);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    [InlineData("ab", false)]  // Too short
    public void ValidateCompanyIdentifier_InvalidFormats_ReturnsFalse(string? identifier, bool expected)
    {
        // Act
        var result = IsValidCompanyIdentifier(identifier);
        
        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Username Validation

    [Theory]
    [InlineData("jsmith", true)]
    [InlineData("john.smith", true)]
    [InlineData("user123", true)]
    [InlineData("a", true)]
    public void ValidateUsername_ValidFormats_ReturnsTrue(string username, bool expected)
    {
        // Act
        var result = IsValidUsername(username);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    public void ValidateUsername_InvalidFormats_ReturnsFalse(string? username, bool expected)
    {
        // Act
        var result = IsValidUsername(username);
        
        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Email Validation

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.co.uk", true)]
    [InlineData("user+tag@example.org", true)]
    public void ValidateEmail_ValidFormats_ReturnsTrue(string email, bool expected)
    {
        // Act
        var result = IsValidEmail(email);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("notanemail", false)]
    [InlineData("missing@domain", false)]
    [InlineData("@nodomain.com", false)]
    public void ValidateEmail_InvalidFormats_ReturnsFalse(string? email, bool expected)
    {
        // Act
        var result = IsValidEmail(email);
        
        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Password Validation

    [Theory]
    [InlineData("password123", true)]
    [InlineData("12345678", true)]
    [InlineData("abcdefgh", true)]
    public void ValidatePassword_ValidFormats_ReturnsTrue(string password, bool expected)
    {
        // Act
        var result = IsValidPassword(password);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("short", false)]  // Less than 8 characters
    [InlineData("1234567", false)]  // 7 characters
    public void ValidatePassword_InvalidFormats_ReturnsFalse(string? password, bool expected)
    {
        // Act
        var result = IsValidPassword(password);
        
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ValidatePasswordMatch_SamePasswords_ReturnsTrue()
    {
        // Arrange
        var password = "password123";
        var confirm = "password123";
        
        // Act
        var result = DoPasswordsMatch(password, confirm);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ValidatePasswordMatch_DifferentPasswords_ReturnsFalse()
    {
        // Arrange
        var password = "password123";
        var confirm = "password456";
        
        // Act
        var result = DoPasswordsMatch(password, confirm);
        
        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Phone Number Validation

    [Theory]
    [InlineData("(555) 123-4567", true)]
    [InlineData("555-123-4567", true)]
    [InlineData("5551234567", true)]
    [InlineData("+1 555 123 4567", true)]
    public void ValidatePhoneNumber_ValidFormats_ReturnsTrue(string phone, bool expected)
    {
        // Act
        var result = IsValidPhoneNumber(phone);
        
        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("123", false)]  // Too short
    [InlineData("abcdefghij", false)]  // Letters only
    public void ValidatePhoneNumber_InvalidFormats_ReturnsFalse(string? phone, bool expected)
    {
        // Act
        var result = IsValidPhoneNumber(phone);
        
        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Helper Methods (Validation Logic)

    private static bool IsValidCompanyIdentifier(string? identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return false;
        return identifier.Trim().Length >= 3;
    }

    private static bool IsValidUsername(string? username)
    {
        return !string.IsNullOrWhiteSpace(username);
    }

    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        // Simple email validation
        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
            return false;
        
        var dotIndex = email.LastIndexOf('.');
        return dotIndex > atIndex + 1 && dotIndex < email.Length - 1;
    }

    private static bool IsValidPassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;
        return password.Length >= 8;
    }

    private static bool DoPasswordsMatch(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }

    private static bool IsValidPhoneNumber(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;
        
        // Remove common formatting characters
        var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());
        return digitsOnly.Length >= 10;
    }

    #endregion
}
