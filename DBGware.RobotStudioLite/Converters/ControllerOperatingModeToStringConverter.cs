using System;
using System.Globalization;
using System.Windows.Data;
using ABB.Robotics.Controllers;

namespace DBGware.RobotStudioLite.Converters
{
    public class ControllerOperatingModeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = (string)App.Current.FindResource("ControllerOperatingMode");
            if (value is not ControllerOperatingMode controllerOperatingMode) return prefix;
            string suffix = controllerOperatingMode switch
            {
                ControllerOperatingMode.Auto => (string)App.Current.FindResource("ControllerOperatingMode_Auto"),
                ControllerOperatingMode.AutoChange => (string)App.Current.FindResource("ControllerOperatingMode_AutoChange"),
                ControllerOperatingMode.Init => (string)App.Current.FindResource("ControllerOperatingMode_Init"),
                ControllerOperatingMode.ManualFullSpeed => (string)App.Current.FindResource("ControllerOperatingMode_ManualFullSpeed"),
                ControllerOperatingMode.ManualFullSpeedChange => (string)App.Current.FindResource("ControllerOperatingMode_ManualFullSpeedChange"),
                ControllerOperatingMode.ManualReducedSpeed => (string)App.Current.FindResource("ControllerOperatingMode_ManualReducedSpeed"),
                ControllerOperatingMode.NotApplicable => (string)App.Current.FindResource("ControllerOperatingMode_NotApplicable"),
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