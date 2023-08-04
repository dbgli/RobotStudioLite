using System.Globalization;
using System.Windows.Controls;

namespace DBGware.RobotStudioLite.ValidationRules
{
    public class NumberRangeValidationRule : ValidationRule
    {
        public double Minimum { get; set; } = double.MinValue;
        public double Maximum { get; set; } = double.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!double.TryParse((string)value, out double number) || number < Minimum || number > Maximum)
            {
                return new(false, string.Format((string)App.Current.FindResource("NumberRangeValidationResultErrorContent"),
                                                Minimum, Maximum));
            }
            return ValidationResult.ValidResult;
        }
    }
}