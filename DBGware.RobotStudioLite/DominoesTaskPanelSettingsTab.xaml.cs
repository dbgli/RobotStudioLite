using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for DominoesTaskPanelSettingsTab.xaml
    /// </summary>
    public partial class DominoesTaskPanelSettingsTab : UserControl
    {
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

        }

        private void AddDominoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 删除骨牌命令

        private void DeleteDominoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DeleteDominoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 上移骨牌命令

        private void MoveDominoUpCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void MoveDominoUpCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 下移骨牌命令

        private void MoveDominoDownCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void MoveDominoDownCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}