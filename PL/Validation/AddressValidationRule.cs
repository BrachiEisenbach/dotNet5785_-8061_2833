using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL.Validation
{
    public class AddressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string address = value as string;

            if (string.IsNullOrWhiteSpace(address))
            {
                return new ValidationResult(false, "כתובת לא יכולה להיות ריקה");
            }

            // אפשר להוסיף כאן בדיקות נוספות כמו אורך מינימלי, תווים מותריים וכו'

            return ValidationResult.ValidResult;
        }
    }
}
