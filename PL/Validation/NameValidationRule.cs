using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PL.Validation
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var name = value?.ToString();

            // מאפשר רק אותיות ורווחים (רווח בודד בין מילים)
            if (!string.IsNullOrWhiteSpace(name) &&
                Regex.IsMatch(name, @"^[\p{L}]+(\s[\p{L}]+)*$"))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Name must contain only letters and spaces.");
        }
    }
}
