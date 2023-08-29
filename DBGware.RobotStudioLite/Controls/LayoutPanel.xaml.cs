using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for LayoutPanel.xaml
    /// </summary>
    public partial class LayoutPanel : UserControl
    {
        public List<ModelVisual3D> RobotModels { get; set; } = new();
        public List<ModelVisual3D> AdjustmentAreaModels { get; set; } = new();
        public List<ModelVisual3D> CalibrationDominoesModels { get; set; } = new();
        public List<ModelVisual3D> LoadingAreaModels { get; set; } = new();
        public List<ModelVisual3D> OmniCoreModels { get; set; } = new();
        public List<ModelVisual3D> TableModels { get; set; } = new();
        public List<ModelVisual3D> UnloadingAreaModels { get; set; } = new();
        public List<ModelVisual3D> DominoesModels { get; set; } = new();
        public List<ModelVisual3D> DominoesPreviewModels { get; set; } = new();
        public List<ModelVisual3D> DominoQueueOrderLabelsModels { get; set; } = new();

        public LayoutPanel()
        {
            InitializeComponent();
        }

        #region 切换模型可见性命令

        private void ToggleModelVisibilityCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MenuItem selectedMenuItem = (MenuItem)e.Parameter;
            List<ModelVisual3D> selectedModels = (List<ModelVisual3D>)selectedMenuItem.Tag;
            SortingVisual3D sortingVisual3D = ((MainWindow)App.Current.MainWindow).scene3DViewerPanel.sortingVisual3D;

            if (selectedMenuItem.IsChecked)
            {
                selectedModels.ForEach(m => sortingVisual3D.Children.Add(m));
            }
            else
            {
                selectedModels.ForEach(m => sortingVisual3D.Children.Remove(m));
            }
        }

        private void ToggleModelVisibilityCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}