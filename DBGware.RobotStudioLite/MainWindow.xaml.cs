using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

using AvalonDock.Layout;
using DBGware.RobotStudioLite.UI;
using DBGware.RobotStudioLite.UI.Controls;
using DBGware.RobotStudioLite.UI.Themes.AvalonDock;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : CustomChromeWindow
    {
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
    }
}