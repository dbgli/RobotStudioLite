using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite.Graphics3D
{
    /// <summary>
    /// 一个用于显示视图立方体的视觉元素。
    /// </summary>
    public class ViewCubeVisual3D : ModelVisual3D
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="FrontText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty FrontTextProperty = DependencyProperty.Register(
            "FrontText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("F", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(0, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="BackText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty BackTextProperty = DependencyProperty.Register(
            "BackText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("B", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(1, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="LeftText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty LeftTextProperty = DependencyProperty.Register(
            "LeftText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("L", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(2, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="RightText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty RightTextProperty = DependencyProperty.Register(
            "RightText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("R", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(3, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="TopText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty TopTextProperty = DependencyProperty.Register(
            "TopText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("U", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(4, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="BottomText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty BottomTextProperty = DependencyProperty.Register(
            "BottomText",
            typeof(string),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata("D", (d, e) =>
            {
                Brush brush = (d as ViewCubeVisual3D)!.GetCubeFaceColor(-1);
                (d as ViewCubeVisual3D)!.UpdateCubeFaceMaterial(5, brush, e.NewValue == null ? string.Empty : (string)e.NewValue);
            }));

        /// <summary>
        /// 标识<see cref="Center" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Point3D),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata(new Point3D(0, 0, 0)));

        /// <summary>
        /// 标识<see cref="Size" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size",
            typeof(double),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata(5.0, VisualModelChanged));

        /// <summary>
        /// 标识<see cref="IsEnabled" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled",
            typeof(bool),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata(true));

        /// <summary>
        /// 标识<see cref="IsTopBottomViewOrientedToFrontBack" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsTopBottomViewOrientedToFrontBackProperty = DependencyProperty.Register(
            "IsTopBottomViewOrientedToFrontBack",
            typeof(bool),
            typeof(ViewCubeVisual3D),
            new PropertyMetadata(false, VisualModelChanged));

        /// <summary>
        /// 标识<see cref="ModelUpDirection" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ModelUpDirectionProperty = DependencyProperty.Register(
            "ModelUpDirection",
            typeof(Vector3D),
            typeof(ViewCubeVisual3D),
            new UIPropertyMetadata(new Vector3D(0, 0, 1), VisualModelChanged));

        /// <summary>
        /// 标识<see cref="Viewport" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ViewportProperty = DependencyProperty.Register(
            "Viewport",
            typeof(Viewport3D),
            typeof(ViewCubeVisual3D),
            new PropertyMetadata(null));

        /// <summary>
        /// 标识<see cref="EnableEdgeClicks" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty EnableEdgeClicksProperty = DependencyProperty.Register(
            "EnableEdgeClicks",
            typeof(bool),
            typeof(ViewCubeVisual3D),
            new PropertyMetadata(false, (d, e) =>
            {
                (d as ViewCubeVisual3D)?.EnableDisableEdgeClicks();
            }));
        #endregion

        #region 字段
        private readonly Dictionary<object, (Vector3D faceNormal, Vector3D faceUpVector)> _faceUpNormalVectors = new();

        private readonly ModelUIElement3D[] _cubeFaceModels = new ModelUIElement3D[6]; // 立方体的6个面
        private readonly ModelUIElement3D[] _cubeEdgeModels = new ModelUIElement3D[12]; // 立方体的12个边
        private readonly ModelUIElement3D[] _cubeCornerModels = new ModelUIElement3D[8]; // 立方体的8个顶点
        private readonly PieSliceVisual3D _circle = new();

        private readonly SolidColorBrush _cornerBrush = Brushes.Silver;
        private readonly SolidColorBrush _edgeBrush = Brushes.Silver;
        private readonly SolidColorBrush _highlightBrush = Brushes.CornflowerBlue;

        private Vector3D _frontVector;
        private Vector3D _leftVector;
        private Vector3D _upVector;

        /// <summary>
        /// 将立方体面编号映射到立方体的文本名称。
        /// </summary>
        private readonly Dictionary<int, string> _mapFaceNames;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置后面文本。
        /// </summary>
        /// <value>后面文本。</value>
        public string BackText
        {
            get => (string)GetValue(BackTextProperty);
            set => SetValue(BackTextProperty, value);
        }

        /// <summary>
        /// 获取或设置下面文本。
        /// </summary>
        /// <value>下面文本。</value>
        public string BottomText
        {
            get => (string)GetValue(BottomTextProperty);
            set => SetValue(BottomTextProperty, value);
        }

        /// <summary>
        /// 获取或设置中心点坐标。
        /// </summary>
        /// <value>中心点坐标。</value>
        public Point3D Center
        {
            get => (Point3D)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        /// <summary>
        /// 获取或设置前面文本。
        /// </summary>
        /// <value>前面文本。</value>
        public string FrontText
        {
            get => (string)GetValue(FrontTextProperty);
            set => SetValue(FrontTextProperty, value);
        }

        /// <summary>
        /// 获取或设置左面文本。
        /// </summary>
        /// <value>左面文本。</value>
        public string LeftText
        {
            get => (string)GetValue(LeftTextProperty);
            set => SetValue(LeftTextProperty, value);
        }

        /// <summary>
        /// 获取或设置模型上方向。
        /// </summary>
        /// <value>模型上方向。</value>
        public Vector3D ModelUpDirection
        {
            get => (Vector3D)GetValue(ModelUpDirectionProperty);
            set => SetValue(ModelUpDirectionProperty, value);
        }

        /// <summary>
        /// 获取或设置右面文本。
        /// </summary>
        /// <value>右面文本。</value>
        public string RightText
        {
            get => (string)GetValue(RightTextProperty);
            set => SetValue(RightTextProperty, value);
        }

        /// <summary>
        /// 获取或设置尺寸。
        /// </summary>
        /// <value>尺寸。</value>
        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        /// <summary>
        /// 获取或设置上面文本。
        /// </summary>
        /// <value>上面文本。</value>
        public string TopText
        {
            get => (string)GetValue(TopTextProperty);
            set => SetValue(TopTextProperty, value);
        }

        /// <summary>
        /// 获取或设置一个值，该值指示视图立方体是否启用。
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// 获取或设置一个值，该值指示俯视图和仰视图是否面向前视图和后视图。
        /// </summary>
        public bool IsTopBottomViewOrientedToFrontBack
        {
            get => (bool)GetValue(IsTopBottomViewOrientedToFrontBackProperty);
            set => SetValue(IsTopBottomViewOrientedToFrontBackProperty, value);
        }

        /// <summary>
        /// 获取或设置由视图立方体所控制的视口。
        /// </summary>
        /// <value>视口。</value>
        [Browsable(false)]
        public Viewport3D Viewport
        {
            get => (Viewport3D)GetValue(ViewportProperty);
            set => SetValue(ViewportProperty, value);
        }

        private double Overhang => 0.001 * Size;

        /// <summary>
        /// 获取或设置视图立方体边缘能否点击。
        /// </summary>
        public bool EnableEdgeClicks
        {
            get => (bool)GetValue(EnableEdgeClicksProperty);
            set => SetValue(EnableEdgeClicksProperty, value);
        }
        #endregion

        #region 事件
        /// <summary>
        /// 当一个面被单击时发生。
        /// </summary>
        public event EventHandler<ClickedEventArgs>? Clicked;
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化<see cref="ViewCubeVisual3D" />类的新实例。
        /// </summary>
        public ViewCubeVisual3D()
        {
            _mapFaceNames = new()
            {
                // 具有文本属性的映射
                [0] = FrontText,
                [1] = BackText,
                [2] = LeftText,
                [3] = RightText,
                [4] = TopText,
                [5] = BottomText
            };
            InitialModels();
        }
        #endregion

        /// <summary>
        /// 引发Clicked事件。
        /// </summary>
        /// <param name="lookDirection">查看方向。</param>
        /// <param name="upDirection">向上方向。</param>
        protected virtual void OnClicked(Vector3D lookDirection, Vector3D upDirection)
        {
            Clicked?.Invoke(this, new() { LookDirection = lookDirection, UpDirection = upDirection });
        }

        /// <summary>
        /// VisualModel属性发生更改。
        /// </summary>
        /// <param name="d">
        /// 发送者。
        /// </param>
        /// <param name="e">
        /// 事件参数。
        /// </param>
        private static void VisualModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewCubeVisual3D)d).UpdateVisuals();
        }

        /// <summary>
        /// 初始模型。
        /// </summary>
        void InitialModels()
        {
            // 初始化元素
            // 初始化立方体的6个面
            for (int i = 0; i < _cubeFaceModels.Length; i++)
            {
                ModelUIElement3D element = new();
                element.MouseLeftButtonDown += FaceMouseLeftButtonDown;
                element.MouseEnter += FacesMouseEnters;
                element.MouseLeave += FacesMouseLeaves;
                _cubeFaceModels[i] = element;
                Children.Add(element);
            }
            // 初始化立方体的12个边
            for (int i = 0; i < _cubeEdgeModels.Length; i++)
            {
                ModelUIElement3D element = new();
                element.MouseLeftButtonDown += FaceMouseLeftButtonDown;
                element.MouseEnter += EdgesMouseEnters;
                element.MouseLeave += EdgesMouseLeaves;
                _cubeEdgeModels[i] = element;
                Children.Add(element);
            }
            // 初始化立方体的8个顶点
            for (int i = 0; i < _cubeCornerModels.Length; i++)
            {
                ModelUIElement3D element = new();
                element.MouseLeftButtonDown += FaceMouseLeftButtonDown;
                element.MouseEnter += CornersMouseEnters;
                element.MouseLeave += CornersMouseLeaves;
                _cubeCornerModels[i] = element;
                Children.Add(element);
            }

            // 添加立方体边框线
            // 立方体的8个顶点坐标
            Point3D p0 = new(-2.5, -2.5, 2.5);
            Point3D p1 = new(2.5, -2.5, 2.5);
            Point3D p2 = new(2.5, 2.5, 2.5);
            Point3D p3 = new(-2.5, 2.5, 2.5);
            Point3D p4 = new(-2.5, -2.5, -2.5);
            Point3D p5 = new(2.5, -2.5, -2.5);
            Point3D p6 = new(2.5, 2.5, -2.5);
            Point3D p7 = new(-2.5, 2.5, -2.5);
            LinesVisual3D lines = new()
            {
                Color = Colors.Gray,
                Thickness = 1,
                Points = new() { p0, p1, p1, p2, p2, p3, p3, p0, p4, p5, p5, p6, p6, p7, p7, p4, p0, p4, p1, p5, p2, p6, p3, p7 }
            };
            Children.Add(lines);

            //添加底部圆环
            Children.Add(_circle);

            UpdateVisuals();
            EnableDisableEdgeClicks();
        }

        private void UpdateVisuals()
        {
            // 更新本地单位向量
            CalculateLocalUnitVectors();
            // 更新面
            _faceUpNormalVectors.Clear();
            CreateCubeFaces();
            CreateCubeEdges();
            CreateCubeCorners();
            CreateCircle();
        }

        /// <summary>
        /// 计算上，前，左向量的归一化单位向量
        /// </summary>
        void CalculateLocalUnitVectors()
        {
            // 坐标系
            //     | Z (Up)
            //     |
            //     |
            //    O|_________ Y (Left)
            //    /
            //   /
            //  / X (Front)

            Vector3D vecUp = ModelUpDirection;

            // 使用右手法则创建与上向量相差90度的左向量
            Vector3D vecLeft1 = new(vecUp.Y, vecUp.Z, vecUp.X);
            if (vecLeft1 == vecUp)
            {
                vecLeft1 = new(0, 0, 1);
            }
            Vector3D vecFront = Vector3D.CrossProduct(vecLeft1, vecUp);
            Vector3D vecLeft = Vector3D.CrossProduct(vecUp, vecFront);
            vecUp.Normalize();
            vecLeft.Normalize();
            vecFront.Normalize();
            _frontVector = vecFront;
            _leftVector = vecLeft;
            _upVector = vecUp;
        }

        void CreateCubeFaces()
        {
            AddCubeFace(_cubeFaceModels[0], _frontVector, _upVector, GetCubeFaceColor(-1), FrontText);
            AddCubeFace(_cubeFaceModels[1], -_frontVector, _upVector, GetCubeFaceColor(-1), BackText);
            AddCubeFace(_cubeFaceModels[2], _leftVector, _upVector, GetCubeFaceColor(-1), LeftText);
            AddCubeFace(_cubeFaceModels[3], -_leftVector, _upVector, GetCubeFaceColor(-1), RightText);
            if (IsTopBottomViewOrientedToFrontBack)
            {
                AddCubeFace(_cubeFaceModels[4], _upVector, _frontVector, GetCubeFaceColor(-1), TopText);
                AddCubeFace(_cubeFaceModels[5], -_upVector, -_frontVector, GetCubeFaceColor(-1), BottomText);
            }
            else
            {
                AddCubeFace(_cubeFaceModels[4], _upVector, _leftVector, GetCubeFaceColor(-1), TopText);
                AddCubeFace(_cubeFaceModels[5], -_upVector, -_leftVector, GetCubeFaceColor(-1), BottomText);
            }
        }

        private Brush GetCubeFaceColor(int index)
        {
            double max = Math.Max(Math.Max(ModelUpDirection.X, ModelUpDirection.Y), ModelUpDirection.Z);
            if (max == ModelUpDirection.Z)
            {
                return index switch
                {
                    0 or 1 => Brushes.Red,
                    2 or 3 => Brushes.Green,
                    4 or 5 => Brushes.Blue,
                    _ => Brushes.Silver,
                };
            }
            else if (max == ModelUpDirection.Y)
            {
                return index switch
                {
                    0 or 1 => Brushes.Blue,
                    2 or 3 => Brushes.Red,
                    4 or 5 => Brushes.Green,
                    _ => Brushes.Silver,
                };
            }
            else // if (max == ModelUpDirection.X)
            {
                return index switch
                {
                    0 or 1 => Brushes.Green,
                    2 or 3 => Brushes.Blue,
                    4 or 5 => Brushes.Red,
                    _ => Brushes.Silver,
                };
            }
        }

        void CreateCubeEdges()
        {
            //               Z | Up
            //                 |
            //         p4______|__p47______p7
            //         /|      |         /|
            //        / |      |        / |
            //    p45/  |      |    p67/  |
            //      /  p04     |      /   |
            //  p5 |--------p56------|p6 p37
            //     |    |      |     |    |
            //     |    |    O +-----|---------- Y Left
            //     |  p0|_____/_p03__|____|p3
            //    p15  /     /      p26   /
            //     |  /     /        |   /
            //     | /p01  /         |  /p23
            //     |/     /          | /
            //  p1 |_____/___p12_____|/p2
            //          /
            //       X / Front

            if (Size == 0) return;
            double halfSize = Size / 2;
            double sideWidthHeight = Size / 5;

            double moveDistance = Math.Sqrt(2) * (sideWidthHeight / 2 - Overhang);
            double squaredLength = Size - 2 * (sideWidthHeight - Overhang);

            Point3D p0 = Center - (_leftVector + _frontVector + _upVector) * halfSize;
            Point3D p1 = p0 + _frontVector * Size;
            Point3D p2 = p1 + _leftVector * Size;
            Point3D p3 = p0 + _leftVector * Size;

            Point3D p04 = p0 + _upVector * halfSize;
            Point3D p15 = p1 + _upVector * halfSize;
            Point3D p26 = p2 + _upVector * halfSize;
            Point3D p37 = p3 + _upVector * halfSize;

            Point3D p01 = p0 + _frontVector * halfSize;
            Point3D p03 = p0 + _leftVector * halfSize;
            Point3D p12 = p03 + _frontVector * Size;
            Point3D p23 = p01 + _leftVector * Size;

            Point3D p45 = p01 + _upVector * Size;
            Point3D p56 = p12 + _upVector * Size;
            Point3D p67 = p23 + _upVector * Size;
            Point3D p47 = p03 + _upVector * Size;

            Point3D[] xPoints = new[] { p01, p23, p67, p45 };
            Point3D[] yPoints = new[] { p56, p12, p03, p47 };
            Point3D[] zPoints = new[] { p04, p15, p26, p37 };
            int counter = 0;
            for (int i = 0; i < xPoints.Length; i++)
            {
                Point3D point = xPoints[i];
                Vector3D faceNormal = (Vector3D)point;
                faceNormal.Normalize();
                Point3D p = point - faceNormal * moveDistance;
                AddCubeEdge(_cubeEdgeModels[counter], p, squaredLength, sideWidthHeight, sideWidthHeight, faceNormal);
                counter++;
            }
            for (int i = 0; i < yPoints.Length; i++)
            {
                Point3D point = yPoints[i];
                Vector3D faceNormal = (Vector3D)point;
                faceNormal.Normalize();
                Point3D p = point - faceNormal * moveDistance;
                AddCubeEdge(_cubeEdgeModels[counter], p, sideWidthHeight, squaredLength, sideWidthHeight, faceNormal);
                counter++;
            }
            for (int i = 0; i < zPoints.Length; i++)
            {
                Point3D point = zPoints[i];
                Vector3D faceNormal = (Vector3D)point;
                faceNormal.Normalize();
                Point3D p = point - faceNormal * moveDistance;
                AddCubeEdge(_cubeEdgeModels[counter], p, sideWidthHeight, sideWidthHeight, squaredLength, faceNormal);
                counter++;
            }
        }

        void CreateCubeCorners()
        {
            //               Z | Up
            //                 |
            //         p4______|__________p7
            //         /|      |         /|
            //        / |      |        / |
            //       /  |      |       /  |
            //      /   |      |      /   |
            //  p5 |-----------------|p6  |
            //     |    |      |     |    |
            //     |    |    O +-----|---------- Y Left
            //     |  p0|_____/______|____|p3
            //     |   /     /       |    /
            //     |  /     /        |   /
            //     | /     /         |  /
            //     |/     /          | /
            //  p1 |_____/___________|/p2
            //          /
            //       X / Front

            if (Size == 0) return;

            double halfSize = Size / 2;
            double sideLength = Size / 5;
            double moveDistance = Math.Sqrt(3) * (sideLength / 2 - Overhang);

            Point3D p0 = Center - (_leftVector + _frontVector + _upVector) * halfSize;
            Point3D p1 = p0 + _frontVector * Size;
            Point3D p2 = p1 + _leftVector * Size;
            Point3D p3 = p0 + _leftVector * Size;
            Point3D p4 = p0 + _upVector * Size;
            Point3D p5 = p1 + _upVector * Size;
            Point3D p6 = p2 + _upVector * Size;
            Point3D p7 = p3 + _upVector * Size;

            Point3D[] cornerPoints = new[] { p0, p1, p2, p3, p4, p5, p6, p7 };
            for (int i = 0; i < _cubeCornerModels.Length; i++)
            {
                Point3D point = cornerPoints[i];
                Vector3D faceNormal = (Vector3D)point;
                faceNormal.Normalize();

                Point3D p = point - faceNormal * moveDistance;

                AddCubeCorner(_cubeCornerModels[i], p, sideLength, faceNormal);
            }
        }

        void CreateCircle()
        {
            _circle.BeginEdit();
            _circle.Center = (Point3D)(_upVector * (-Size / 2));
            _circle.Normal = _upVector;
            _circle.UpVector = _leftVector; // 旋转90度使其位于立方体的底部平面
            _circle.InnerRadius = Size * 1.2;
            _circle.OuterRadius = Size * 1.6;
            _circle.StartAngle = 0;
            _circle.EndAngle = 360;
            _circle.Fill = Brushes.Silver;
            _circle.ThetaDiv = 100; // 默认是20太低了
            _circle.EndEdit();

            PieSliceVisual3D innerCircle = new()
            {
                Center = (Point3D)(_upVector * (-Size / 2)),
                Normal = _upVector,
                UpVector = _leftVector,
                InnerRadius = Size,
                OuterRadius = Size * 1.2,
                StartAngle = 0,
                EndAngle = 360,
                Fill = Brushes.Gray,
                ThetaDiv = 100
            };
            _circle.Children.Add(innerCircle);
        }

        private void AddCubeFace(ModelUIElement3D element, Vector3D normal, Vector3D up, Brush background, string text)
        {
            Material material = CreateTextMaterial(background, text);
            double s = Size;
            MeshBuilder builder = new(false, true);
            builder.AddCubeFace(Center, normal, up, s, s, s);
            MeshGeometry3D geometry = builder.ToMesh();
            geometry.Freeze();
            GeometryModel3D model = new() { Geometry = geometry, Material = material };
            element.Model = model;

            _faceUpNormalVectors.Add(element, (normal, up));
        }

        private void AddCubeEdge(ModelUIElement3D element, Point3D center, double x, double y, double z, Vector3D faceNormal)
        {
            MeshBuilder builder = new(false, true);
            builder.AddBox(center, _frontVector, _leftVector, x, y, z);
            MeshGeometry3D geometry = builder.ToMesh();
            geometry.Freeze();
            GeometryModel3D model = new() { Geometry = geometry, Material = MaterialHelper.CreateMaterial(_edgeBrush) };
            element.Model = model;

            _faceUpNormalVectors.Add(element, (faceNormal, _upVector));
        }

        void AddCubeCorner(ModelUIElement3D element, Point3D center, double sideLength, Vector3D faceNormal)
        {

            MeshBuilder builder = new(false, true);
            builder.AddBox(center, _frontVector, _leftVector, sideLength, sideLength, sideLength);
            MeshGeometry3D geometry = builder.ToMesh();
            geometry.Freeze();
            GeometryModel3D model = new() { Geometry = geometry, Material = MaterialHelper.CreateMaterial(_cornerBrush) };
            element.Model = model;

            _faceUpNormalVectors.Add(element, (faceNormal, _upVector));
        }

        private static Material CreateTextMaterial(Brush background, string text, Brush? foreground = null)
        {
            Grid grid = new() { Width = 150, Height = 150, Background = background };
            foreground ??= Brushes.Gray;
            grid.Children.Add(
                new TextBlock()
                {
                    Text = text,
                    FontFamily = new("Microsoft YaHei UI,Microsoft YaHei"),
                    FontSize = 64,
                    FontWeight = FontWeights.Bold,
                    Foreground = foreground,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
            grid.Arrange(new Rect(new Point(0, 0), new Size(150, 150)));

            RenderTargetBitmap bmp = new((int)grid.Width, (int)grid.Height, 96, 96, PixelFormats.Default);
            bmp.Render(grid);
            bmp.Freeze();
            Material material = MaterialHelper.CreateMaterial(new ImageBrush(bmp));
            material.Freeze();
            return material;
        }

        private void EnableDisableEdgeClicks()
        {
            if (EnableEdgeClicks)
            {
                for (int i = 0; i < _cubeEdgeModels.Length; i++)
                {
                    _cubeEdgeModels[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < _cubeCornerModels.Length; i++)
                {
                    _cubeCornerModels[i].Visibility = Visibility.Visible;
                }
            }
            else
            {
                for (int i = 0; i < _cubeEdgeModels.Length; i++)
                {
                    _cubeEdgeModels[i].Visibility = Visibility.Hidden;
                }
                for (int i = 0; i < _cubeCornerModels.Length; i++)
                {
                    _cubeCornerModels[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private void UpdateCubeFaceMaterial(int index, Brush background, string text)
        {
            if (_cubeFaceModels.Length > 0 && index < _cubeFaceModels.Length)
            {
                (_cubeFaceModels[index].Model as GeometryModel3D)!.Material = CreateTextMaterial(background, text);
                _mapFaceNames[index] = text;
            }
            else
            {
                UpdateVisuals();
            }
        }

        private void FacesMouseEnters(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            int faceIndex = Array.IndexOf(_cubeFaceModels, s);
            (s.Model as GeometryModel3D)!.Material = CreateTextMaterial(_highlightBrush, _mapFaceNames[faceIndex], Brushes.White);
        }

        private void FacesMouseLeaves(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            int faceIndex = Array.IndexOf(_cubeFaceModels, s);
            (s.Model as GeometryModel3D)!.Material = CreateTextMaterial(GetCubeFaceColor(-1), _mapFaceNames[faceIndex]);
        }

        private void EdgesMouseEnters(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            (s.Model as GeometryModel3D)!.Material = MaterialHelper.CreateMaterial(_highlightBrush);
        }

        private void EdgesMouseLeaves(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            (s.Model as GeometryModel3D)!.Material = MaterialHelper.CreateMaterial(_edgeBrush);

        }

        private void CornersMouseEnters(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            (s.Model as GeometryModel3D)!.Material = MaterialHelper.CreateMaterial(_highlightBrush);
        }

        private void CornersMouseLeaves(object sender, MouseEventArgs e)
        {
            ModelUIElement3D s = (sender as ModelUIElement3D)!;
            (s.Model as GeometryModel3D)!.Material = MaterialHelper.CreateMaterial(_cornerBrush);
        }

        /// <summary>
        /// 处理视图立方体上的左键单击事件。
        /// </summary>
        /// <param name="sender">
        /// 发送者。
        /// </param>
        /// <param name="e">
        /// 事件参数。
        /// </param>
        private void FaceMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsEnabled) return;

            Vector3D faceNormal = _faceUpNormalVectors[sender].faceNormal;
            Vector3D faceUp = _faceUpNormalVectors[sender].faceUpVector;

            Vector3D lookDirection = -faceNormal;
            Vector3D upDirection = faceUp;
            lookDirection.Normalize();
            upDirection.Normalize();

            // 双击反转查看方向
            if (e.ClickCount == 2)
            {
                lookDirection *= -1;
                if (upDirection != _upVector)
                {
                    upDirection *= -1;
                }
            }

            if (Viewport?.Camera is ProjectionCamera camera)
            {
                Point3D target = camera.Position + camera.LookDirection;
                double distance = camera.LookDirection.Length;
                lookDirection *= distance;
                Point3D newPosition = target - lookDirection;
                CameraHelper.AnimateTo(camera, newPosition, lookDirection, upDirection, 500);
            }

            e.Handled = true;
            OnClicked(lookDirection, upDirection);
        }

        /// <summary>
        /// 提供Clicked事件的事件数据。
        /// </summary>
        public class ClickedEventArgs : EventArgs
        {
            /// <summary>
            /// 获取或设置查看方向。
            /// </summary>
            /// <value>查看方向。</value>
            public Vector3D LookDirection { get; set; }

            /// <summary>
            /// 获取或设置向上方向。
            /// </summary>
            /// <value>向上方向。</value>
            public Vector3D UpDirection { get; set; }
        }
    }
}