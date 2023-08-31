using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for ControllerStatusPanel.xaml
    /// </summary>
    public partial class ControllerStatusPanel : UserControl, INotifyPropertyChanged
    {
        private ControllerOperatingMode controllerOperatingMode;
        private ControllerState controllerState;
        private ExecutionStatus executionStatus;
        private int speedRatio;
        private bool isMaster;

        public ControllerOperatingMode ControllerOperatingMode
        {
            get => controllerOperatingMode;
            set
            {
                // 为了能够实时切换语言，暂时无条件引发属性更改事件，下同
                //if (controllerOperatingMode == value) return;
                controllerOperatingMode = value;
                OnPropertyChanged(nameof(ControllerOperatingMode));
            }
        }

        public ControllerState ControllerState
        {
            get => controllerState;
            set
            {
                //if (controllerState == value) return;
                controllerState = value;
                OnPropertyChanged(nameof(ControllerState));
            }
        }

        public ExecutionStatus ExecutionStatus
        {
            get => executionStatus;
            set
            {
                //if (executionStatus == value) return;
                executionStatus = value;
                OnPropertyChanged(nameof(ExecutionStatus));
            }
        }

        public int SpeedRatio
        {
            get => speedRatio;
            set
            {
                //if (speedRatio == value) return;
                speedRatio = value;
                OnPropertyChanged(nameof(SpeedRatio));
            }
        }

        public bool IsMaster
        {
            get => isMaster;
            set
            {
                //if (isMaster == value) return;
                isMaster = value;
                OnPropertyChanged(nameof(IsMaster));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ControllerStatusPanel()
        {
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

        #region 关闭控制器状态面板命令

        private void CloseControllerStatusPanelCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((MainWindow)App.Current.MainWindow).controllerStatusPanelToggleButton.IsChecked = false;
        }

        private void CloseControllerStatusPanelCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}