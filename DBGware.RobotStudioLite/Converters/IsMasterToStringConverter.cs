using System;
using System.Globalization;
using System.Windows.Data;

namespace DBGware.RobotStudioLite.Converters
{
    public class IsMasterToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = (string)App.Current.FindResource("IsMaster");
            if (value is not bool isMaster) return prefix;
            string suffix = isMaster ? (string)App.Current.FindResource("Holding")
                                     : (string)App.Current.FindResource("NotHolding");
            return prefix + suffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}