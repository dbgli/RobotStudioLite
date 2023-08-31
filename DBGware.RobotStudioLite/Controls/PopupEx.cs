using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;

namespace DBGware.RobotStudioLite.Controls
{
    public class PopupEx : Popup
    {
        #region 依赖属性
        /// <summary>
        /// 标识<see cref="IsPositionUpdatable" />依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsPositionUpdatableProperty = DependencyProperty.Register(
            "IsPositionUpdatable",
            typeof(bool),
            typeof(PopupEx),
            new FrameworkPropertyMetadata(true, IsPositionUpdatableChanged));

        /// <summary>
        /// 标识<see cref="Topmost" />依赖属性。
        /// </summary>
        public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(
            typeof(Popup),
            new FrameworkPropertyMetadata(false, OnTopmostChanged));
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置一个值，该值指示PopupEx的位置能否更新，默认值为true。
        /// </summary>
        public bool IsPositionUpdatable
        {
            get => (bool)GetValue(IsPositionUpdatableProperty);
            set => SetValue(IsPositionUpdatableProperty, value);
        }

        /// <summary>
        /// 获取或设置一个值，该值指示PopupEx是否显示在最顶层，默认值为false。
        /// </summary>
        public bool Topmost
        {
            get => (bool)GetValue(TopmostProperty);
            set => SetValue(TopmostProperty, value);
        }
        #endregion

        /// <summary>
        /// 初始化一个<see cref="PopupEx" />类的新实例。
        /// </summary>
        public PopupEx()
        {
            Loaded += PopupEx_Loaded;
        }

        private static void IsPositionUpdatableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupEx)d).PopupEx_Loaded((PopupEx)d, null);
        }

        private void PopupEx_Loaded(object sender, RoutedEventArgs? e)
        {
            Popup popup = (Popup)sender;
            DependencyObject? parent = VisualTreeHelper.GetParent(popup);

            // 一直向上找父对象
            while (parent is not null and not Window)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Window window)
            {
                window.LocationChanged -= PositionChanged;
                window.SizeChanged -= PositionChanged;
                if (IsPositionUpdatable)
                {
                    window.LocationChanged += PositionChanged;
                    window.SizeChanged += PositionChanged;
                }
            }
        }

        private void PositionChanged(object? sender, EventArgs e)
        {
            try
            {
                // 利用反射调用非公开方法，当窗口位置或尺寸改变时更新PopupEx的位置
                MethodInfo? method = typeof(Popup).GetMethod("UpdatePosition", BindingFlags.NonPublic | BindingFlags.Instance);
                if (IsOpen)
                {
                    method?.Invoke(this, null);
                }
            }
            catch
            {
                return;
            }
        }

        private static void OnTopmostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PopupEx)d).UpdateWindow();
        }

        /// <summary>
        /// 重写打开方法。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }

        /// <summary>
        /// 更新PopupEx层级。
        /// </summary>
        private void UpdateWindow()
        {
            IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(Child)).Handle;
            if (NativeMethods.GetWindowRect(hwnd, out RECT rect))
            {
                NativeMethods.SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, (int)Width, (int)Height, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #region P/Invoke导入与定义
        public static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
            [DllImport("user32", EntryPoint = "SetWindowPos")]
            internal static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        }
        #endregion
    }
}