namespace ValidationFramework.Core
{
    public interface IValidator
    {
        ValidationResult Validate(object value, string propertyName);
    }
}