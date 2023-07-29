using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for RobotControlPanel.xaml
    /// </summary>
    public partial class RobotControlPanel : UserControl
    {
        public RobotControlPanel()
        {
            InitializeComponent();
        }

        #region 开启电机命令

        private void TurnOnMotorsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.Robot.Controller == null) return;
            App.Robot.Controller.State = ControllerState.MotorsOn;
        }

        private void TurnOnMotorsCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.State != ControllerState.MotorsOn;
        }

        #endregion

        #region 关闭电机命令

        private void TurnOffMotorsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.Robot.Controller == null) return;
            App.Robot.Controller.State = ControllerState.MotorsOff;
        }

        private void TurnOffMotorsCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.State != ControllerState.MotorsOff;
        }

        #endregion

        #region 请求主控权命令

        private void RequestMastershipCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.Robot.Controller == null) return;
            App.Robot.Mastership = Mastership.Request(App.Robot.Controller);
        }

        private void RequestMastershipCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.IsMaster == false;
        }

        #endregion

        #region 释放主控权命令

        private void ReleaseMastershipCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            App.Robot.Mastership?.Release();
            App.Robot.Mastership?.Dispose();
            App.Robot.Mastership = null;
        }

        private void ReleaseMastershipCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.IsMaster == true;
        }

        #endregion

        #region 重置程序指针命令

        private void ResetProgramPointerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            App.Robot.Controller?.Rapid.GetTask("T_ROB1").ResetProgramPointer();
        }

        private void ResetProgramPointerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion

        #region 启动Rapid程序命令

        private void StartRapidProgramCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _ = App.Robot.Controller?.Rapid.Start(true);
        }

        private void StartRapidProgramCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.State == ControllerState.MotorsOn
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion

        #region 停止Rapid程序命令

        private void StopRapidProgramCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            App.Robot.Controller?.Rapid.Stop(StopMode.Immediate);
        }

        private void StopRapidProgramCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Stopped;
        }

        #endregion
    }
}