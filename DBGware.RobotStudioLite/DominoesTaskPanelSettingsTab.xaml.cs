using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for DominoesTaskPanelSettingsTab.xaml
    /// </summary>
    public partial class DominoesTaskPanelSettingsTab : UserControl
    {
        public Tray Tray { get; set; } = new() { Rows = 5, Columns = 6, RowSpacing = 65, ColumnSpacing = 35 };
        public DominoSize DominoSize { get; set; } = new() { Length = 22, Width = 10, Height = 54 };
        public ObservableCollection<Domino> Dominoes { get; set; } = new();

        public DominoesTaskPanelSettingsTab()
        {
            InitializeComponent();
        }

        #region 加载预设命令

        private void LoadPresetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void LoadPresetCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 保存为预设命令

        private void SaveAsPresetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

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