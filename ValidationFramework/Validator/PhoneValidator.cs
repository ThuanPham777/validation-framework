using System.Text.RegularExpressions;
using ValidationFramework.Result;

namespace ValidationFramework.Validator
{
    public class PhoneValidator : IValidator
    {
        private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{7,15}$", RegexOptions.Compiled);

        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return ValidationResult.Fail(propertyName, $"{propertyName} must be a valid phone number.", value, "PHONE");
            if (!PhoneRegex.IsMatch(s))
                return ValidationResult.Fail(propertyName, $"{propertyName} is not a valid phone number format.", value, "PHONE_FORMAT");
            return ValidationResult.Ok(propertyName);
        }
    }
}