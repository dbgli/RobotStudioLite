using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for CellTextBox.xaml
    /// </summary>
    public partial class CellTextBox : UserControl
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="Text" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(CellTextBox),
            new FrameworkPropertyMetadata(string.Empty,
                                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置文本内容。
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        public CellTextBox()
        {
            InitializeComponent();
        }

        private async void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Visibility = Visibility.Visible;
            // 延时等待鼠标按下事件传播结束后再设置焦点，否则会立即失去焦点
            await Task.Delay(10);
            textBox.Focus();
            textBox.SelectAll();
        }

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // LostFocus事件先于设置为UpdateSourceTrigger=LostFocus的绑定验证规则，
            // 所以直接GetHasError总是返回上一次的验证结果，
            // 而手动触发验证效果又不太理想，所以延时等待验证完成后再获取
            await Task.Delay(10);
            if (Validation.GetHasError(this)) return;
            textBox.Visibility = Visibility.Collapsed;
        }
    }
}