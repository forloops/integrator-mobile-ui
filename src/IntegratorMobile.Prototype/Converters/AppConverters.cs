using System.Globalization;

namespace IntegratorMobile.Converters;

public class TabSelectedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int selectedIndex && parameter is string paramStr && int.TryParse(paramStr, out int paramIndex))
        {
            return selectedIndex == paramIndex ? Colors.White : Color.FromArgb("#94A3B8");
        }
        return Color.FromArgb("#94A3B8");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IntEqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is string paramStr && int.TryParse(paramStr, out int paramInt))
        {
            return intValue == paramInt;
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;
        return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;
        return true;
    }
}

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status.ToUpper() switch
            {
                "SCHEDULED" => Color.FromArgb("#64748B"),
                "EN ROUTE" => Color.FromArgb("#0284C7"),
                "ON SITE" => Color.FromArgb("#16A34A"),
                "IN PROGRESS" => Color.FromArgb("#0284C7"),
                "COMPLETED" => Color.FromArgb("#16A34A"),
                "NEED TO RETURN" => Color.FromArgb("#D97706"),
                "CANCELLED" => Color.FromArgb("#DC2626"),
                _ => Color.FromArgb("#64748B")
            };
        }
        return Color.FromArgb("#64748B");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
