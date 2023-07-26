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
    /// Interaction logic for RobotJointJogPanel.xaml
    /// </summary>
    public partial class RobotJointJogPanel : UserControl
    {
        private List<double> jointValues = new();

        public List<double> JointValues
        {
            get => jointValues;
            set
            {
                jointValues = value;

                joint1ValueTextBox.Text = jointValues[0].ToString("F3");
                joint2ValueTextBox.Text = jointValues[1].ToString("F3");
                joint3ValueTextBox.Text = jointValues[2].ToString("F3");
                joint4ValueTextBox.Text = jointValues[3].ToString("F3");
                joint5ValueTextBox.Text = jointValues[4].ToString("F3");
                joint6ValueTextBox.Text = jointValues[5].ToString("F3");
            }
        }

        public RobotJointJogPanel()
        {
            InitializeComponent();
            JointValues = new() { 0, 0, 0, 0, 0, 0 };
        }

        #region 机器人关节点动命令

        private void RobotJointJogCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 访问Rapid数据
            RapidData? rdJogCommand = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JogCommand");
            RapidData? rdJointJogStep = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JointJogStep");
            if (rdJogCommand == null || rdJointJogStep == null) return;

            // 写入Rapid数据
            // 注意：字符串类型的Rapid数据写入时要加双引号！
            // 注意：常量类型的Rapid数据不能写入！
            rdJogCommand.StringValue = (string)e.Parameter;
            rdJointJogStep.StringValue = jointJogStepTextBox.Text;

            // 设置程序指针到点动例行程序
            App.Robot.Controller?.Rapid.GetTask("T_ROB1").SetProgramPointer("MotionModule", "Jog");

            // 启动Rapid程序
            _ = App.Robot.Controller?.Rapid.Start(true);
        }

        private void RobotJointJogCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.Controller?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.Controller?.IsMaster == true
                        && App.Robot.Controller?.State == ControllerState.MotorsOn
                        && App.Robot.Controller?.Rapid.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion
    }
}