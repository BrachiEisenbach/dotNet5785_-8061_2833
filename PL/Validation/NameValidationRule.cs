using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var name = value?.ToString();
            if (!string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Name must contain only letters.");
        }
    }
}
