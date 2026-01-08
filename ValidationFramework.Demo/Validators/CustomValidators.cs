using ValidationFramework.Validator;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.Validators
{
    /// <summary>
    /// Custom validator: Only letters allowed (a-z, A-Z)
    /// </summary>
    public class AlphaOnlyValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && !System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z]+$"))
                return ValidationResult.Fail(propertyName, "Must contain only letters", value, "ALPHA_ONLY");
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Custom validator: No special characters allowed
    /// </summary>
    public class NoSpecialCharValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"[^a-zA-Z0-9]"))
                return ValidationResult.Fail(propertyName, "Must not contain special characters", value, "NO_SPECIAL_CHARS");
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Custom validator: No digits allowed
    /// </summary>
    public class NoDigitValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"\d"))
                return ValidationResult.Fail(propertyName, "Must not contain digits", value, "NO_DIGITS");
            return ValidationResult.Ok(propertyName);
        }
    }
}
