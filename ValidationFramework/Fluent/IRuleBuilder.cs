using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Interface for a property rule builder that allows chaining validation rules.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public interface IRuleBuilder<T, TProperty>
    {
        /// <summary>
        /// Adds a required validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> Required();

        /// <summary>
        /// Adds an email validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> Email();

        /// <summary>
        /// Adds a length validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> Length(int min, int max);

        /// <summary>
        /// Adds a minimum length validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> MinLength(int min);

        /// <summary>
        /// Adds a maximum length validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> MaxLength(int max);

        /// <summary>
        /// Adds a regex pattern validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> Matches(string pattern);

        /// <summary>
        /// Adds a phone number validation rule.
        /// </summary>
        IRuleBuilder<T, TProperty> Phone();

        /// <summary>
        /// Adds a custom validation rule using a predicate.
        /// </summary>
        IRuleBuilder<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage, string? errorCode = null);

        /// <summary>
        /// Adds a custom validation rule using a delegate that returns ValidationResult.
        /// </summary>
        IRuleBuilder<T, TProperty> Custom(Func<TProperty, string, ValidationResult> validator);

        /// <summary>
        /// Sets a custom error message for the last added rule.
        /// </summary>
        IRuleBuilder<T, TProperty> WithMessage(string message);

        /// <summary>
        /// Sets a custom error code for the last added rule.
        /// </summary>
        IRuleBuilder<T, TProperty> WithErrorCode(string errorCode);

        /// <summary>
        /// Adds a condition to only apply the last rule when the predicate is true.
        /// </summary>
        IRuleBuilder<T, TProperty> When(Func<T, bool> condition);
    }
}
