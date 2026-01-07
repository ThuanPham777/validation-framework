using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    public interface IFluentValidator<T> where T : class
    {
        List<ValidationResult> Validate(T instance);
        bool IsValid(T instance);
        void ValidateAndThrow(T instance);
    }
}
