using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ABB.Robotics.Controllers.RapidDomain;
using HelixToolkit.Wpf;
using DBGware.RobotStudioLite.Controls;
using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for Scene3DViewerPanel.xaml
    /// </summary>
    public partial class Scene3DViewerPanel : UserControl
    {
        /// <summary>
        /// 骨牌模型初始位姿到机器人末端TCP初始位姿的水平变换。
        /// </summary>
        private static readonly Transform3D attachedDominoModelOriginalTransformHorizontal
            = Transform3DHelper.CombineTransform(new RotateTransform3D(new AxisAngleRotation3D(new(1, 0, 0), 90), new(-351.49, 296.66, 34.11)),
                                                 new TranslateTransform3D(41.49, 11.34, 707.89));

        /// <summary>
        /// 骨牌模型初始位姿到机器人末端TCP初始位姿的垂直变换。
        /// </summary>
        private static readonly Transform3D attachedDominoModelOriginalTransformVertical
            = Transform3DHelper.CombineTransform(new RotateTransform3D(new AxisAngleRotation3D(new(0, 0, 1), 90), new(-351.49, 296.66, 34.11)),
                                                 attachedDominoModelOriginalTransformHorizontal);

        /// <summary>
        /// 指示当前拾取或放置是否为第一次，用于判断应用水平变换还是垂直变换。
        /// </summary>
        private bool isFirstPickOrPut = false;

        public List<ModelVisual3D> DominoModels { get; set; } = new();
        public List<ModelVisual3D> DominoPreviewModels { get; set; } = new();
        public List<BillboardTextVisual3D> DominoQueueOrderLabels { get; set; } = new();
        public ModelVisual3D? AttachedDominoModel { get; set; } = null;

        public Scene3DViewerPanel()
        {
            InitializeComponent();
            LoadEnvironment();
            LoadRobot();
            LoadDominoes(new() { Rows = 5, Columns = 6, RowSpacing = 65, ColumnSpacing = 35 });
        }

        private void LoadEnvironment()
        {
            string basePath = @$"{AppDomain.CurrentDomain.BaseDirectory}Models\";
            List<string> fileNames = new() { "AdjustmentArea_Model.obj",
                                             "CalibrationDominoes_Model.obj",
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

                // 校准骨牌默认不显示
                if (fileName != "CalibrationDominoes_Model.obj")
                {
                    sortingVisual3D.Children.Add(modelVisual3D);
                }

                // 将模型记录在布局面板中
                LayoutPanel layoutPanel = ((MainWindow)App.Current.MainWindow).layoutPanel;
                switch (fileName)
                {
                    case "AdjustmentArea_Model.obj":
                        layoutPanel.AdjustmentAreaModels.Add(modelVisual3D);
                        break;
                    case "CalibrationDominoes_Model.obj":
                        layoutPanel.CalibrationDominoesModels.Add(modelVisual3D);
                        break;
                    case "LoadingArea_Model.obj":
                        layoutPanel.LoadingAreaModels.Add(modelVisual3D);
                        break;
                    case "OmniCore_Model.obj":
                        layoutPanel.OmniCoreModels.Add(modelVisual3D);
                        break;
                    case "RobotBase_Model.obj":
                        layoutPanel.RobotModels.Add(modelVisual3D);
                        break;
                    case "Table_Model.obj":
                        layoutPanel.TableModels.Add(modelVisual3D);
                        break;
                    case "UnloadingArea_Model.obj":
                        layoutPanel.UnloadingAreaModels.Add(modelVisual3D);
                        break;
                    default:
                        break;
                }
            }
        }

        private void LoadRobot()
        {
            string basePath = @$"{AppDomain.CurrentDomain.BaseDirectory}Models\";
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

                // 将模型记录在布局面板中
                LayoutPanel layoutPanel = ((MainWindow)App.Current.MainWindow).layoutPanel;
                layoutPanel.RobotModels.Add(linkVisual3D);
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

        private void LoadDominoes(Tray tray)
        {
            // 如果有的话，清空上一次的骨牌模型
            DominoModels.ForEach(d => sortingVisual3D.Children.Remove(d));
            DominoModels.Clear();
            // 将模型从布局面板中删除
            LayoutPanel layoutPanel = ((MainWindow)App.Current.MainWindow).layoutPanel;
            layoutPanel.DominoesModels.Clear();

            ModelImporter modelImporter = new();
            for (int row = 0; row < tray.Rows; row++)
            {
                for (int column = 0; column < tray.Columns; column++)
                {
                    Model3DGroup model3DGroup = modelImporter.Load(@$"{AppDomain.CurrentDomain.BaseDirectory}Models\Domino_Model.obj");
                    ModelVisual3D modelVisual3D = new()
                    {
                        Content = model3DGroup,
                        Transform = new TranslateTransform3D(row * tray.RowSpacing, -column * tray.ColumnSpacing, 0)
                    };
                    // 根据可见性设置确定是否加入到3D场景中
                    if (((MainWindow)App.Current.MainWindow).layoutPanel.dominoesVisibilityMenuItem.IsChecked)
                    {
                        sortingVisual3D.Children.Add(modelVisual3D);
                    }
                    DominoModels.Add(modelVisual3D);
                    // 将模型记录在布局面板中
                    layoutPanel.DominoesModels.Add(modelVisual3D);
                }
            }
        }

        private void UpdateDominoQueueOrderLabels()
        {
            // 如果有的话，清空上一次的骨牌队列顺序标签
            DominoQueueOrderLabels.ForEach(l => sortingVisual3D.Children.Remove(l));
            DominoQueueOrderLabels.Clear();
            // 将模型从布局面板中删除
            LayoutPanel layoutPanel = ((MainWindow)App.Current.MainWindow).layoutPanel;
            layoutPanel.DominoQueueOrderLabelsModels.Clear();

            Tray tray = ((MainWindow)App.Current.MainWindow)
                                                .dominoesTaskPanel
                                                .dominoesTaskPanelSettingsTab
                                                .Tray;
            ObservableCollection<Domino> dominoes = ((MainWindow)App.Current.MainWindow)
                                                                            .dominoesTaskPanel
                                                                            .dominoesTaskPanelSettingsTab
                                                                            .Dominoes;

            Point3D originalPoint = new(-351.49, 296.66, 44.11);
            for (int i = 0; i < dominoes.Count; i++)
            {
                int row = (dominoes[i].ID - 1) / tray.Columns;
                int column = (dominoes[i].ID - 1) % tray.Columns;
                Vector3D offset = new(row * tray.RowSpacing, -column * tray.ColumnSpacing, 0);
                Point3D point = originalPoint + offset;
                BillboardTextVisual3D dominoQueueOrderLabel = new()
                {
                    Text = (i + 1).ToString(),
                    Foreground = Brushes.Black,
                    Background = new SolidColorBrush(new() { A = 255, R = 255, G = 255, B = 192 }),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new(1),
                    Padding = new(2),
                    Position = point
                };
                // 根据可见性设置确定是否加入到3D场景中
                if (((MainWindow)App.Current.MainWindow).layoutPanel.dominoQueueOrderLabelsVisibilityMenuItem.IsChecked)
                {
                    sortingVisual3D.Children.Add(dominoQueueOrderLabel);
                }
                DominoQueueOrderLabels.Add(dominoQueueOrderLabel);
                // 将模型记录在布局面板中
                layoutPanel.DominoQueueOrderLabelsModels.Add(dominoQueueOrderLabel);
            }
        }

        public void Tray_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Rows"
             && e.PropertyName != "Columns"
             && e.PropertyName != "RowSpacing"
             && e.PropertyName != "ColumnSpacing"
             || sender is not Tray tray) return;

            LoadDominoes(tray);
            UpdateDominoQueueOrderLabels();
        }

        public void Dominoes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Domino> dominoes = (sender as ObservableCollection<Domino>)!;

            // 如果有的话，清空上一次的骨牌预览模型
            DominoPreviewModels.ForEach(d => sortingVisual3D.Children.Remove(d));
            DominoPreviewModels.Clear();
            // 将模型从布局面板中删除
            LayoutPanel layoutPanel = ((MainWindow)App.Current.MainWindow).layoutPanel;
            layoutPanel.DominoesPreviewModels.Clear();

            ModelImporter modelImporter = new();
            foreach (Domino domino in dominoes)
            {
                Model3DGroup model3DGroup = modelImporter.Load(@$"{AppDomain.CurrentDomain.BaseDirectory}Models\DominoPreview_Model.obj");
                // obj模型的mtl文件自带透明度，但似乎HelixToolkit不认，所以手动修改材质
                Material material = MaterialHelper.CreateMaterial(Colors.Blue, 0.25);
                foreach (Model3D model3D in model3DGroup.Children)
                {
                    ((GeometryModel3D)model3D).Material = material;
                    ((GeometryModel3D)model3D).BackMaterial = material;
                }

                ModelVisual3D modelVisual3D = new()
                {
                    Content = model3DGroup,
                    Transform = Transform3DHelper.CombineTransform(new RotateTransform3D(new AxisAngleRotation3D(new(0, 0, 1), domino.Position.R), new(10, -234.26, 42.5)),
                                                                   new TranslateTransform3D(domino.Position.X, domino.Position.Y, domino.Position.Z))
                };
                // 根据可见性设置确定是否加入到3D场景中
                if (((MainWindow)App.Current.MainWindow).layoutPanel.dominoesPreviewVisibilityMenuItem.IsChecked)
                {
                    sortingVisual3D.Children.Add(modelVisual3D);
                }
                DominoPreviewModels.Add(modelVisual3D);
                // 将模型记录在布局面板中
                layoutPanel.DominoesPreviewModels.Add(modelVisual3D);

                // 为每一个骨牌订阅位置属性更改事件
                // 注意：先退订上一次的事件，再订阅这一次的事件，避免重复订阅
                // TODO 在Dominoes集合里移出Domino时，仍然保留着一个事件订阅，导致GC无法回收，考虑在Domino类里增加清除所有订阅方法，并在移出集合时调用
                domino.Position.PropertyChanged -= DominoPosition_PropertyChanged;
                domino.Position.PropertyChanged += DominoPosition_PropertyChanged;
                // 如果直接修改Domino类的Position属性而不是修改DominoPosition类的X、Y、Z、R属性也能触发事件
                // TODO 可以用事件转发代替
                domino.PropertyChanged -= DominoPosition_PropertyChanged;
                domino.PropertyChanged += DominoPosition_PropertyChanged;
                // 当骨牌ID属性更改时，更新骨牌队列顺序标签
                domino.PropertyChanged -= DominoID_PropertyChanged;
                domino.PropertyChanged += DominoID_PropertyChanged;
            }
            UpdateDominoQueueOrderLabels();
        }

        private void DominoPosition_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "X"
             && e.PropertyName != "Y"
             && e.PropertyName != "Z"
             && e.PropertyName != "R"
             && e.PropertyName != "Position") return;

            ObservableCollection<Domino> dominoes = ((MainWindow)App.Current.MainWindow)
                                                                            .dominoesTaskPanel
                                                                            .dominoesTaskPanelSettingsTab
                                                                            .Dominoes;

            for (int i = 0; i < dominoes.Count; i++)
            {
                // 先旋转后平移，在原点处旋转可以避免在非原点处旋转而引入的额外的平移分量
                DominoPreviewModels[i].Transform = Transform3DHelper.CombineTransform(new RotateTransform3D(new AxisAngleRotation3D(new(0, 0, 1), dominoes[i].Position.R), new(10, -234.26, 42.5)),
                                                                                      new TranslateTransform3D(dominoes[i].Position.X, dominoes[i].Position.Y, dominoes[i].Position.Z));
            }
        }

        private void DominoID_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ID") return;
            UpdateDominoQueueOrderLabels();
        }

        public void RdIsAttached_ValueChanged(object? sender, DataValueChangedEventArgs e)
        {
            if (sender is not RapidData rdIsAttached) return;

            if (bool.Parse(rdIsAttached.StringValue)) // 拾取
            {
                Point3D tcp = new(App.Robot.StatusCache!.LinearPosition.Trans.X,
                                  App.Robot.StatusCache!.LinearPosition.Trans.Y,
                                  App.Robot.StatusCache!.LinearPosition.Trans.Z);

                // 即使GetPosition(CoordinateSystemType.World)，也仍然是wobj0的基坐标系，手动将tcp从基坐标系转换到世界坐标系
                Transform3D transform3D = Transform3DHelper.CombineTransform(new RotateTransform3D(new AxisAngleRotation3D(new(0, 0, 1), 90), new(0, 0, 0)),
                                                                             new TranslateTransform3D(-310, -180, 10));
                tcp = transform3D.Transform(tcp);

                foreach (ModelVisual3D dominoModel in DominoModels)
                {
                    Rect3D dominoModelBoundingBox = dominoModel.FindBounds(Transform3D.Identity);
                    // 向上偏移一点确保拾取成功
                    dominoModelBoundingBox.Offset(0, 0, 5);

                    if (dominoModelBoundingBox.Contains(tcp))
                    {
                        isFirstPickOrPut = !isFirstPickOrPut;
                        AttachedDominoModel = dominoModel;
                        App.Robot.Joints[5].PropertyChanged += GlobalTransform_Changed;
                        return;
                    }
                }
            }
            else // 放置
            {
                try
                {
                    App.Robot.Joints[5].PropertyChanged -= GlobalTransform_Changed;
                }
                catch (ArgumentException)
                {
                    // 如果RAPID里IsAttached变量没有复位为FALSE，在初始化时会复位，引发一次值更改事件
                    // 但此时没有任何订阅，所以抛异常，不影响运行
                    // 常发生于运行中途停止机器人程序运行
                    // TODO 在RobotJoint类中自定义事件访问器，判断事件订阅是否为空
                }

                if (isFirstPickOrPut)
                {
                    AttachedDominoModel!.Transform = Transform3DHelper.CombineTransform(AttachedDominoModel.Transform,
                                                                                        new TranslateTransform3D(0, -6.2, -12.75));
                }

                AttachedDominoModel = null;
            }
        }

        private void GlobalTransform_Changed(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not RobotJoint robotJoint
                || e.PropertyName != "GlobalTransform"
                || AttachedDominoModel is null) return;

            Transform3D attachedDominoModelOriginalTransform = isFirstPickOrPut ? attachedDominoModelOriginalTransformHorizontal
                                                                                : attachedDominoModelOriginalTransformVertical;

            AttachedDominoModel.Transform = Transform3DHelper.CombineTransform(attachedDominoModelOriginalTransform,
                                                                               robotJoint.GlobalTransform);
        }

        private void GlyphButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not GlyphButton glyphButton) return;
            glyphButton.AnimateOpacity(1.0, 200);

            if (glyphButton.Content is not TextBlock textBlock) return;
            textBlock.FontSize = 20;
        }

        private void GlyphButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not GlyphButton glyphButton) return;
            glyphButton.AnimateOpacity(0.5, 200);

            if (glyphButton.Content is not TextBlock textBlock) return;
            textBlock.FontSize = 16;
        }

        #region 重置场景命令

        private void ResetSceneCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            isFirstPickOrPut = false;
            try
            {
                App.Robot.Joints[5].PropertyChanged -= GlobalTransform_Changed;
            }
            catch (ArgumentException)
            {
                // 同上
            }
            AttachedDominoModel = null;
            Tray tray = ((MainWindow)App.Current.MainWindow).dominoesTaskPanel.dominoesTaskPanelSettingsTab.Tray;
            LoadDominoes(tray);
        }

        private void ResetSceneCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = App.Robot.StatusCache?.ExecutionStatus != ExecutionStatus.Running;
        }

        #endregion

        #region 显示或隐藏场景信息命令

        private void ShowOrHideSceneInfoCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            helixViewport3D.ShowCameraInfo = !helixViewport3D.ShowCameraInfo;
            helixViewport3D.ShowTriangleCountInfo = !helixViewport3D.ShowTriangleCountInfo;
            helixViewport3D.ShowFrameRate = !helixViewport3D.ShowFrameRate;

            if (e.Parameter is not GlyphButton glyphButton) return;
            SolidColorBrush solidColorBrush = helixViewport3D.ShowCameraInfo ? new(new() { A = 255, R = 241, G = 175, B = 119 })
                                                                             : new(new() { A = 255, R = 246, G = 246, B = 246 });
            glyphButton.Background = solidColorBrush;
            glyphButton.HoverBackground = solidColorBrush;
        }

        private void ShowOrHideSceneInfoCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}