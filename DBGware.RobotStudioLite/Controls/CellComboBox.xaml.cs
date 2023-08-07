using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for CellComboBox.xaml
    /// </summary>
    public partial class CellComboBox : UserControl
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="ItemsSource" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(CellComboBox),
            new FrameworkPropertyMetadata(null,
                                          FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// 标识<see cref="SelectedItem" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(CellComboBox),
            new FrameworkPropertyMetadata(null,
                                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置项目源。
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// 获取或设置选定项。
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        #endregion

        public CellComboBox()
        {
            InitializeComponent();
        }

        private async void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            comboBox.Visibility = Visibility.Visible;
            // 延时等待鼠标按下事件传播结束后再打开下拉菜单，否则会立即关闭
            await Task.Delay(10);
            comboBox.IsDropDownOpen = true;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            comboBox.Visibility = Visibility.Collapsed;
        }
    }
}