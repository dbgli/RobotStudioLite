using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DBGware.RobotStudioLite.SQLite;
using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : CustomChromeWindow
    {
        private static readonly string databasePath = @$"{AppDomain.CurrentDomain.BaseDirectory}Users\Users.db";

        public ObservableCollection<string> Usernames { get; set; } = new();
        public string SelectedUsername { get; set; } = string.Empty;

        public SignInWindow()
        {
            InitializeComponent();

            // 如果数据库文件不存在，则创建数据库文件，并添加一个默认的管理员用户
            if (!File.Exists(databasePath))
            {
                File.Create(databasePath).Close();
                using SQLiteConnection connection = new(databasePath);
                connection.CreateTable<User>();
                User adminUser = new() { ID = Guid.NewGuid(), Username = "admin", Password = "admin", IsAdmin = true };
                connection.Insert(adminUser);
            }

            // 读取所有用户名，添加到Usernames里
            {
                using SQLiteConnection connection = new(databasePath);
                connection.Table<User>().ToList().ForEach(u => Usernames.Add(u.Username));
            }
        }

        #region 登录命令

        private void SignInCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            using SQLiteConnection connection = new(databasePath);
            User? user = connection.Table<User>().FirstOrDefault(u => u.Username == SelectedUsername);
            // 出于安全考虑，PasswordBox的Password并不是依赖属性，不能绑定
            if (user?.Password == passwordBox.Password)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show((string)App.Current.FindResource("UsernameOrPasswordIncorrectMessage"),
                                "RobotStudioLite",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information,
                                MessageBoxResult.OK);
                passwordBox.Password = string.Empty;
            }
        }

        private void SignInCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}