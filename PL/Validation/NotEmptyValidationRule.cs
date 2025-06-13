using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value?.ToString();
            if (!string.IsNullOrWhiteSpace(text))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Field is required.");
        }
    }
}
