using ValidationFramework.Validator;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.WinUI.Validators
{
    // Custom validator: Check if string does not contain special characters
    public class NoSpecialCharValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"[^a-zA-Z0-9]"))
                return ValidationResult.Fail(propertyName, $"{propertyName} must not contain special characters.", value, "NO_SPECIAL_CHAR");
            return ValidationResult.Ok(propertyName);
        }
    }

    // Custom validator: Check if password contains at least one uppercase, one lowercase, and one digit
    public class StrongPasswordValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return ValidationResult.Ok(propertyName);

            if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"[A-Z]"))
                return ValidationResult.Fail(propertyName, $"{propertyName} must contain at least one uppercase letter.", value, "STRONG_PASSWORD_UPPER");

            if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"[a-z]"))
                return ValidationResult.Fail(propertyName, $"{propertyName} must contain at least one lowercase letter.", value, "STRONG_PASSWORD_LOWER");

            if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"\d"))
                return ValidationResult.Fail(propertyName, $"{propertyName} must contain at least one digit.", value, "STRONG_PASSWORD_DIGIT");

            return ValidationResult.Ok(propertyName);
        }
    }
}
