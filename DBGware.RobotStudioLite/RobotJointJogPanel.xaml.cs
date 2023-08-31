using System.Windows.Controls;
using System.Windows.Input;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for RobotJointJogPanel.xaml
    /// </summary>
    public partial class RobotJointJogPanel : UserControl
    {
        private JointTarget? jointPosition;

        public JointTarget? JointPosition
        {
            get => jointPosition;
            set
            {
                jointPosition = value;

                joint1ValueTextBox.Text = jointPosition?.RobAx.Rax_1.ToString("F3") ?? "N/A";
                joint2ValueTextBox.Text = jointPosition?.RobAx.Rax_2.ToString("F3") ?? "N/A";
                joint3ValueTextBox.Text = jointPosition?.RobAx.Rax_3.ToString("F3") ?? "N/A";
                joint4ValueTextBox.Text = jointPosition?.RobAx.Rax_4.ToString("F3") ?? "N/A";
                joint5ValueTextBox.Text = jointPosition?.RobAx.Rax_5.ToString("F3") ?? "N/A";
                joint6ValueTextBox.Text = jointPosition?.RobAx.Rax_6.ToString("F3") ?? "N/A";

                bool isJointPositionNonNull = jointPosition != null;
                joint1ValueTextBox.IsEnabled = isJointPositionNonNull;
                joint2ValueTextBox.IsEnabled = isJointPositionNonNull;
                joint3ValueTextBox.IsEnabled = isJointPositionNonNull;
                joint4ValueTextBox.IsEnabled = isJointPositionNonNull;
                joint5ValueTextBox.IsEnabled = isJointPositionNonNull;
                joint6ValueTextBox.IsEnabled = isJointPositionNonNull;
            }
        }
        public double JointJogStep { get; set; } = 1.0;

        public RobotJointJogPanel()
        {
            InitializeComponent();
            JointPosition = null;
        }

        #region 机器人关节点动命令

        private void RobotJointJogCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // 访问Rapid数据
                RapidData? rdJogCommand = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JogCommand");
                RapidData? rdJointJogStep = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "MotionModule", "JointJogStep");
                if (rdJogCommand == null || rdJointJogStep == null) return;

                // 写入Rapid数据
                // 注意：字符串类型的Rapid数据写入时要加双引号！
                // 注意：常量类型的Rapid数据不能写入！
                rdJogCommand.StringValue = (string)e.Parameter;
                rdJointJogStep.StringValue = JointJogStep.ToString();

                // 设置程序指针到点动例行程序
                App.Robot.Controller?.Rapid.GetTask("T_ROB1").SetProgramPointer("MotionModule", "Jog");

                // 启动Rapid程序
                _ = App.Robot.Controller?.Rapid.Start(true);
            }
            catch
            {
                // 请勿频繁操作，并确保先同步RAPID程序到机器人
            }
        }

        private void RobotJointJogCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.State == ControllerState.MotorsOn
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion
    }
}