using System.Collections.Generic;
using System.Linq;
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
            await App.Robot.ConnectController(controllerInfoWrapper.ControllerInfo);
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
            await App.Robot.ConnectController(controllerInfoWrapper.ControllerInfo);
        }

        #endregion

        #region 断开控制器命令

        private async void DisconnectControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            await App.Robot.DisconnectController();
        }

        private void DisconnectControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            e.CanExecute = controllerInfoWrapper.SystemId == App.Robot.StatusCache?.SystemId;
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