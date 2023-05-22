using System;
using System.Collections.Generic;

namespace DBGware.RobotStudioLite.UI.Themes.AvalonDock
{
    public class AvalonDockThemeSelector
    {
        public static List<Uri> Themes { get; } = new()
        {
            new Uri("/DBGware.RobotStudioLite.UI.Themes.AvalonDock;Component/Themes/BlueTheme.xaml", UriKind.Relative),
            new Uri("/DBGware.RobotStudioLite.UI.Themes.AvalonDock;Component/Themes/DarkTheme.xaml", UriKind.Relative),
            new Uri("/DBGware.RobotStudioLite.UI.Themes.AvalonDock;Component/Themes/LightTheme.xaml", UriKind.Relative)
        };
    }
}