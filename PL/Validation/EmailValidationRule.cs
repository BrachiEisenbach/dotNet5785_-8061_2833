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
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Email is required.");

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return new ValidationResult(false, "Invalid email format.");

            return ValidationResult.ValidResult;
        }
    }
}
