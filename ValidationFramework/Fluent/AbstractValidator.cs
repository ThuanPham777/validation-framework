using System.Linq.Expressions;
using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Abstract base class for creating fluent validators.
    /// Inherit from this class and configure rules in the constructor.
    /// </summary>
    /// <typeparam name="T">The type to validate</typeparam>
    public abstract class AbstractValidator<T> : IFluentValidator<T>
    {
        private readonly ValidatorBuilder<T> _builder = new();

        /// <summary>
        /// Constructor that should be used to configure validation rules.
        /// Call RuleFor to define rules.
        /// </summary>
        protected AbstractValidator()
        {
            // Derived classes will call RuleFor in their constructor
        }

        /// <summary>
        /// Defines a validation rule for a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="propertyExpression">Expression pointing to the property</param>
        /// <returns>PropertyValidator for chaining validation rules</returns>
        protected PropertyValidator<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return _builder.For(propertyExpression);
        }

        /// <summary>
        /// Validates the specified instance against all configured rules.
        /// </summary>
        /// <param name="instance">The instance to validate</param>
        /// <returns>List of validation results</returns>
        public List<ValidationResult> Validate(T instance)
        {
            return _builder.Validate(instance);
        }
    }
}
