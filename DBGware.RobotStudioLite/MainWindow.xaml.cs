using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using AvalonDock.Layout;
using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomChromeWindow
    {
        public string ConnectedControllerName
        {
            get => connectedControllerNameTextBlock.Text;
            set
            {
                connectedControllerNameTextBlock.Text = value;
                connectedControllerNameTextBlock.Visibility = string.IsNullOrEmpty(value) ?
                                                              Visibility.Hidden : Visibility.Visible;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CustomChromeWindow_Activated(object sender, EventArgs e)
        {
            // 当窗口被激活时，激活DockingManager中最后被激活的项
            bool hasFloatingWindow = false;

            // TODO
            List<LayoutContent> items = dockingManager.Layout.Descendents().OfType<LayoutContent>().ToList();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item.IsFloating)
                {
                    if (!hasFloatingWindow)
                        hasFloatingWindow = true;
                    items.RemoveAt(i--);
                }
            }

            if (hasFloatingWindow && items.Count > 0)
            {
                var index = 0;

                var tmpTimeStamp = items[0].LastActivationTimeStamp;
                for (var j = 1; j < items.Count; j++)
                {
                    var item2 = items[j];
                    if (item2.LastActivationTimeStamp > tmpTimeStamp)
                    {
                        tmpTimeStamp = item2.LastActivationTimeStamp;
                        index = j;
                    }
                }

                items[index].IsActive = true;
            }
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && !menuItem.IsChecked)
            {
                languageMenuItem.Items.OfType<MenuItem>().ToList()
                                .ForEach(item => { if (item.IsChecked) { item.IsChecked = false; } });   // 复位
                menuItem.IsChecked = true;
                App.CurrentLanguage = (string)menuItem.Tag;

                // DynamicResource不动态，在这里手动修改
                scene3DViewerPanelLayoutDocument.Title = (string)App.Current.FindResource("Scene3DViewer");
                controllerScannerPanelLayoutDocument.Title = (string)App.Current.FindResource("ControllerScanner");
                robotControlPanelLayoutAnchorable.Title = (string)App.Current.FindResource("RobotControlPanel");
            }
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && !menuItem.IsChecked)
            {
                themeMenuItem.Items.OfType<MenuItem>().ToList()
                             .ForEach(item => { if (item.IsChecked) { item.IsChecked = false; } });   // 复位
                menuItem.IsChecked = true;
                App.CurrentTheme = (string)menuItem.Tag;
            }
        }

        private void CustomChromeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            languageMenuItem.Items.OfType<MenuItem>().ToList()
                            .FindAll(item => (string)item.Tag == App.CurrentLanguage)
                            .ForEach(item => item.IsChecked = true);

            themeMenuItem.Items.OfType<MenuItem>().ToList()
                         .FindAll(item => (string)item.Tag == App.CurrentTheme)
                         .ForEach(item => item.IsChecked = true);
        }

        private void CustomChromeWindow_Closing(object sender, CancelEventArgs e)
        {
            if (App.Robot.StatusCache == null) return;
            e.Cancel = true;
            MessageBoxResult messageBoxResult = MessageBox.Show(string.Format((string)App.Current.FindResource("CloseApplicationMessage"),
                                                                              App.Robot.StatusCache.Name),
                                                                "RobotStudioLite",
                                                                MessageBoxButton.YesNo,
                                                                MessageBoxImage.Information,
                                                                MessageBoxResult.Yes);
            if (messageBoxResult == MessageBoxResult.No) return;
            DisconnectAndClose();
        }

        private async void DisconnectAndClose()
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

            // 等待上一次Closing事件处理完成后再次关闭窗口，此时已断开控制器可正常关闭
            // 不加延时可能会在窗口关闭期间调用Close方法引发异常
            await Task.Delay(100);
            Close();
        }
    }
}