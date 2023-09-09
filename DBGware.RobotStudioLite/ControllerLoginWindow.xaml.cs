using System.Windows.Input;
using ABB.Robotics.Controllers;
using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for ControllerLoginWindow.xaml
    /// </summary>
    public partial class ControllerLoginWindow : CustomChromeWindow
    {
        public string Username { get; set; } = string.Empty;
        public UserInfo UserInfo { get; set; } = new();
        public bool IsLocalClient { get; set; } = false;

        public ControllerLoginWindow(string controllerName)
        {
            InitializeComponent();
            string title = $"{App.Current.FindResource("Login")}: {controllerName}";
            Title = title;
            titleTextBlock.Text = title;
        }

        #region 以默认用户身份登录命令

        private void LoginAsDefaultUserCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // DefaultUsername: Default User
            // DefaultPassword: robotics
            // DefaultApplication: PCSDK
            UserInfo = IsLocalClient ? UserInfo.DefaultLocalUser : UserInfo.DefaultUser;
            DialogResult = true;
        }

        private void LoginAsDefaultUserCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region 登录控制器命令

        private void LoginControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            UserInfo = new(Username, passwordBox.Password)
            {
                Local = IsLocalClient
            };
            DialogResult = true;
        }

        private void LoginControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(Username)
                        && !string.IsNullOrEmpty(passwordBox.Password);
        }

        #endregion

        #region 取消登录控制器命令

        private void CancelLoginControllerCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CancelLoginControllerCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}