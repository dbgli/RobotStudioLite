using System;
using System.Globalization;
using System.Windows.Data;
using DBGware.RobotStudioLite.Utilities;

namespace DBGware.RobotStudioLite.Converters
{
    public class CategoryIdToCategoryTypeExConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int categoryId) return CategoryTypeEx.Common;
            CategoryTypeEx categoryTypeEx;
            try
            {
                categoryTypeEx = (CategoryTypeEx)categoryId;
            }
            catch
            {
                categoryTypeEx = CategoryTypeEx.Common;
            }
            return categoryTypeEx;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}