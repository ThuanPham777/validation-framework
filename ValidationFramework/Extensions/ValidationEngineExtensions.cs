using ValidationFramework.Core;
using ValidationFramework.Fluent;
using ValidationFramework.Result;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Extension methods for ValidationEngine to support fluent validators.
    /// </summary>
    public static class ValidationEngineExtensions
    {
        private static readonly Dictionary<Type, object> _fluentValidators = new();

        /// <summary>
        /// Registers a fluent validator for type T.
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="engine">The validation engine</param>
        /// <param name="validator">The fluent validator</param>
        public static void AddFluentValidator<T>(this ValidationEngine engine, IFluentValidator<T> validator)
        {
            _fluentValidators[typeof(T)] = validator;
        }

        /// <summary>
        /// Registers a fluent validator using a builder configuration action.
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="engine">The validation engine</param>
        /// <param name="configure">Configuration action for the builder</param>
        public static void AddFluentValidator<T>(this ValidationEngine engine, Action<ValidatorBuilder<T>> configure)
        {
            var builder = new ValidatorBuilder<T>();
            configure(builder);
            var validator = builder.Build();
            _fluentValidators[typeof(T)] = validator;
        }

        /// <summary>
        /// Validates an instance using fluent validator if registered, otherwise uses attribute-based validation.
        /// </summary>
        /// <typeparam name="T">The type to validate</typeparam>
        /// <param name="engine">The validation engine</param>
        /// <param name="instance">The instance to validate</param>
        /// <returns>List of validation results</returns>
        public static List<ValidationResult> ValidateWithFluent<T>(this ValidationEngine engine, T instance)
        {
            var results = new List<ValidationResult>();

            // First, run fluent validation if registered
            if (_fluentValidators.TryGetValue(typeof(T), out var validator))
            {
                if (validator is IFluentValidator<T> fluentValidator)
                {
                    results.AddRange(fluentValidator.Validate(instance!));
                }
            }

            // Then, run attribute-based and custom validators from engine
            results.AddRange(engine.Validate(instance!));

            return results;
        }

        /// <summary>
        /// Gets the registered fluent validator for type T if exists.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The fluent validator or null</returns>
        public static IFluentValidator<T>? GetFluentValidator<T>()
        {
            if (_fluentValidators.TryGetValue(typeof(T), out var validator))
            {
                return validator as IFluentValidator<T>;
            }
            return null;
        }

        /// <summary>
        /// Clears all registered fluent validators.
        /// </summary>
        public static void ClearFluentValidators()
        {
            _fluentValidators.Clear();
        }
    }
}
