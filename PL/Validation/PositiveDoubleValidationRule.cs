using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{
    public class PositiveDoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (double.TryParse(value as string, out double number))
            {
                return number >= 0 ? ValidationResult.ValidResult : new ValidationResult(false, "Must be non-negative");
            }
            return new ValidationResult(false, "Invalid number");
        }
    }
}


