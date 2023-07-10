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
    }
}