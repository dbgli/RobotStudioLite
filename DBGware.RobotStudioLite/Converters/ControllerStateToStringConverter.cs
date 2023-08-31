using System;
using System.Globalization;
using System.Windows.Data;
using ABB.Robotics.Controllers;

namespace DBGware.RobotStudioLite.Converters
{
    public class ControllerStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string prefix = (string)App.Current.FindResource("ControllerState");
            if (value is not ControllerState controllerState) return prefix;
            string suffix = controllerState switch
            {
                ControllerState.EmergencyStop => (string)App.Current.FindResource("ControllerState_EmergencyStop"),
                ControllerState.EmergencyStopReset => (string)App.Current.FindResource("ControllerState_EmergencyStopReset"),
                ControllerState.GuardStop => (string)App.Current.FindResource("ControllerState_GuardStop"),
                ControllerState.Init => (string)App.Current.FindResource("ControllerState_Init"),
                ControllerState.MotorsOff => (string)App.Current.FindResource("ControllerState_MotorsOff"),
                ControllerState.MotorsOn => (string)App.Current.FindResource("ControllerState_MotorsOn"),
                ControllerState.SystemFailure => (string)App.Current.FindResource("ControllerState_SystemFailure"),
                ControllerState.Unknown => (string)App.Current.FindResource("ControllerState_Unknown"),
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