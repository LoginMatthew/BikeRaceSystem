using System.Globalization;

namespace BikeRaceSystem.UI.Maui.Converters;

public class ProgressToHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double progress = (double)value;
        double maxHeight = double.Parse(parameter.ToString());

        return progress * maxHeight;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
