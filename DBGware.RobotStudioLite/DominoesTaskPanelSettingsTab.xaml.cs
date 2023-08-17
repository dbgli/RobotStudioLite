using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Win32;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for DominoesTaskPanelSettingsTab.xaml
    /// </summary>
    public partial class DominoesTaskPanelSettingsTab : UserControl
    {
        public Tray Tray { get; set; } = new() { Rows = 5, Columns = 6, RowSpacing = 65, ColumnSpacing = 35 };
        public DominoSize DominoSize { get; set; } = new() { Length = 22, Width = 10, Height = 54 };
        public DominoPosition PushPosition { get; set; } = new();
        public ObservableCollection<Domino> Dominoes { get; set; } = new();

        public DominoesTaskPanelSettingsTab()
        {
            InitializeComponent();
        }

        #region 加载预设命令

        private void LoadPresetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Presets\",
                Filter = (string)App.Current.FindResource("SaveAsPresetDialogFilter")
            };
            if (openFileDialog.ShowDialog() != true) return;

            try
            {
                XDocument preset = XDocument.Load(openFileDialog.FileName);

                XElement tray = preset.Root!.Element("Tray")!;
                Tray.Rows = int.Parse(tray.Attribute("Rows")!.Value);
                Tray.Columns = int.Parse(tray.Attribute("Columns")!.Value);
                Tray.RowSpacing = double.Parse(tray.Attribute("RowSpacing")!.Value);
                Tray.ColumnSpacing = double.Parse(tray.Attribute("ColumnSpacing")!.Value);

                XElement dominoSize = preset.Root!.Element("DominoSize")!;
                DominoSize.Length = double.Parse(dominoSize.Attribute("Length")!.Value);
                DominoSize.Width = double.Parse(dominoSize.Attribute("Width")!.Value);
                DominoSize.Height = double.Parse(dominoSize.Attribute("Height")!.Value);

                XElement pushPosition = preset.Root!.Element("PushPosition")!;
                PushPosition.X = double.Parse(pushPosition.Attribute("X")!.Value);
                PushPosition.Y = double.Parse(pushPosition.Attribute("Y")!.Value);
                PushPosition.Z = double.Parse(pushPosition.Attribute("Z")!.Value);
                PushPosition.R = double.Parse(pushPosition.Attribute("R")!.Value);

                XElement dominoes = preset.Root!.Element("Dominoes")!;
                Dominoes.Clear();
                foreach (XElement domino in dominoes.Elements())
                {
                    XElement position = domino.Element("Position")!;
                    Dominoes.Add(new()
                    {
                        ID = int.Parse(domino.Attribute("ID")!.Value),
                        Position = new()
                        {
                            X = double.Parse(position.Attribute("X")!.Value),
                            Y = double.Parse(position.Attribute("Y")!.Value),
                            Z = double.Parse(position.Attribute("Z")!.Value),
                            R = double.Parse(position.Attribute("R")!.Value)
                        }
                    });
                }
            }
            catch
            {
                MessageBox.Show((string)App.Current.FindResource("LoadPresetFailedMessage"),
                                "RobotStudioLite",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning,
                                MessageBoxResult.OK);
            }
        }

        private void LoadPresetCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 保存为预设命令

        private void SaveAsPresetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Presets\",
                Filter = (string)App.Current.FindResource("SaveAsPresetDialogFilter"),
                DefaultExt = ".rslps"
            };
            if (saveFileDialog.ShowDialog() != true) return;

            XDocument preset = new(new XDeclaration("1.0", "utf-8", "yes"),
                                   new XElement("DominoesTaskSettings",
                                       new XElement("Tray",
                                           new XAttribute("Rows", Tray.Rows),
                                           new XAttribute("Columns", Tray.Columns),
                                           new XAttribute("RowSpacing", Tray.RowSpacing),
                                           new XAttribute("ColumnSpacing", Tray.ColumnSpacing)),
                                       new XElement("DominoSize",
                                           new XAttribute("Length", DominoSize.Length),
                                           new XAttribute("Width", DominoSize.Width),
                                           new XAttribute("Height", DominoSize.Height)),
                                       new XElement("PushPosition",
                                           new XAttribute("X", PushPosition.X),
                                           new XAttribute("Y", PushPosition.Y),
                                           new XAttribute("Z", PushPosition.Z),
                                           new XAttribute("R", PushPosition.R)),
                                       new XElement("Dominoes")));
            XElement dominoes = preset.Root!.Element("Dominoes")!;
            foreach (Domino domino in Dominoes)
            {
                dominoes.Add(new XElement("Domino",
                                 new XAttribute("ID", domino.ID),
                                 new XElement("Position",
                                     new XAttribute("X", domino.Position.X),
                                     new XAttribute("Y", domino.Position.Y),
                                     new XAttribute("Z", domino.Position.Z),
                                     new XAttribute("R", domino.Position.R))));
            }
            preset.Save(saveFileDialog.FileName);
        }

        private void SaveAsPresetCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 添加骨牌命令

        private void AddDominoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Dominoes.Add(new());
        }

        private void AddDominoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Dominoes.Count < 10;
        }

        #endregion

        #region 删除骨牌命令

        private void DeleteDominoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Domino selectedDomino = (Domino)e.Parameter;
            Dominoes.Remove(selectedDomino);
            // 行号不会自动更新，删除后手动刷新
            CollectionViewSource.GetDefaultView(dominoListView.ItemsSource).Refresh();
        }

        private void DeleteDominoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is Domino;
        }

        #endregion

        #region 上移骨牌命令

        private void MoveDominoUpCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            int selectedIndex = (int)e.Parameter;
            (Dominoes[selectedIndex], Dominoes[selectedIndex - 1]) = (Dominoes[selectedIndex - 1], Dominoes[selectedIndex]);
            dominoListView.SelectedIndex = selectedIndex - 1;
        }

        private void MoveDominoUpCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is int selectedIndex
                           && selectedIndex != -1
                           && selectedIndex != 0;
        }

        #endregion

        #region 下移骨牌命令

        private void MoveDominoDownCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            int selectedIndex = (int)e.Parameter;
            (Dominoes[selectedIndex], Dominoes[selectedIndex + 1]) = (Dominoes[selectedIndex + 1], Dominoes[selectedIndex]);
            dominoListView.SelectedIndex = selectedIndex + 1;
        }

        private void MoveDominoDownCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is int selectedIndex
                           && selectedIndex != -1
                           && selectedIndex != Dominoes.Count - 1;
        }

        #endregion
    }
}