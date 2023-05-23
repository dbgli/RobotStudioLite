using System.Windows;
using System.Windows.Controls;

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

        private void ThemeItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                Application.Current.Resources.MergedDictionaries[0].Source = ControlThemeSelector.Themes[int.Parse((string)menuItem.Tag)];
                Application.Current.Resources.MergedDictionaries[1].Source = AvalonDockThemeSelector.Themes[int.Parse((string)menuItem.Tag)];
            }
        }
    }
}