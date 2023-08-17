using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Win32;
using ABB.Robotics.Controllers.FileSystemDomain;
using ABB.Robotics.Controllers.RapidDomain;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for RapidEditor.xaml
    /// </summary>
    public partial class RapidEditor : UserControl
    {
        private string programFilePath = string.Empty;

        public ObservableCollection<TreeViewItem> ProgramModuleItems { get; set; } = new();
        public ObservableCollection<TabItem> ProgramModuleTabs { get; set; } = new();

        public RapidEditor()
        {
            InitializeComponent();
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

        #region 加载程序命令

        private void LoadProgramCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ProgramModuleItems.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show((string)App.Current.FindResource("ProgramModuleExistsMessage"),
                                                                    "RobotStudioLite",
                                                                    MessageBoxButton.YesNo,
                                                                    MessageBoxImage.Information,
                                                                    MessageBoxResult.Yes);
                if (messageBoxResult != MessageBoxResult.Yes) return;
            }

            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"RapidPrograms\",
                Filter = (string)App.Current.FindResource("LoadProgramDialogFilter")
            };
            if (openFileDialog.ShowDialog() != true) return;
            programFilePath = openFileDialog.FileName;

            ProgramModuleItems.Clear();
            ProgramModuleTabs.Clear();

            try
            {
                XDocument rapidProgramModules = XDocument.Load(programFilePath);
                foreach (XElement module in rapidProgramModules.Root!.Elements())
                {
                    string moduleName = module.Value;

                    // Header部分
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
                programFilePath = string.Empty;
                MessageBox.Show((string)App.Current.FindResource("LoadProgramFailedMessage"),
                                "RobotStudioLite",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning,
                                MessageBoxResult.OK);
            }
        }

        private void LoadProgramCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 同步到机器人命令

        private void SynchronizeToRobotCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string localDirectory = Path.GetDirectoryName(programFilePath)!;
            string remoteDirectory = $"RapidPrograms/{Path.GetFileNameWithoutExtension(programFilePath)}";
            string programFileName = Path.GetFileName(programFilePath);

            App.Robot.Controller?.FileSystem.PutDirectory(localDirectory, remoteDirectory, true);
            App.Robot.Controller?.Rapid.GetTask("T_ROB1").LoadProgramFromFile($"{remoteDirectory}/{programFileName}", RapidLoadMode.Replace);
        }

        private void SynchronizeToRobotCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.IsMaster == true
                        && App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running
                        && programFilePath != string.Empty;
        }

        #endregion

        #region 创建备份命令

        private void CreateBackupCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 打开文件夹对话框选择备份位置
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"RapidPrograms\"
            };
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            // 创建目录
            FileSystem fileSystem = App.Robot.Controller!.FileSystem;
            string folderName = Path.GetFileName(folderBrowserDialog.SelectedPath);
            if (!fileSystem.DirectoryExists($"RapidPrograms/{folderName}"))
            {
                try
                {
                    fileSystem.CreateDirectory($"RapidPrograms/{folderName}");
                }
                catch
                {
                    // 疑似PC SDK bug：如果目标文件夹名和某个父文件夹名从头开始有相同部分，则会去掉相同部分，导致路径错误，抛出异常，但文件夹已经正常创建，忽略异常可继续运行
                    // 例：在虚拟控制器下，RemoteDirectory目录默认为C:/ABC/DEFXYZ/GHI/Workstation/Virtual Controllers/CRB1100_OmniCore/HOME
                    // 此时若CreateDirectory("DEF")，则抛出异常：无法找到C:/ABCXYZ/GHI/Workstation/Virtual Controllers/CRB1100_OmniCore/HOME
                    // 环境：Windows 11 x64 C# 10.0 .Net6.0 WPF Debug AnyCPU
                    // 临时解决方案：try-catch住但不做任何处理
                }
            }

            // 创建pgf文件
            XDocument programFile = new(new XDeclaration("1.0", "iso-8859-1", null),
                                        new XElement("Program"));

            // 保存modx文件
            IEnumerable<Module> modules = App.Robot.Controller!.Rapid.GetTask("T_ROB1").GetModules().Where(m => !m.IsSystem);
            foreach (Module module in modules)
            {
                module.SaveToFile($"RapidPrograms/{folderName}");
                programFile.Root!.Add(new XElement("Module", $"{module.Name}.modx"));
            }

            // 下载modx文件到本地，似乎GetDirectory方法的overwrite参数不起作用
            if (Directory.GetDirectories(folderBrowserDialog.SelectedPath).Length > 0 || Directory.GetFiles(folderBrowserDialog.SelectedPath).Length > 0)
            {
                Directory.Delete(folderBrowserDialog.SelectedPath, true);
                Directory.CreateDirectory(folderBrowserDialog.SelectedPath);
            }
            fileSystem.GetDirectory($"RapidPrograms/{folderName}", folderBrowserDialog.SelectedPath, true);

            // 保存pgf文件
            string programFileFullName = @$"{folderBrowserDialog.SelectedPath}\{folderName}.pgf";
            programFile.Save(programFileFullName);

            // 上传pgf文件到控制器
            fileSystem.PutFile(programFileFullName, $"RapidPrograms/{folderName}/{folderName}.pgf", true);

            // 不用SaveProgramToFile方法是因为这个方法只能保存有名程序，而一般程序默认是无名的
            // 要想使用必须在RobotStudio里重命名程序赋予名称才行，PC SDK没有提供命名程序的接口
            // App.Robot.Controller?.Rapid.GetTask("T_ROB1").SaveProgramToFile($"RapidPrograms/{folderName}");
        }

        private void CreateBackupCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.ExecutionStatus == ExecutionStatus.Stopped;
        }

        #endregion

        private void ProgramModuleItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            string moduleFileName = ((TextBlock)((StackPanel)((TreeViewItem)sender).Header).Children[1]).Text;
            string moduleFilePath = @$"{Path.GetDirectoryName(programFilePath)}\{moduleFileName}.modx";

            TabItem? tabItem = ProgramModuleTabs.ToList().Find(t => t.Header.ToString() == moduleFileName);
            if (tabItem != null)
            {
                programModuleTabControl.SelectedItem = tabItem;
                return;
            }

            TabItem programModuleTab = new() { Header = moduleFileName, Content = new RapidModuleTab(moduleFilePath) };
            ProgramModuleTabs.Add(programModuleTab);
            programModuleTabControl.SelectedItem = programModuleTab;
        }
    }
}