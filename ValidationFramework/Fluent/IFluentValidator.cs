using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Interface for fluent validators that validate objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to validate</typeparam>
    public interface IFluentValidator<T>
    {
        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate</param>
        /// <returns>List of validation results</returns>
        List<ValidationResult> Validate(T instance);
    }
}
