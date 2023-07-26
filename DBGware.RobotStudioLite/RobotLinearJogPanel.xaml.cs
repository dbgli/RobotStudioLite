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
        private List<double> linearValues = new();

        public List<double> LinearValues
        {
            get => linearValues;
            set
            {
                linearValues = value;

                linear1ValueTextBox.Text = linearValues[0].ToString("F3");
                linear2ValueTextBox.Text = linearValues[1].ToString("F3");
                linear3ValueTextBox.Text = linearValues[2].ToString("F3");
                linear4ValueTextBox.Text = linearValues[3].ToString("F3");
                linear5ValueTextBox.Text = linearValues[4].ToString("F3");
                linear6ValueTextBox.Text = linearValues[5].ToString("F3");
            }
        }

        public RobotLinearJogPanel()
        {
            InitializeComponent();
            LinearValues = new() { 0, 0, 0, 0, 0, 0 };
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
            e.CanExecute = App.Robot.Controller?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.Controller?.IsMaster == true
                        && App.Robot.Controller?.State == ControllerState.MotorsOn
                        && App.Robot.Controller?.Rapid.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion
    }
}