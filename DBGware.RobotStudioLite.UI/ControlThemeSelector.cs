using System;
using System.Collections.Generic;

namespace DBGware.RobotStudioLite.UI
{
    public class ControlThemeSelector
    {
        public static List<Uri> Themes { get; } = new()
        {
            new Uri("/DBGware.RobotStudioLite.UI;Component/Themes/BlueTheme.xaml", UriKind.Relative),
            new Uri("/DBGware.RobotStudioLite.UI;Component/Themes/DarkTheme.xaml", UriKind.Relative),
            new Uri("/DBGware.RobotStudioLite.UI;Component/Themes/LightTheme.xaml", UriKind.Relative)
        };
    }
}