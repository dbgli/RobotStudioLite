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

        public static RoutedCommand RobotJointJogCommand { get; } = new();
        public static RoutedCommand RobotLinearJogCommand { get; } = new();

        public static RoutedCommand LoadPresetCommand { get; } = new();
        public static RoutedCommand SaveAsPresetCommand { get; } = new();
        public static RoutedCommand DeployTaskCommand { get; } = new();
        public static RoutedCommand AddDominoCommand { get; } = new();
        public static RoutedCommand DeleteDominoCommand { get; } = new();
        public static RoutedCommand MoveDominoUpCommand { get; } = new();
        public static RoutedCommand MoveDominoDownCommand { get; } = new();

        public static RoutedCommand CloseProgramModuleTabCommand { get; } = new();
        public static RoutedCommand LoadProgramCommand { get; } = new();
        public static RoutedCommand SynchronizeToRobotCommand { get; } = new();
        public static RoutedCommand CreateBackupCommand { get; } = new();

        public static RoutedCommand ResetSceneCommand { get; } = new();
        public static RoutedCommand ShowOrHideSceneInfoCommand { get; } = new();

        public static RoutedCommand SignInCommand { get; } = new();

        public static RoutedCommand ToggleModelVisibilityCommand { get; } = new();
    }
}