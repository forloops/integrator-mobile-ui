using System.Globalization;

namespace IntegratorMobile.UI.Converters;

public class StringToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !string.IsNullOrEmpty(value as string);
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
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;
        return false;
    }
}

public class StatusToBadgeTypeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status.ToUpper() switch
            {
                "READY" => Controls.BadgeType.Ready,
                "IN PROGRESS" => Controls.BadgeType.InProgress,
                "COMPLETED" => Controls.BadgeType.Completed,
                "NEED TO RETURN" => Controls.BadgeType.NeedToReturn,
                "SCHEDULED" => Controls.BadgeType.Scheduled,
                "EN ROUTE" => Controls.BadgeType.EnRoute,
                _ => Controls.BadgeType.Default
            };
        }
        return Controls.BadgeType.Default;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
