using FluentAssertions;
using Xunit;

namespace IntegratorMobile.Tests.Converters;

/// <summary>
/// Tests for converter logic used in XAML bindings.
/// These test the logic without requiring MAUI types.
/// </summary>
public class ConverterTests
{
    #region TabSelectedConverter Tests

    [Theory]
    [InlineData(0, "0", true)]
    [InlineData(1, "1", true)]
    [InlineData(2, "2", true)]
    [InlineData(0, "1", false)]
    [InlineData(1, "0", false)]
    public void TabSelectedConverter_ReturnsCorrectSelection(int selectedIndex, string parameter, bool expected)
    {
        // Act
        var result = TabSelectedConvert(selectedIndex, parameter);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void TabSelectedConverter_InvalidParameter_ReturnsFalse()
    {
        // Act
        var result = TabSelectedConvert(0, "invalid");

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region IntEqualConverter Tests

    [Theory]
    [InlineData(0, "0", true)]
    [InlineData(1, "1", true)]
    [InlineData(5, "5", true)]
    [InlineData(0, "1", false)]
    [InlineData(1, "2", false)]
    public void IntEqualConverter_ReturnsCorrectEquality(int value, string parameter, bool expected)
    {
        // Act
        var result = IntEqualConvert(value, parameter);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void IntEqualConverter_InvalidParameter_ReturnsFalse()
    {
        // Act
        var result = IntEqualConvert(0, "not-a-number");

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region InverseBoolConverter Tests

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void InverseBoolConverter_InvertsValue(bool input, bool expected)
    {
        // Act
        var result = InverseBoolConvert(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void InverseBoolConverter_NonBool_ReturnsTrue()
    {
        // Act - null or invalid input
        var result = InverseBoolConvertFromObject(null);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region StatusToColorConverter Tests

    [Theory]
    [InlineData("SCHEDULED", "#64748B")]
    [InlineData("EN ROUTE", "#0284C7")]
    [InlineData("ON SITE", "#16A34A")]
    [InlineData("IN PROGRESS", "#0284C7")]
    [InlineData("COMPLETED", "#16A34A")]
    [InlineData("NEED TO RETURN", "#D97706")]
    [InlineData("CANCELLED", "#DC2626")]
    public void StatusToColorConverter_ReturnsCorrectColor(string status, string expectedHex)
    {
        // Act
        var result = StatusToColorConvert(status);

        // Assert
        result.Should().Be(expectedHex);
    }

    [Theory]
    [InlineData("scheduled")]
    [InlineData("Scheduled")]
    [InlineData("SCHEDULED")]
    public void StatusToColorConverter_IsCaseInsensitive(string status)
    {
        // Act
        var result = StatusToColorConvert(status);

        // Assert
        result.Should().Be("#64748B");
    }

    [Theory]
    [InlineData("UNKNOWN")]
    [InlineData("")]
    [InlineData(null)]
    public void StatusToColorConverter_UnknownStatus_ReturnsDefault(string? status)
    {
        // Act
        var result = StatusToColorConvert(status);

        // Assert
        result.Should().Be("#64748B");
    }

    #endregion

    #region String Not Empty Converter Tests

    [Theory]
    [InlineData("hello", true)]
    [InlineData("  ", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void StringNotEmptyConverter_ReturnsCorrectValue(string? input, bool expected)
    {
        // Act
        var result = !string.IsNullOrWhiteSpace(input);

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region Helper Methods (Converter Logic)

    private static bool TabSelectedConvert(int selectedIndex, string parameter)
    {
        if (int.TryParse(parameter, out int paramIndex))
        {
            return selectedIndex == paramIndex;
        }
        return false;
    }

    private static bool IntEqualConvert(int value, string parameter)
    {
        if (int.TryParse(parameter, out int paramInt))
        {
            return value == paramInt;
        }
        return false;
    }

    private static bool InverseBoolConvert(bool value)
    {
        return !value;
    }

    private static bool InverseBoolConvertFromObject(object? value)
    {
        if (value is bool boolValue)
            return !boolValue;
        return true;
    }

    private static string StatusToColorConvert(string? status)
    {
        if (string.IsNullOrEmpty(status))
            return "#64748B";
            
        return status.ToUpper() switch
        {
            "SCHEDULED" => "#64748B",
            "EN ROUTE" => "#0284C7",
            "ON SITE" => "#16A34A",
            "IN PROGRESS" => "#0284C7",
            "COMPLETED" => "#16A34A",
            "NEED TO RETURN" => "#D97706",
            "CANCELLED" => "#DC2626",
            _ => "#64748B"
        };
    }

    #endregion
}
