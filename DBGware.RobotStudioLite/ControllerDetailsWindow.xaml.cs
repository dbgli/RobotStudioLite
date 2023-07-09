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
using System.Windows.Shapes;

using DBGware.RobotStudioLite.UI.Controls;

namespace DBGware.RobotStudioLite
{
    /// <summary>
    /// Interaction logic for ControllerDetailsWindow.xaml
    /// </summary>
    public partial class ControllerDetailsWindow : CustomChromeWindow
    {
        public ControllerDetailsWindow(string controllerDetails)
        {
            InitializeComponent();
            controllerDetailsTextBox.Text = controllerDetails;
        }
    }
}