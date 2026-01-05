using ValidationFramework.Core;

namespace ValidationFramework.Core
{
    public class LengthValidator : IValidator
    {
        private readonly int _min;
        private readonly int _max;
        public LengthValidator(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is not string s)
                return ValidationResult.Fail(propertyName, $"{propertyName} must be a string.", value, "LENGTH_TYPE");
            if (s.Length < _min || s.Length > _max)
                return ValidationResult.Fail(propertyName, $"{propertyName} length must be between {_min} and {_max}.", value, "LENGTH");
            return ValidationResult.Ok(propertyName);
        }
    }
}