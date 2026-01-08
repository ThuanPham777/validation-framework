using System.Linq.Expressions;
using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Fluent API builder for creating validators for type T.
    /// </summary>
    /// <typeparam name="T">The type to validate</typeparam>
    public class ValidatorBuilder<T> : IFluentValidator<T>
    {
        private readonly List<object> _propertyValidators = new();

        /// <summary>
        /// Specifies a property to validate.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="propertyExpression">Expression pointing to the property</param>
        /// <returns>PropertyValidator for chaining validation rules</returns>
        public PropertyValidator<T, TProperty> For<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyValidator = new PropertyValidator<T, TProperty>(propertyExpression);
            _propertyValidators.Add(propertyValidator);
            return propertyValidator;
        }

        /// <summary>
        /// Builds and returns the fluent validator.
        /// </summary>
        /// <returns>An IFluentValidator instance</returns>
        public IFluentValidator<T> Build()
        {
            return this;
        }

        /// <summary>
        /// Validates the specified instance against all configured rules.
        /// </summary>
        /// <param name="instance">The instance to validate</param>
        /// <returns>List of validation results</returns>
        public List<ValidationResult> Validate(T instance)
        {
            var results = new List<ValidationResult>();

            foreach (var validator in _propertyValidators)
            {
                // Use reflection to call Validate method on the property validator
                var validateMethod = validator.GetType().GetMethod("Validate",
       System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (validateMethod != null)
                {
                    var result = validateMethod.Invoke(validator, new object[] { instance! });
                    if (result is List<ValidationResult> validationResults)
                    {
                        results.AddRange(validationResults);
                    }
                }
            }

            return results;
        }
    }
}
