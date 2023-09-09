using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using STT = System.Threading.Tasks;
using ABB.Robotics;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.MotionDomain;
using ABB.Robotics.Controllers.RapidDomain;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite
{
    public class Robot
    {
        public Controller? Controller { get; private set; } = null;
        public Mastership? Mastership { get; set; } = null;
        public RobotStatus? StatusCache { get; private set; } = null;
        public Timer StatusCacheRefreshTimer { get; private set; } = new();
        public List<RobotJoint> Joints { get; set; } = new();
        public bool IsStatusCacheRefreshing { get; private set; } = false;
        public RapidData? RdIsAttached { get; private set; } = null;

        public Robot()
        {
            // 不要用DispatcherTimer，因为同在UI线程，耗时操作影响性能
            // 用System.Timers.Timer，在独立线程中触发事件，执行耗时操作
            StatusCacheRefreshTimer.Elapsed += RefreshStatusCache;
        }

        private void SetPosition(JointTarget jointPosition)
        {
            Joints[0].Angle = jointPosition.RobAx.Rax_1;
            Joints[1].Angle = jointPosition.RobAx.Rax_2;
            Joints[2].Angle = jointPosition.RobAx.Rax_3;
            Joints[3].Angle = jointPosition.RobAx.Rax_4;
            Joints[4].Angle = jointPosition.RobAx.Rax_5;
            Joints[5].Angle = jointPosition.RobAx.Rax_6;

            // 注意变换矩阵的结合顺序，矩阵乘法没有交换律
            Joints[0].GlobalTransform = Joints[0].LocalTransform;
            Joints[1].GlobalTransform = Transform3DHelper.CombineTransform(Joints[1].LocalTransform,
                                                                           Joints[0].GlobalTransform);
            Joints[2].GlobalTransform = Transform3DHelper.CombineTransform(Joints[2].LocalTransform,
                                                                           Joints[1].GlobalTransform);
            Joints[3].GlobalTransform = Transform3DHelper.CombineTransform(Joints[3].LocalTransform,
                                                                           Joints[2].GlobalTransform);
            Joints[4].GlobalTransform = Transform3DHelper.CombineTransform(Joints[4].LocalTransform,
                                                                           Joints[3].GlobalTransform);
            Joints[5].GlobalTransform = Transform3DHelper.CombineTransform(Joints[5].LocalTransform,
                                                                           Joints[4].GlobalTransform);
        }

        private void RefreshStatusCache(object? sender, ElapsedEventArgs e)
        {
            // 防止重入
            if (IsStatusCacheRefreshing || Controller == null || StatusCache == null) return;
            IsStatusCacheRefreshing = true;

            // TODO 用事件刷新状态
            StatusCache.Name = Controller.Name;
            StatusCache.SystemId = Controller.SystemId;
            StatusCache.OperatingMode = Controller.OperatingMode;
            StatusCache.IsMaster = Controller.IsMaster;
            StatusCache.State = Controller.State;
            StatusCache.ExecutionStatus = Controller.Rapid.ExecutionStatus;
            StatusCache.JointPosition = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition();
            StatusCache.LinearPosition = Controller.MotionSystem.ActiveMechanicalUnit.GetPosition(CoordinateSystemType.World);
            StatusCache.SpeedRatio = Controller.MotionSystem.SpeedRatio;

            // 用Dispatcher.Invoke方法处理UI线程中的事务
            App.Current.Dispatcher.Invoke(() =>
            {
                // 在异步操作后，手动刷新命令可用性，注意只有在UI线程中调用才起作用
                CommandManager.InvalidateRequerySuggested();

                ((MainWindow)App.Current.MainWindow).ConnectedControllerName = StatusCache.Name;
                ((MainWindow)App.Current.MainWindow).robotJointJogPanel.JointPosition = StatusCache.JointPosition;
                ((MainWindow)App.Current.MainWindow).robotLinearJogPanel.LinearPosition = StatusCache.LinearPosition;
                ((MainWindow)App.Current.MainWindow).controllerStatusPanel.ControllerOperatingMode = StatusCache.OperatingMode;
                ((MainWindow)App.Current.MainWindow).controllerStatusPanel.IsMaster = StatusCache.IsMaster;
                ((MainWindow)App.Current.MainWindow).controllerStatusPanel.ControllerState = StatusCache.State;
                ((MainWindow)App.Current.MainWindow).controllerStatusPanel.ExecutionStatus = StatusCache.ExecutionStatus;
                ((MainWindow)App.Current.MainWindow).controllerStatusPanel.SpeedRatio = StatusCache.SpeedRatio;

                SetPosition(StatusCache.JointPosition);

                if (RdIsAttached != null) return;
                try
                {
                    RdIsAttached = Controller.Rapid.GetRapidData("T_ROB1", "ToolModule", "IsAttached");
                    RdIsAttached.ValueChanged += ((MainWindow)App.Current.MainWindow).scene3DViewerPanel.RdIsAttached_ValueChanged;
                }
                catch
                {
                    // 没有IsAttached变量
                }
            });

            IsStatusCacheRefreshing = false;
        }

        public async STT.Task ConnectController(ControllerInfo controllerInfo)
        {
            if (StatusCache != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(string.Format((string)App.Current.FindResource("ChangeControllerMessage"),
                                                                                  StatusCache.Name,
                                                                                  controllerInfo.ControllerName),
                                                                    "RobotStudioLite",
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Information,
                                                                    MessageBoxResult.Yes);
                if (messageBoxResult == MessageBoxResult.No) return;
                await DisconnectController();
            }

            bool isLoginSuccessful = false;

            // 控制器登录窗口
            do
            {
                ControllerLoginWindow controllerLoginWindow = new(controllerInfo.ControllerName);
                if (controllerLoginWindow.ShowDialog() != true) return;

                try
                {
                    Controller = Controller.Connect(controllerInfo, ConnectionType.Standalone);
                    Controller.Logon(controllerLoginWindow.UserInfo);
                    isLoginSuccessful = true;
                }
                catch (GenericControllerException)
                {
                    Controller?.Dispose();
                    Controller = null;
                    MessageBoxResult messageBoxResult = MessageBox.Show(string.Format((string)App.Current.FindResource("LoginControllerFailedMessage"),
                                                                                      controllerLoginWindow.UserInfo.Name,
                                                                                      controllerInfo.ControllerName),
                                                                        "RobotStudioLite",
                                                                        MessageBoxButton.OKCancel,
                                                                        MessageBoxImage.Warning,
                                                                        MessageBoxResult.OK);
                    if (messageBoxResult == MessageBoxResult.Cancel) return;
                }
            }
            while (!isLoginSuccessful);

            StatusCache = new();
            StatusCacheRefreshTimer.Start();
            ((MainWindow)App.Current.MainWindow).controllerStatusPanelToggleButton.Visibility = Visibility.Visible;

            // 初次先主动获取全部事件日志
            Controller!.EventLog.GetAllMessages(0).ForEach(m => ((MainWindow)App.Current.MainWindow).eventsPanel.EventLogMessages.Add(m));
            // 订阅事件日志
            Controller.EventLog.MessageWritten += ((MainWindow)App.Current.MainWindow).eventsPanel.EventLog_MessageWritten;
        }

        public async STT.Task DisconnectController()
        {
            // 退订事件日志
            Controller!.EventLog.MessageWritten -= ((MainWindow)App.Current.MainWindow).eventsPanel.EventLog_MessageWritten;
            ((MainWindow)App.Current.MainWindow).eventsPanel.EventLogMessages.Clear();

            // 定时器停止时如果有定时事件正在执行，等待定时事件结束后再释放资源
            StatusCacheRefreshTimer.Stop();
            while (IsStatusCacheRefreshing) await STT.Task.Delay(100);
            StatusCache = null;

            Mastership?.Release();
            Mastership?.Dispose();
            Mastership = null;

            if (RdIsAttached != null)
            {
                RdIsAttached.ValueChanged -= ((MainWindow)App.Current.MainWindow).scene3DViewerPanel.RdIsAttached_ValueChanged;
                RdIsAttached.Dispose();
                RdIsAttached = null;
            }

            Controller?.Logoff();
            Controller?.Dispose();
            Controller = null;

            // 由于延时的存在，命令可用性轮询会在状态更新前执行完，所以需要在状态更新后手动再次触发
            CommandManager.InvalidateRequerySuggested();

            ((MainWindow)App.Current.MainWindow).ConnectedControllerName = string.Empty;
            ((MainWindow)App.Current.MainWindow).robotJointJogPanel.JointPosition = null;
            ((MainWindow)App.Current.MainWindow).robotLinearJogPanel.LinearPosition = null;
            ((MainWindow)App.Current.MainWindow).controllerStatusPanelToggleButton.IsChecked = false;
            ((MainWindow)App.Current.MainWindow).controllerStatusPanelToggleButton.Visibility = Visibility.Hidden;

            SetPosition(new());
        }

        public Point3D CalculateForwardKinematics(List<double> angles)
        {
            // TODO
            return new();
        }

        public List<double> CalculateInverseKinematics(Point3D target)
        {
            // TODO
            return new();
        }
    }
}