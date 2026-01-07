using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Interface for fluent validation rule that can validate a specific property value.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public interface IFluentRule<T, TProperty>
    {
        /// <summary>
        /// Validates the property value and returns the result.
        /// </summary>
        ValidationResult Validate(TProperty value, string propertyName);

        /// <summary>
        /// Gets or sets the custom error message for this rule.
        /// </summary>
        string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets the error code for this rule.
        /// </summary>
        string ErrorCode { get; }
    }
}
