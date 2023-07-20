using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for Scene3DViewer.xaml
    /// </summary>
    public partial class Scene3DViewer : UserControl
    {
        public Scene3DViewer()
        {
            InitializeComponent();
            LoadScene();
        }

        private void LoadScene()
        {
            string basePath = @".\Models\";
            List<string> fileNames = new() { "AdjustmentArea_Model.obj",
                                             "CalibrationDomino_Model.obj",
                                             "Domino_Model.obj",
                                             "LoadingArea_Model.obj",
                                             "OmniCore_Model.obj",
                                             "RobotBase_Model.obj",
                                             "RobotLink1_Model.obj",
                                             "RobotLink2_Model.obj",
                                             "RobotLink3_Model.obj",
                                             "RobotLink4_Model.obj",
                                             "RobotLink5_Model.obj",
                                             "RobotLink6_Model.obj",
                                             "Table_Model.obj",
                                             "UnloadingArea_Model.obj" };
            ModelImporter modelImporter = new();
            foreach (string fileName in fileNames)
            {
                Model3DGroup model3DGroup = modelImporter.Load(basePath + fileName);
                ModelVisual3D modelVisual3D = new() { Content = model3DGroup };
                sortingVisual3D.Children.Add(modelVisual3D);
            }
        }
    }
}