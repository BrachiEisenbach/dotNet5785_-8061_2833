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

            if (!string.IsNullOrWhiteSpace(phone))
            {
                // לבדוק שהמחרוזת מכילה רק ספרות ומקפים
                if (Regex.IsMatch(phone, @"^[0-9\-]+$"))
                {
                    // לספור רק את הספרות
                    int digitCount = phone.Count(char.IsDigit);

                    // לבדוק אם יש 9 או 10 ספרות
                    if (digitCount == 9 || digitCount == 10)
                        return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, "Invalid phone number.");
        }
    }
}

