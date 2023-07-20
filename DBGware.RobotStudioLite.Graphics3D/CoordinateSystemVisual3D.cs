using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace DBGware.RobotStudioLite.Graphics3D
{
    /// <summary>
    /// 一个用于显示坐标系的视觉元素，带有指示X、Y、Z方向的箭头。
    /// </summary>
    public class CoordinateSystemVisual3D : ModelVisual3D
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="ArrowLengths" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ArrowLengthsProperty = DependencyProperty.Register(
            "ArrowLengths",
            typeof(double),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(1.0, GeometryChanged));

        /// <summary>
        /// 标识<see cref="XAxisColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty XAxisColorProperty = DependencyProperty.Register(
            "XAxisColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(128, 0, 0)));

        /// <summary>
        /// 标识<see cref="YAxisColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty YAxisColorProperty = DependencyProperty.Register(
            "YAxisColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(0, 128, 0)));

        /// <summary>
        /// 标识<see cref="ZAxisColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ZAxisColorProperty = DependencyProperty.Register(
            "ZAxisColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(0, 0, 128)));

        /// <summary>
        /// 标识<see cref="XLabelText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty XLabelTextProperty = DependencyProperty.Register(
            "XLabelText",
            typeof(string),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata("X"));

        /// <summary>
        /// 标识<see cref="YLabelText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty YLabelTextProperty = DependencyProperty.Register(
            "YLabelText",
            typeof(string),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata("Y"));

        /// <summary>
        /// 标识<see cref="ZLabelText" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ZLabelTextProperty = DependencyProperty.Register(
            "ZLabelText",
            typeof(string),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata("Z"));

        /// <summary>
        /// 标识<see cref="XLabelColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty XLabelColorProperty = DependencyProperty.Register(
            "XLabelColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(255, 0, 0)));

        /// <summary>
        /// 标识<see cref="YLabelColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty YLabelColorProperty = DependencyProperty.Register(
            "YLabelColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(0, 221, 0))); // #00DD00 稍微降一点，否则浅色背景下太亮导致对比度不够

        /// <summary>
        /// 标识<see cref="ZLabelColor" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ZLabelColorProperty = DependencyProperty.Register(
            "ZLabelColor",
            typeof(Color),
            typeof(CoordinateSystemVisual3D),
            new UIPropertyMetadata(Color.FromRgb(0, 0, 255)));
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置箭头的长度。
        /// </summary>
        /// <value>箭头的长度。</value>
        public double ArrowLengths
        {
            get => (double)GetValue(ArrowLengthsProperty);
            set => SetValue(ArrowLengthsProperty, value);
        }

        /// <summary>
        /// 获取或设置X轴的颜色。
        /// </summary>
        /// <value>X轴的颜色。</value>
        public Color XAxisColor
        {
            get => (Color)GetValue(XAxisColorProperty);
            set => SetValue(XAxisColorProperty, value);
        }

        /// <summary>
        /// 获取或设置Y轴的颜色。
        /// </summary>
        /// <value>Y轴的颜色。</value>
        public Color YAxisColor
        {
            get => (Color)GetValue(YAxisColorProperty);
            set => SetValue(YAxisColorProperty, value);
        }

        /// <summary>
        /// 获取或设置Z轴的颜色。
        /// </summary>
        /// <value>Z轴的颜色。</value>
        public Color ZAxisColor
        {
            get => (Color)GetValue(ZAxisColorProperty);
            set => SetValue(ZAxisColorProperty, value);
        }

        /// <summary>
        /// 获取或设置X轴标签的文本。
        /// </summary>
        /// <value>X轴标签的文本。</value>
        public string XLabelText
        {
            get => (string)GetValue(XLabelTextProperty);
            set => SetValue(XLabelTextProperty, value);
        }

        /// <summary>
        /// 获取或设置Y轴标签的文本。
        /// </summary>
        /// <value>Y轴标签的文本。</value>
        public string YLabelText
        {
            get => (string)GetValue(YLabelTextProperty);
            set => SetValue(YLabelTextProperty, value);
        }

        /// <summary>
        /// 获取或设置Z轴标签的文本。
        /// </summary>
        /// <value>Z轴标签的文本。</value>
        public string ZLabelText
        {
            get => (string)GetValue(ZLabelTextProperty);
            set => SetValue(ZLabelTextProperty, value);
        }

        /// <summary>
        /// 获取或设置X轴标签的颜色。
        /// </summary>
        /// <value>X轴标签的颜色。</value>
        public Color XLabelColor
        {
            get => (Color)GetValue(XLabelColorProperty);
            set => SetValue(XLabelColorProperty, value);
        }

        /// <summary>
        /// 获取或设置Y轴标签的颜色。
        /// </summary>
        /// <value>Y轴标签的颜色。</value>
        public Color YLabelColor
        {
            get => (Color)GetValue(YLabelColorProperty);
            set => SetValue(YLabelColorProperty, value);
        }

        /// <summary>
        /// 获取或设置Z轴标签的颜色。
        /// </summary>
        /// <value>Z轴标签的颜色。</value>
        public Color ZLabelColor
        {
            get => (Color)GetValue(ZLabelColorProperty);
            set => SetValue(ZLabelColorProperty, value);
        }
        #endregion

        /// <summary>
        /// 初始化一个<see cref="CoordinateSystemVisual3D" />类的新实例。
        /// </summary>
        public CoordinateSystemVisual3D()
        {
            OnGeometryChanged();
        }

        /// <summary>
        /// 几何形状发生了变化。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="args">参数。</param>
        protected static void GeometryChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((CoordinateSystemVisual3D)obj).OnGeometryChanged();
        }

        /// <summary>
        /// 返回指定颜色的材质组。
        /// </summary>
        /// <param name="color">指定的颜色。</param>
        /// <returns>一个包含DiffuseMaterial和SpecularMaterial的材质组。</returns>
        private static MaterialGroup MaterialHelper(Color color)
        {
            DiffuseMaterial diffuse = new(new SolidColorBrush(color));
            SpecularMaterial specular = new(new SolidColorBrush(color), 2);
            MaterialGroup materials = new();
            materials.Children.Add(diffuse);
            materials.Children.Add(specular);
            return materials;
        }

        /// <summary>
        /// 当几何形状发生变化时被调用。
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            // 定义箭头的长度和直径
            Children.Clear();
            double l = ArrowLengths;
            double d = l * 0.1;

            // 创建X轴
            ArrowVisual3D xAxis = new();
            xAxis.BeginEdit();
            xAxis.Point2 = new Point3D(l, 0, 0);
            xAxis.Diameter = d;
            xAxis.Material = MaterialHelper(XAxisColor);
            xAxis.EndEdit();
            Children.Add(xAxis);

            // 创建Y轴
            ArrowVisual3D yAxis = new();
            yAxis.BeginEdit();
            yAxis.Point2 = new Point3D(0, l, 0);
            yAxis.Diameter = d;
            yAxis.Material = MaterialHelper(YAxisColor);
            yAxis.EndEdit();
            Children.Add(yAxis);

            // 创建Z轴
            ArrowVisual3D zAxis = new();
            zAxis.BeginEdit();
            zAxis.Point2 = new Point3D(0, 0, l);
            zAxis.Diameter = d;
            zAxis.Material = MaterialHelper(ZAxisColor);
            zAxis.EndEdit();
            Children.Add(zAxis);

            // 创建坐标原点
            SphereVisual3D origin = new();
            origin.BeginEdit();
            origin.Radius = d / 2;
            origin.Material = MaterialHelper(Colors.Black);
            origin.EndEdit();
            Children.Add(origin);

            // 对透明对象排序
            SortingVisual3D sorting = new()
            {
                SortingFrequency = 10
            };

            // 创建X轴标签
            BillboardTextVisual3D xLabel = new()
            {
                Position = new Point3D(l * 1.25, 0, 0),
                Text = XLabelText,
                Foreground = new SolidColorBrush(XLabelColor)
            };
            sorting.Children.Add(xLabel);

            // 创建Y轴标签
            BillboardTextVisual3D yLabel = new()
            {
                Position = new Point3D(0, l * 1.25, 0),
                Text = YLabelText,
                Foreground = new SolidColorBrush(YLabelColor)
            };
            sorting.Children.Add(yLabel);

            // 创建Z轴标签
            BillboardTextVisual3D zLabel = new()
            {
                Position = new Point3D(0, 0, l * 1.25),
                Text = ZLabelText,
                Foreground = new SolidColorBrush(ZLabelColor)
            };
            sorting.Children.Add(zLabel);

            Children.Add(sorting);
        }
    }
}