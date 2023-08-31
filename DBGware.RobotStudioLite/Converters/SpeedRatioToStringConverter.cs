using System;
using System.Globalization;
using System.Windows.Data;

namespace DBGware.RobotStudioLite.Converters
{
    public class SpeedRatioToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = (string)App.Current.FindResource("SpeedRatio");
            if (value is not int speedRatio) return prefix;
            string suffix = $"{speedRatio}%";
            return prefix + suffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}