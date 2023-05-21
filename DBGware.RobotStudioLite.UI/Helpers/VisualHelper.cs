using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DBGware.RobotStudioLite.UI.Helpers
{
    internal static class VisualHelper
    {
        public static T? GetParent<T>(DependencyObject elem) where T : DependencyObject
        {
            for (var parent = VisualTreeHelper.GetParent(elem); parent != null; parent = VisualTreeHelper.GetParent(parent))
            {
                if (parent is T tmp)
                {
                    return tmp;
                }
            }

            return null;
        }
    }
}