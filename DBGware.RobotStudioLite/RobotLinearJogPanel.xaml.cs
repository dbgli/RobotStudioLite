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
    /// Interaction logic for RobotLinearJogPanel.xaml
    /// </summary>
    public partial class RobotLinearJogPanel : UserControl
    {
        private RobTarget? linearPosition;

        public RobTarget? LinearPosition
        {
            get => linearPosition;
            set
            {
                linearPosition = value;

                double rx = double.NaN;
                double ry = double.NaN;
                double rz = double.NaN;
                linearPosition?.Rot.ToEulerAngles(out rx, out ry, out rz);
                linear1ValueTextBox.Text = linearPosition?.Trans.X.ToString("F3") ?? "N/A";
                linear2ValueTextBox.Text = linearPosition?.Trans.Y.ToString("F3") ?? "N/A";
                linear3ValueTextBox.Text = linearPosition?.Trans.Z.ToString("F3") ?? "N/A";
                linear4ValueTextBox.Text = !double.IsNaN(rx) ? rx.ToString("F3") : "N/A";
                linear5ValueTextBox.Text = !double.IsNaN(ry) ? ry.ToString("F3") : "N/A";
                linear6ValueTextBox.Text = !double.IsNaN(rz) ? rz.ToString("F3") : "N/A";

                bool isLinearPositionNonNull = linearPosition != null;
                linear1ValueTextBox.IsEnabled = isLinearPositionNonNull;
                linear2ValueTextBox.IsEnabled = isLinearPositionNonNull;
                linear3ValueTextBox.IsEnabled = isLinearPositionNonNull;
                linear4ValueTextBox.IsEnabled = isLinearPositionNonNull;
                linear5ValueTextBox.IsEnabled = isLinearPositionNonNull;
                linear6ValueTextBox.IsEnabled = isLinearPositionNonNull;
            }
        }

        public RobotLinearJogPanel()
        {
            InitializeComponent();
            LinearPosition = null;
        }

        #region 机器人线性点动命令

        private void RobotLinearJogCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 访问Rapid数据
            RapidData? rdJogCommand = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JogCommand");
            RapidData? rdLinearJogStep = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "LinearJogStep");
            if (rdJogCommand == null || rdLinearJogStep == null) return;

            // 写入Rapid数据
            // 注意：字符串类型的Rapid数据写入时要加双引号！
            // 注意：常量类型的Rapid数据不能写入！
            rdJogCommand.StringValue = (string)e.Parameter;
            rdLinearJogStep.StringValue = linearJogStepTextBox.Text;

            // 设置程序指针到点动例行程序
            App.Robot.Controller?.Rapid.GetTask("T_ROB1").SetProgramPointer("MotionModule", "Jog");

            // 启动Rapid程序
            _ = App.Robot.Controller?.Rapid.Start(true);
        }

        private void RobotLinearJogCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.CachedStatus?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.CachedStatus?.IsMaster == true
                        && App.Robot.CachedStatus?.State == ControllerState.MotorsOn
                        && App.Robot.CachedStatus?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion
    }
}