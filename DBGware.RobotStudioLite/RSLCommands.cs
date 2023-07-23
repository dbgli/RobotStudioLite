using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DBGware.RobotStudioLite
{
    public static class RSLCommands
    {
        public static RoutedCommand RefreshControllerListCommand { get; } = new();
        public static RoutedCommand ConnectControllerCommand { get; } = new();
        public static RoutedCommand DisconnectControllerCommand { get; } = new();
        public static RoutedCommand CopyControllerInfoCommand { get; } = new();
        public static RoutedCommand ShowControllerDetailsCommand { get; } = new();
        public static RoutedCommand TurnOnMotorsCommand { get; } = new();
        public static RoutedCommand TurnOffMotorsCommand { get; } = new();
        public static RoutedCommand RequestMastershipCommand { get; } = new();
        public static RoutedCommand ReleaseMastershipCommand { get; } = new();
        public static RoutedCommand ResetProgramPointerCommand { get; } = new();
        public static RoutedCommand StartRapidProgramCommand { get; } = new();
        public static RoutedCommand StopRapidProgramCommand { get; } = new();
    }
}