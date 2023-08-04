using System.Globalization;
using System.Windows.Controls;

namespace DBGware.RobotStudioLite.ValidationRules
{
    public class IntegerRangeValidationRule : ValidationRule
    {
        public int Minimum { get; set; } = int.MinValue;
        public int Maximum { get; set; } = int.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse((string)value, out int integer) || integer < Minimum || integer > Maximum)
            {
                return new(false, string.Format((string)App.Current.FindResource("IntegerRangeValidationResultErrorContent"),
                                                Minimum, Maximum));
            }
            return ValidationResult.ValidResult;
        }
    }
}