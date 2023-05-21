using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DBGware.RobotStudioLite.UI.Controls
{
    internal static class BooleanBoxes
    {
        public static readonly object TrueBox = true;
        public static readonly object FalseBox = false;

        public static object Box(bool value)
        {
            return value ? TrueBox : FalseBox;
        }

        public static object? Box(bool? value)
        {
            return value.HasValue ? value.Value ? TrueBox : FalseBox : null;
        }
    }

    internal static class ObjectBoxes
    {
        public static readonly object NullBox = null!;
        public static readonly object EmptyBox = string.Empty;
        public static readonly object ThicknessBox = default(Thickness);
        public static readonly object TransparentBox = Brushes.Transparent;
    }
}