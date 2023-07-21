using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using DBGware.RobotStudioLite.Properties;
using ABB.Robotics.Controllers;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string currentTheme = string.Empty;
        private static string currentLanguage = string.Empty;
        private static readonly List<string> supportedThemes = new() { "Blue", "Dark", "Light" };
        private static readonly List<string> supportedLanguages = new() { "de", "en", "es", "fr", "it", "ja", "ko", "pt", "ru", "th", "vi", "zh-CHS", "zh-CHT" };

        public static string CurrentTheme
        {
            get => currentTheme;
            set
            {
                currentTheme = supportedThemes.Contains(value) ? value : "Dark";
                Current.Resources.MergedDictionaries[0].Source = new Uri($"/DBGware.RobotStudioLite.UI;Component/Themes/{currentTheme}.xaml", UriKind.Relative);
                Current.Resources.MergedDictionaries[1].Source = new Uri($"/DBGware.RobotStudioLite.UI.Themes.AvalonDock;Component/Themes/{currentTheme}.xaml", UriKind.Relative);
            }
        }

        public static string CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                currentLanguage = supportedLanguages.Contains(value) ? value : "en";
                Current.Resources.MergedDictionaries[2].Source = new Uri($"/DBGware.RobotStudioLite.Localization;Component/Resources/{currentLanguage}.xaml", UriKind.Relative);
            }
        }

        public static List<string> SupportedThemes => supportedThemes;
        public static List<string> SupportedLanguages => supportedLanguages;

        public static Robot Robot { get; set; } = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // 这里必须用属性，因为要用到set访问器
                CurrentTheme = Settings.Default.Theme;
                CurrentLanguage = Settings.Default.Language;
            }
            catch (Exception)
            {
                // 文件损坏
                throw;
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Settings.Default.Theme = currentTheme;
                Settings.Default.Language = currentLanguage;
                Settings.Default.Save();
            }
            catch (Exception)
            {
                // 文件损坏
                throw;
            }
        }
    }
}