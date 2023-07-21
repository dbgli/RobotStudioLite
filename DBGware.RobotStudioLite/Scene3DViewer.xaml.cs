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
            LoadEnvironment();
            LoadRobot();

            List<double> angles = new() { 0, 0, 0, 0, 90, 0 };
            App.Robot.CalculateForwardKinematics(angles);
        }

        private void LoadEnvironment()
        {
            string basePath = @".\Models\";
            List<string> fileNames = new() { "AdjustmentArea_Model.obj",
                                             "CalibrationDomino_Model.obj",
                                             "Domino_Model.obj",
                                             "LoadingArea_Model.obj",
                                             "OmniCore_Model.obj",
                                             "RobotBase_Model.obj",
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

        private void LoadRobot()
        {
            string basePath = @".\Models\";
            List<string> fileNames = new() { "RobotLink1_Model.obj",
                                             "RobotLink2_Model.obj",
                                             "RobotLink3_Model.obj",
                                             "RobotLink4_Model.obj",
                                             "RobotLink5_Model.obj",
                                             "RobotLink6_Model.obj" };
            ModelImporter modelImporter = new();
            foreach (string fileName in fileNames)
            {
                Model3DGroup link = modelImporter.Load(basePath + fileName);
                App.Robot.Joints.Add(new(link));
                ModelVisual3D linkVisual3D = new() { Content = link };
                sortingVisual3D.Children.Add(linkVisual3D);
            }

            // 旋转轴如果是X轴，要取反才能与ABB旋转正方向一致
            App.Robot.Joints[0].MinAngle = -230;
            App.Robot.Joints[0].MaxAngle = 230;
            App.Robot.Joints[0].RotationAxis = new(0, 0, 1);
            App.Robot.Joints[0].RotationPoint = new(-310, -180, 169);

            App.Robot.Joints[1].MinAngle = -115;
            App.Robot.Joints[1].MaxAngle = 113;
            App.Robot.Joints[1].RotationAxis = new(-1, 0, 0);
            App.Robot.Joints[1].RotationPoint = new(-274, -180, 337);

            App.Robot.Joints[2].MinAngle = -205;
            App.Robot.Joints[2].MaxAngle = 55;
            App.Robot.Joints[2].RotationAxis = new(-1, 0, 0);
            App.Robot.Joints[2].RotationPoint = new(-269, -180, 617);

            App.Robot.Joints[3].MinAngle = -230;
            App.Robot.Joints[3].MaxAngle = 230;
            App.Robot.Joints[3].RotationAxis = new(0, 1, 0);
            App.Robot.Joints[3].RotationPoint = new(-310, -103, 627);

            App.Robot.Joints[4].MinAngle = -125;
            App.Robot.Joints[4].MaxAngle = 120;
            App.Robot.Joints[4].RotationAxis = new(-1, 0, 0);
            App.Robot.Joints[4].RotationPoint = new(-286.5, 120, 627);

            App.Robot.Joints[5].MinAngle = -400;
            App.Robot.Joints[5].MaxAngle = 400;
            App.Robot.Joints[5].RotationAxis = new(0, 1, 0);
            App.Robot.Joints[5].RotationPoint = new(-310, 176, 627);
        }
    }
}