using ValidationFramework.Core;

namespace ValidationFramework.Demo
{
    // Custom validator: kiểm tra chuỗi chỉ chứa chữ cái
    public class AlphaOnlyValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && !System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z]+$"))
                return ValidationResult.Fail(propertyName, $"{propertyName} chỉ được chứa chữ cái.");
            return ValidationResult.Ok(propertyName);
        }
    }

    // Custom validator: kiểm tra chuỗi không chứa ký tự đặc biệt
    public class NoSpecialCharValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"[^a-zA-Z0-9]"))
                return ValidationResult.Fail(propertyName, $"{propertyName} không được chứa ký tự đặc biệt.");
            return ValidationResult.Ok(propertyName);
        }
    }
}
