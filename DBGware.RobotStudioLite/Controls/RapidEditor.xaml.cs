using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for RapidEditor.xaml
    /// </summary>
    public partial class RapidEditor : UserControl
    {
        public ObservableCollection<TreeViewItem> ProgramModuleItems { get; set; } = new();
        public ObservableCollection<TabItem> ProgramModuleTabs { get; set; } = new();

        public RapidEditor()
        {
            InitializeComponent();
            LoadProgram();
        }

        #region 关闭程序模块选项卡命令

        private void CloseProgramModuleTabCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ProgramModuleTabs.Remove((TabItem)e.Parameter);
        }

        private void CloseProgramModuleTabCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        private void LoadProgram()
        {
            try
            {
                XDocument rapidProgramModules = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + @"RapidProgramModules\RapidProgramModules.pgf");
                foreach (XElement module in rapidProgramModules.Root!.Elements())
                {
                    string moduleName = module.Value;

                    TextBlock iconTextBlock = new()
                    {
                        Text = "\uF000",
                        FontFamily = new("Segoe Fluent Icons,Segoe MDL2 Assets"),
                        Margin = new(0, 0, 4, 0),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    TextBlock headerTextBlock = new() { Text = Regex.Replace(moduleName, @"\.modx$", string.Empty) };
                    StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };
                    stackPanel.Children.Add(iconTextBlock);
                    stackPanel.Children.Add(headerTextBlock);

                    TreeViewItem programModuleItem = new() { Header = stackPanel, Tag = RapidItemType.ProgramModule };
                    programModuleItem.MouseDoubleClick += ProgramModuleItem_MouseDoubleClick;
                    ProgramModuleItems.Add(programModuleItem);
                }
            }
            catch
            {
                MessageBox.Show((string)App.Current.FindResource("LoadProgramFailedMessage"),
                                                                 "RobotStudioLite",
                                                                 MessageBoxButton.OK,
                                                                 MessageBoxImage.Warning,
                                                                 MessageBoxResult.OK);
            }
        }

        private void ProgramModuleItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            string fileName = ((TextBlock)((StackPanel)((TreeViewItem)sender).Header).Children[1]).Text;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"RapidProgramModules\" + fileName + ".modx";

            TabItem? tabItem = ProgramModuleTabs.ToList().Find(t => t.Header.ToString() == fileName);
            if (tabItem != null)
            {
                programModuleTabControl.SelectedItem = tabItem;
                return;
            }

            TabItem programModuleTab = new() { Header = fileName, Content = new RapidModuleTab(filePath) };
            ProgramModuleTabs.Add(programModuleTab);
            programModuleTabControl.SelectedItem = programModuleTab;
        }
    }
}