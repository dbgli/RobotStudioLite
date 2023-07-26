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
        private List<double> jointAngles = new();

        public List<double> JointAngles
        {
            get => jointAngles;
            set
            {
                jointAngles = value;

                joint1AngleTextBox.Text = jointAngles[0].ToString("F3");
                joint2AngleTextBox.Text = jointAngles[1].ToString("F3");
                joint3AngleTextBox.Text = jointAngles[2].ToString("F3");
                joint4AngleTextBox.Text = jointAngles[3].ToString("F3");
                joint5AngleTextBox.Text = jointAngles[4].ToString("F3");
                joint6AngleTextBox.Text = jointAngles[5].ToString("F3");
            }
        }

        public RobotJointJogPanel()
        {
            InitializeComponent();
            JointAngles = new() { 0, 0, 0, 0, 0, 0 };
        }

        #region 机器人关节点动命令

        private void RobotJointJogCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 访问Rapid数据
            RapidData? rdJogCommand = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JogCommand");
            RapidData? rdJogStep = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JogStep");
            if (rdJogCommand == null || rdJogStep == null) return;

            // 写入Rapid数据
            // 注意：字符串类型的Rapid数据写入时要加双引号！
            // 注意：常量类型的Rapid数据不能写入！
            rdJogCommand.StringValue = (string)e.Parameter;
            rdJogStep.StringValue = jogStepTextBox.Text;

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