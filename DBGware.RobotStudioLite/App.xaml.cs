using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using DBGware.RobotStudioLite.Properties;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        private string currentTheme = string.Empty;
        private static string currentLanguage = string.Empty;
        private static readonly List<string> supportedThemes = new() { "Blue", "Dark", "Light" };
        private static readonly List<string> supportedLanguages = new() { "de", "en", "es", "fr", "it", "ja", "ko", "pt", "ru", "th", "vi", "zh-CHS", "zh-CHT" };

        public string CurrentTheme
        {
            get => currentTheme;
            set
            {
                if (currentTheme == value) return;
                currentTheme = supportedThemes.Contains(value) ? value : "Dark";
                Current.Resources.MergedDictionaries[0].Source = new Uri($"/DBGware.RobotStudioLite.UI;Component/Themes/{currentTheme}.xaml", UriKind.Relative);
                Current.Resources.MergedDictionaries[1].Source = new Uri($"/DBGware.RobotStudioLite.UI.Themes.AvalonDock;Component/Themes/{currentTheme}.xaml", UriKind.Relative);
                OnPropertyChanged(nameof(CurrentTheme));
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public App()
        {
            using Stream streamLight = typeof(MainWindow).Assembly.GetManifestResourceStream("DBGware.RobotStudioLite.HighlightingDefinitions.RapidLightHighlighting.xshd")!;
            using XmlReader xmlReaderLight = new XmlTextReader(streamLight);
            IHighlightingDefinition rapidLightHighlighting = HighlightingLoader.Load(xmlReaderLight, HighlightingManager.Instance);
            HighlightingManager.Instance.RegisterHighlighting("RapidLight", new[] { ".mod", ".modx" }, rapidLightHighlighting);

            using Stream streamDark = typeof(MainWindow).Assembly.GetManifestResourceStream("DBGware.RobotStudioLite.HighlightingDefinitions.RapidDarkHighlighting.xshd")!;
            using XmlReader xmlReaderDark = new XmlTextReader(streamDark);
            IHighlightingDefinition rapidDarkHighlighting = HighlightingLoader.Load(xmlReaderDark, HighlightingManager.Instance);
            HighlightingManager.Instance.RegisterHighlighting("RapidDark", new[] { ".mod", ".modx" }, rapidDarkHighlighting);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }

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