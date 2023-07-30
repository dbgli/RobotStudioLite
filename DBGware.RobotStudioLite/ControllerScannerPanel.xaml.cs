using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for ControllerScannerPanel.xaml
    /// </summary>
    public partial class ControllerScannerPanel : UserControl
    {
        public ControllerScannerPanel()
        {
            InitializeComponent();
        }

        #region 刷新控制器列表命令

        private void RefreshControllerListCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            NetworkScanner networkScanner = new();
            networkScanner.Scan();
            List<ControllerInfoWrapper> controllerInfoWrappers = new();
            foreach (ControllerInfo controllerInfo in networkScanner.Controllers.Where(c => c.Availability == Availability.Available))
            {
                controllerInfoWrappers.Add(new ControllerInfoWrapper(controllerInfo));
            }
            controllerListView.ItemsSource = controllerInfoWrappers;
        }

        private void RefreshControllerListCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 连接控制器命令

        private async void ConnectControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            await ConnectController(controllerInfoWrapper.ControllerInfo);
        }

        private void ConnectControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            e.CanExecute = controllerInfoWrapper.SystemId != App.Robot.StatusCache?.SystemId;
        }

        private async void ConnectControllerMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ControllerInfoWrapper controllerInfoWrapper) return;
            if (controllerInfoWrapper.SystemId == App.Robot.StatusCache?.SystemId) return;
            await ConnectController(controllerInfoWrapper.ControllerInfo);
        }

        private static async Task ConnectController(ControllerInfo controllerInfo)
        {
            if (App.Robot.StatusCache != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(string.Format((string)App.Current.FindResource("ChangeControllerMessage"),
                                                                                  App.Robot.StatusCache.Name,
                                                                                  controllerInfo.ControllerName),
                                                                    "RobotStudioLite",
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Information,
                                                                    MessageBoxResult.Yes);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    await DisconnectController();
                }
                else
                {
                    return;
                }
            }
            App.Robot.Controller = Controller.Connect(controllerInfo, ConnectionType.Standalone);
            App.Robot.Controller.Logon(UserInfo.DefaultUser);
            ((MainWindow)App.Current.MainWindow).ConnectedControllerName = App.Robot.Controller.Name;
            App.Robot.StatusCache = new();
            App.Robot.StatusCacheRefreshTimer.Start();
        }

        #endregion

        #region 断开控制器命令

        private async void DisconnectControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await DisconnectController();
        }

        private void DisconnectControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            e.CanExecute = controllerInfoWrapper.SystemId == App.Robot.StatusCache?.SystemId;
        }

        private static async Task DisconnectController()
        {
            // 定时器停止时如果有定时事件正在执行，等待定时事件结束后再释放资源
            App.Robot.StatusCacheRefreshTimer.Stop();
            while (App.Robot.IsStatusCacheRefreshing) await Task.Delay(100);
            App.Robot.StatusCache = null;

            App.Robot.Mastership?.Release();
            App.Robot.Mastership?.Dispose();
            App.Robot.Mastership = null;

            App.Robot.Controller?.Logoff();
            App.Robot.Controller?.Dispose();
            App.Robot.Controller = null;

            // 由于延时的存在，命令可用性轮询会在状态更新前执行完，所以需要在状态更新后手动再次触发
            CommandManager.InvalidateRequerySuggested();

            ((MainWindow)App.Current.MainWindow).ConnectedControllerName = string.Empty;
            ((MainWindow)App.Current.MainWindow).robotJointJogPanel.JointPosition = null;
            ((MainWindow)App.Current.MainWindow).robotLinearJogPanel.LinearPosition = null;
        }

        #endregion

        #region 复制控制器信息命令

        private void CopyControllerInfoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            Clipboard.SetText($"{controllerInfoWrapper.DisplayName}\t" +
                              $"{controllerInfoWrapper.DisplayLocation}\t" +
                              $"{controllerInfoWrapper.IPAddress}\t" +
                              $"{controllerInfoWrapper.Version}\t" +
                              $"{controllerInfoWrapper.IsVirtual}");
            Clipboard.Flush();
        }

        private void CopyControllerInfoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 显示控制器详情命令

        private void ShowControllerDetailsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            ControllerDetailsWindow controllerDetailsWindow = new(controllerInfoWrapper.ToString());
            controllerDetailsWindow.Show();
        }

        private void ShowControllerDetailsCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}