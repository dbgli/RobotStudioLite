using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite.Graphics3D
{
    /// <summary>
    /// 一个用于显示一组网格线的视觉元素。
    /// </summary>
    public class GridLinesVisual3D : MeshElement3D
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="Center" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
            "Center",
            typeof(Point3D),
            typeof(GridLinesVisual3D),
            new UIPropertyMetadata(new Point3D(), GeometryChanged));

        /// <summary>
        /// 标识<see cref="MinorDistance" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty MinorDistanceProperty = DependencyProperty.Register(
            "MinorDistance",
            typeof(double),
            typeof(GridLinesVisual3D),
            new PropertyMetadata(2.5, GeometryChanged));

        /// <summary>
        /// 标识<see cref="LengthDirection" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty LengthDirectionProperty = DependencyProperty.Register(
            "LengthDirection",
            typeof(Vector3D),
            typeof(GridLinesVisual3D),
            new UIPropertyMetadata(new Vector3D(1, 0, 0), GeometryChanged));

        /// <summary>
        /// 标识<see cref="Length" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length",
            typeof(double),
            typeof(GridLinesVisual3D),
            new PropertyMetadata(200.0, GeometryChanged));

        /// <summary>
        /// 标识<see cref="MajorDistance" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty MajorDistanceProperty = DependencyProperty.Register(
            "MajorDistance",
            typeof(double),
            typeof(GridLinesVisual3D),
            new PropertyMetadata(10.0, GeometryChanged));

        /// <summary>
        /// 标识<see cref="Normal" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(
            "Normal",
            typeof(Vector3D),
            typeof(GridLinesVisual3D),
            new UIPropertyMetadata(new Vector3D(0, 0, 1), GeometryChanged));

        /// <summary>
        /// 标识<see cref="Thickness" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            "Thickness",
            typeof(double),
            typeof(GridLinesVisual3D),
            new PropertyMetadata(0.08, GeometryChanged));

        /// <summary>
        /// 标识<see cref="Width" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width",
            typeof(double),
            typeof(GridLinesVisual3D),
            new PropertyMetadata(200.0, GeometryChanged));
        #endregion

        #region 字段
        /// <summary>
        /// 长度方向。
        /// </summary>
        private Vector3D lengthDirection;

        /// <summary>
        /// 宽度方向。
        /// </summary>
        private Vector3D widthDirection;
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置网格的中心点。
        /// </summary>
        /// <value>中心点。</value>
        public Point3D Center
        {
            get => (Point3D)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        /// <summary>
        /// 获取或设置网格区域的长度。
        /// </summary>
        /// <value>长度。</value>
        public double Length
        {
            get => (double)GetValue(LengthProperty);
            set => SetValue(LengthProperty, value);
        }

        /// <summary>
        /// 获取或设置网格的长度方向。
        /// </summary>
        /// <value>长度方向。</value>
        public Vector3D LengthDirection
        {
            get => (Vector3D)GetValue(LengthDirectionProperty);
            set => SetValue(LengthDirectionProperty, value);
        }

        /// <summary>
        /// 获取或设置主网格线间的距离。
        /// </summary>
        /// <value>距离。</value>
        public double MajorDistance
        {
            get => (double)GetValue(MajorDistanceProperty);
            set => SetValue(MajorDistanceProperty, value);
        }

        /// <summary>
        /// 获取或设置次网格线间的距离。
        /// </summary>
        /// <value>距离。</value>
        public double MinorDistance
        {
            get => (double)GetValue(MinorDistanceProperty);
            set => SetValue(MinorDistanceProperty, value);
        }

        /// <summary>
        /// 获取或设置网格平面的法线向量。
        /// </summary>
        /// <value>法线向量。</value>
        public Vector3D Normal
        {
            get => (Vector3D)GetValue(NormalProperty);
            set => SetValue(NormalProperty, value);
        }

        /// <summary>
        /// 获取或设置网格线的线宽。
        /// </summary>
        /// <value>线宽。</value>
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        /// <summary>
        /// 获取或设置网格区域的宽度（垂直于长度方向）。
        /// </summary>
        /// <value>宽度。</value>
        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        #endregion

        /// <summary>
        /// 初始化一个<see cref="GridLinesVisual3D" />类的新实例。
        /// </summary>
        public GridLinesVisual3D()
        {
            Fill = Brushes.Gray;
        }

        /// <summary>
        /// 执行曲面细分并返回<see cref="MeshGeometry3D" />对象。
        /// </summary>
        /// <returns>
        /// 三角形网格几何体。
        /// </returns>
        protected override MeshGeometry3D Tessellate()
        {
            lengthDirection = LengthDirection;
            lengthDirection.Normalize();

            // 如果法线向量和长度方向不垂直，则覆盖长度方向
            if (Vector3D.DotProduct(Normal, LengthDirection) != 0.0)
            {
                lengthDirection = Normal.FindAnyPerpendicular();
                lengthDirection.Normalize();
            }

            // 通过将长度方向向量围绕法线向量旋转90°来创建宽度方向
            RotateTransform3D rotate = new(new AxisAngleRotation3D(Normal, 90.0));
            widthDirection = rotate.Transform(lengthDirection);
            widthDirection.Normalize();

            MeshBuilder mesh = new(true, false);
            double minX = -Width / 2;
            double minY = -Length / 2;
            double maxX = Width / 2;
            double maxY = Length / 2;
            double epsilon = MinorDistance / 10;

            double x = minX;
            while (x <= maxX + epsilon)
            {
                double t = Thickness;
                if (MajorDistance == MinorDistance || IsMultipleOf(x, MajorDistance))
                {
                    t *= 2;
                }

                AddLineX(mesh, x, minY, maxY, t);
                x += MinorDistance;
            }

            double y = minY;
            while (y <= maxY + epsilon)
            {
                double t = Thickness;
                if (MajorDistance == MinorDistance || IsMultipleOf(y, MajorDistance))
                {
                    t *= 2;
                }

                AddLineY(mesh, y, minX, maxX, t);
                y += MinorDistance;
            }

            MeshGeometry3D m = mesh.ToMesh();
            m.Freeze();
            return m;
        }

        /// <summary>
        /// 确定x是否是d的倍数。
        /// </summary>
        /// <param name="x">
        /// 参数x。
        /// </param>
        /// <param name="d">
        /// 参数d。
        /// </param>
        /// <returns>
        /// 如果x是d的倍数，则返回true；否则返回false。
        /// </returns>
        private static bool IsMultipleOf(double x, double d)
        {
            double x2 = d * (int)(x / d);
            return Math.Abs(x - x2) < 1e-3;
        }

        /// <summary>
        /// 添加X轴方向的线。
        /// </summary>
        /// <param name="mesh">
        /// 网格。
        /// </param>
        /// <param name="x">
        /// X轴坐标。
        /// </param>
        /// <param name="minY">
        /// Y轴最小值。
        /// </param>
        /// <param name="maxY">
        /// Y轴最大值。
        /// </param>
        /// <param name="thickness">
        /// 线宽。
        /// </param>
        private void AddLineX(MeshBuilder mesh, double x, double minY, double maxY, double thickness)
        {
            int i0 = mesh.Positions.Count;
            mesh.Positions.Add(GetPoint(x - (thickness / 2), minY));
            mesh.Positions.Add(GetPoint(x - (thickness / 2), maxY));
            mesh.Positions.Add(GetPoint(x + (thickness / 2), maxY));
            mesh.Positions.Add(GetPoint(x + (thickness / 2), minY));
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.TriangleIndices.Add(i0);
            mesh.TriangleIndices.Add(i0 + 1);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 3);
            mesh.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// 添加Y轴方向的线。
        /// </summary>
        /// <param name="mesh">
        /// 网格。
        /// </param>
        /// <param name="y">
        /// Y轴坐标。
        /// </param>
        /// <param name="minX">
        /// X轴最小值。
        /// </param>
        /// <param name="maxX">
        /// X轴最大值。
        /// </param>
        /// <param name="thickness">
        /// 线宽。
        /// </param>
        private void AddLineY(MeshBuilder mesh, double y, double minX, double maxX, double thickness)
        {
            int i0 = mesh.Positions.Count;
            mesh.Positions.Add(GetPoint(minX, y + (thickness / 2)));
            mesh.Positions.Add(GetPoint(maxX, y + (thickness / 2)));
            mesh.Positions.Add(GetPoint(maxX, y - (thickness / 2)));
            mesh.Positions.Add(GetPoint(minX, y - (thickness / 2)));
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.Normals.Add(Normal);
            mesh.TriangleIndices.Add(i0);
            mesh.TriangleIndices.Add(i0 + 1);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 2);
            mesh.TriangleIndices.Add(i0 + 3);
            mesh.TriangleIndices.Add(i0);
        }

        /// <summary>
        /// 在平面上获取一个点。
        /// </summary>
        /// <param name="x">
        /// X轴坐标。
        /// </param>
        /// <param name="y">
        /// Y轴坐标。
        /// </param>
        /// <returns>
        /// 一个表示坐标的<see cref="Point3D" />对象。
        /// </returns>
        private Point3D GetPoint(double x, double y)
        {
            return Center + (widthDirection * x) + (lengthDirection * y);
        }
    }
}