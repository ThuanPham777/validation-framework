using ValidationFramework.Core;

namespace ValidationFramework.Core
{
    public class RequiredValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
            {
                return ValidationResult.Fail(propertyName, $"{propertyName} is required.", value, "REQUIRED");
            }
            return ValidationResult.Ok(propertyName);
        }
    }
}