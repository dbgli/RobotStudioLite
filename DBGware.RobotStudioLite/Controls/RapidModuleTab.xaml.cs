using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBGware.RobotStudioLite.Controls
{
    /// <summary>
    /// Interaction logic for RapidModuleTab.xaml
    /// </summary>
    public partial class RapidModuleTab : UserControl
    {
        public RapidModuleTab(string filePath)
        {
            InitializeComponent();

            using StreamReader streamReader = new(filePath);
            textEditor.Text = streamReader.ReadToEnd();
        }
    }
}