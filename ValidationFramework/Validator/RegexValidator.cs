using System.Text.RegularExpressions;
using ValidationFramework.Result;

namespace ValidationFramework.Validator
{
    public class RegexValidator : IValidator
    {
        private readonly Regex _regex;
        private readonly string _pattern;
        public RegexValidator(string pattern)
        {
            _pattern = pattern;
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is not string s)
                return ValidationResult.Fail(propertyName, $"{propertyName} must be a string.", value, "REGEX_TYPE");
            if (!_regex.IsMatch(s))
                return ValidationResult.Fail(propertyName, $"{propertyName} does not match pattern '{_pattern}'.", value, "REGEX");
            return ValidationResult.Ok(propertyName);
        }
    }
}