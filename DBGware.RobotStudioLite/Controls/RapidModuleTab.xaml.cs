using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit.Highlighting;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for RapidModuleTab.xaml
    /// </summary>
    public partial class RapidModuleTab : UserControl
    {
        public RapidModuleTab(string filePath)
        {
            InitializeComponent();

            // 设置初始高亮
            textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(((App)App.Current).CurrentTheme == "Dark" ? "RapidDark" : "RapidLight");
            ((App)App.Current).PropertyChanged += AppThemeChanged;

            // 读取文件
            using StreamReader streamReader = new(filePath);
            textEditor.Text = streamReader.ReadToEnd();
        }

        private void AppThemeChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CurrentTheme") return;
            textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(((App)App.Current).CurrentTheme == "Dark" ? "RapidDark" : "RapidLight");
        }
    }
}