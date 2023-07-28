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
        private RobTarget linearPosition;

        public RobTarget LinearPosition
        {
            get => linearPosition;
            set
            {
                linearPosition = value;
                linearPosition.Rot.ToEulerAngles(out double rx, out double ry, out double rz);
                linear1ValueTextBox.Text = linearPosition.Trans.X.ToString("F3");
                linear2ValueTextBox.Text = linearPosition.Trans.Y.ToString("F3");
                linear3ValueTextBox.Text = linearPosition.Trans.Z.ToString("F3");
                linear4ValueTextBox.Text = rx.ToString("F3");
                linear5ValueTextBox.Text = ry.ToString("F3");
                linear6ValueTextBox.Text = rz.ToString("F3");
            }
        }

        public RobotLinearJogPanel()
        {
            InitializeComponent();
            LinearPosition = new();
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