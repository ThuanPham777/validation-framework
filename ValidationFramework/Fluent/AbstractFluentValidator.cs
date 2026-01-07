using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Abstract base class for defining fluent validators using inheritance.
    /// Similar to FluentValidation's AbstractValidator pattern.
    /// </summary>
    /// <typeparam name="T">The type of the model to validate.</typeparam>
    public abstract class AbstractFluentValidator<T> where T : class
    {
        private readonly ValidatorBuilder<T> _builder = new();
        private FluentValidator<T>? _validator;

        protected AbstractFluentValidator()
        {
            // Derived class will call RuleFor in its constructor
        }

        /// <summary>
        /// Defines validation rules for a specific property.
        /// </summary>
        protected IRuleBuilder<T, TProperty> RuleFor<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> propertyExpression)
        {
            return _builder.For(propertyExpression);
        }

        /// <summary>
        /// Validates the model and returns all validation results.
        /// </summary>
        public List<ValidationResult> Validate(T instance)
        {
            EnsureValidatorBuilt();
            return _validator!.Validate(instance);
        }

        /// <summary>
        /// Validates the model and returns true if valid.
        /// </summary>
        public bool IsValid(T instance)
        {
            EnsureValidatorBuilt();
            return _validator!.IsValid(instance);
        }

        /// <summary>
        /// Validates the model and throws ValidationException if invalid.
        /// </summary>
        public void ValidateAndThrow(T instance)
        {
            EnsureValidatorBuilt();
            _validator!.ValidateAndThrow(instance);
        }

        /// <summary>
        /// Gets the underlying FluentValidator instance.
        /// </summary>
        public FluentValidator<T> GetValidator()
        {
            EnsureValidatorBuilt();
            return _validator!;
        }

        private void EnsureValidatorBuilt()
        {
            _validator ??= _builder.Build();
        }
    }
}
