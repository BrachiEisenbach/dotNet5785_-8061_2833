using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var phone = value?.ToString();
            if (!string.IsNullOrWhiteSpace(phone) && Regex.IsMatch(phone, @"^0\d{1,2}-?\d{7}$"))
                return ValidationResult.ValidResult;

            return new ValidationResult(false, "Invalid phone number.");
        }
    }
}
