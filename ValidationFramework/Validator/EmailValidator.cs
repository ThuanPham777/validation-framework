using System.Text.RegularExpressions;
using ValidationFramework.Core;

namespace ValidationFramework.Core
{
    public class EmailValidator : IValidator
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return ValidationResult.Fail(propertyName, $"{propertyName} must be a valid email.", value, "EMAIL");
            if (!EmailRegex.IsMatch(s))
                return ValidationResult.Fail(propertyName, $"{propertyName} is not a valid email format.", value, "EMAIL_FORMAT");
            return ValidationResult.Ok(propertyName);
        }
    }
}