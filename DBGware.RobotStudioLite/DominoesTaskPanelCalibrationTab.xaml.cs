using System.Windows.Controls;
using System.Windows.Input;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for DominoesTaskPanelCalibrationTab.xaml
    /// </summary>
    public partial class DominoesTaskPanelCalibrationTab : UserControl
    {
        public RobTarget X1 { get; set; }
        public RobTarget X2 { get; set; }
        public RobTarget Y1 { get; set; }

        public DominoesTaskPanelCalibrationTab()
        {
            InitializeComponent();
        }

        #region 记录当前位置到X1命令

        private void RecordCurrentPositionToX1CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            X1 = App.Robot.StatusCache!.LinearPosition;
            x1TextBox.Text = X1.ToString();
        }

        private void RecordCurrentPositionToX1CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache != null;
        }

        #endregion

        #region 记录当前位置到X2命令

        private void RecordCurrentPositionToX2CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            X2 = App.Robot.StatusCache!.LinearPosition;
            x2TextBox.Text = X2.ToString();
        }

        private void RecordCurrentPositionToX2CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache != null;
        }

        #endregion

        #region 记录当前位置到Y1命令

        private void RecordCurrentPositionToY1CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Y1 = App.Robot.StatusCache!.LinearPosition;
            y1TextBox.Text = Y1.ToString();
        }

        private void RecordCurrentPositionToY1CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache != null;
        }

        #endregion

        #region 计算框架命令

        private void CalculateFrameCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // 访问Rapid数据
                RapidData? rdX1 = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "CalibrationModule", "X1");
                RapidData? rdX2 = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "CalibrationModule", "X2");
                RapidData? rdY1 = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "CalibrationModule", "Y1");
                RapidData? rdCalibrationObject = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "CalibrationModule", "CalibrationObject");
                RapidData? rdFrame = App.Robot.Controller?.Rapid.GetRapidData("T_ROB1", "CalibrationModule", "Frame");
                if (rdX1 == null || rdX2 == null || rdY1 == null || rdFrame == null || rdCalibrationObject == null) return;

                // 订阅返回值的值更改事件，当该事件触发时表示已经计算完成
                try
                {
                    rdFrame.ValueChanged -= RdFrame_ValueChanged;
                    rdFrame.ValueChanged += RdFrame_ValueChanged;
                }
                catch
                {
                    // 初次没有订阅过时发生
                }

                // 写入Rapid数据
                rdX1.StringValue = x1TextBox.Text;
                rdX2.StringValue = x2TextBox.Text;
                rdY1.StringValue = y1TextBox.Text;
                rdCalibrationObject.StringValue = calibrationObjectComboBox.SelectedIndex.ToString();

                // 设置程序指针到校准例行程序
                App.Robot.Controller?.Rapid.GetTask("T_ROB1").SetProgramPointer("CalibrationModule", "Calibrate");

                // 启动Rapid程序
                _ = App.Robot.Controller?.Rapid.Start(true);
            }
            catch
            {
                // 确保先同步RAPID程序到机器人
            }
        }

        private void CalculateFrameCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.OperatingMode == ControllerOperatingMode.Auto
                        && App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.State == ControllerState.MotorsOn
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion

        private void RdFrame_ValueChanged(object? sender, DataValueChangedEventArgs e)
        {
            frameTextBox.Text = ((RapidData)sender!).StringValue;
        }
    }
}