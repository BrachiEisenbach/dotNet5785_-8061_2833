using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{
    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;

            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult(false, "הסיסמה לא יכולה להיות ריקה");
            }
            if (password.Length < 6)
            {
                return new ValidationResult(false, "הסיסמה חייבת להכיל לפחות 6 תווים");
            }

            // אפשר להוסיף כאן בדיקות מורכבות יותר לפי צורך, כמו אותיות גדולות, מספרים וכו'

            return ValidationResult.ValidResult;
        }
    }
}
