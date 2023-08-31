using System;
using System.Globalization;
using System.Windows.Data;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite.Converters
{
    public class ExecutionStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = (string)App.Current.FindResource("ExecutionStatus");
            if (value is not ExecutionStatus executionStatus) return prefix;
            string suffix = executionStatus switch
            {
                ExecutionStatus.Unknown => (string)App.Current.FindResource("ExecutionStatus_Unknown"),
                ExecutionStatus.Running => (string)App.Current.FindResource("ExecutionStatus_Running"),
                ExecutionStatus.Stopped => (string)App.Current.FindResource("ExecutionStatus_Stopped"),
                _ => throw new NotImplementedException()
            };
            return prefix + suffix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}