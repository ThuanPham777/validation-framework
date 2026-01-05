using ValidationFramework.Result;

namespace ValidationFramework.Validator
{
    public interface IValidator
    {
        ValidationResult Validate(object value, string propertyName);
    }
}