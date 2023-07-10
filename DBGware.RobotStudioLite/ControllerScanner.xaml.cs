﻿using System;
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
using ABB.Robotics.Controllers.Discovery;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for ControllerScanner.xaml
    /// </summary>
    public partial class ControllerScanner : UserControl
    {
        public ControllerScanner()
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

        private void ConnectControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            ConnectController(controllerInfoWrapper.ControllerInfo);
        }

        private void ConnectControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            e.CanExecute = controllerInfoWrapper.SystemId != App.Controller?.SystemId;
        }

        private void ConnectControllerMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem listViewItem) return;
            if (listViewItem.Content is not ControllerInfoWrapper controllerInfoWrapper) return;
            if (controllerInfoWrapper.SystemId == App.Controller?.SystemId) return;
            ConnectController(controllerInfoWrapper.ControllerInfo);
        }

        private static void ConnectController(ControllerInfo controllerInfo)
        {
            if (App.Controller != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(string.Format((string)App.Current.FindResource("ChangeControllerMessage"),
                                                                                  App.Controller.Name,
                                                                                  controllerInfo.ControllerName),
                                                                    (string)App.Current.FindResource("ChangeControllerCaption"),
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Information,
                                                                    MessageBoxResult.Yes);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    App.Controller.Logoff();
                    App.Controller.Dispose();
                    App.Controller = null;
                }
                else
                {
                    return;
                }
            }
            App.Controller = Controller.Connect(controllerInfo, ConnectionType.Standalone);
            App.Controller.Logon(UserInfo.DefaultUser);
            ((MainWindow)App.Current.MainWindow).ConnectedControllerName = App.Controller.Name;
        }

        #endregion

        #region 断开控制器命令

        private void DisconnectControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (App.Controller != null)
            {
                App.Controller.Logoff();
                App.Controller.Dispose();
                App.Controller = null;
                ((MainWindow)App.Current.MainWindow).ConnectedControllerName = string.Empty;
            }
        }

        private void DisconnectControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is not ControllerInfoWrapper controllerInfoWrapper) return;
            e.CanExecute = controllerInfoWrapper.SystemId == App.Controller?.SystemId;
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