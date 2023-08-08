using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DBGware.RobotStudioLite.Converters
{
    public class IndexToRowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ListViewItem listViewItem) return string.Empty;
            ListView listView = (ListView)ItemsControl.ItemsControlFromItemContainer(listViewItem);
            int rowNumber = listView.ItemContainerGenerator.IndexFromContainer(listViewItem) + 1;
            return rowNumber.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}