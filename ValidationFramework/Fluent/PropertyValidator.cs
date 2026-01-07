using System.Linq.Expressions;
using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Represents a validator for a specific property of type T.
    /// </summary>
    /// <typeparam name="T">The type containing the property</typeparam>
    /// <typeparam name="TProperty">The type of the property</typeparam>
    public class PropertyValidator<T, TProperty>
    {
        private readonly Expression<Func<T, TProperty>> _propertyExpression;
        private readonly string _propertyName;
        private readonly List<IValidator> _validators = new();
        private string? _customMessage;

        internal PropertyValidator(Expression<Func<T, TProperty>> propertyExpression)
        {
            _propertyExpression = propertyExpression;
            _propertyName = GetPropertyName(propertyExpression);
        }

        private static string GetPropertyName(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            throw new ArgumentException("Expression must be a property access expression", nameof(expression));
        }

        /// <summary>
        /// Adds a Required validation rule.
        /// </summary>
        public PropertyValidator<T, TProperty> Required()
        {
            _validators.Add(new RequiredValidator());
            return this;
        }

        /// <summary>
        /// Adds an Email validation rule.
        /// </summary>
        public PropertyValidator<T, TProperty> Email()
        {
            _validators.Add(new EmailValidator());
            return this;
        }

        /// <summary>
        /// Adds a Length validation rule.
        /// </summary>
        public PropertyValidator<T, TProperty> Length(int min, int max)
        {
            _validators.Add(new LengthValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds a Regex validation rule.
        /// </summary>
        public PropertyValidator<T, TProperty> Regex(string pattern)
        {
            _validators.Add(new RegexValidator(pattern));
            return this;
        }

        /// <summary>
        /// Adds a Phone validation rule.
        /// </summary>
        public PropertyValidator<T, TProperty> Phone()
        {
            _validators.Add(new PhoneValidator());
            return this;
        }

        /// <summary>
        /// Adds a custom validation rule using a predicate.
        /// </summary>
        public PropertyValidator<T, TProperty> Custom(Func<TProperty?, bool> predicate, string? errorMessage = null)
        {
            _validators.Add(new DelegateValidator((value, propertyName) =>
                 {
                     var typedValue = value is TProperty prop ? prop : default;
                     if (!predicate(typedValue))
                     {
                         var message = errorMessage ?? _customMessage ?? $"{propertyName} is invalid.";
                         return ValidationResult.Fail(propertyName, message, value);
                     }
                     return ValidationResult.Ok(propertyName);
                 }));
            return this;
        }

        /// <summary>
        /// Adds a custom validation rule using a full validator function.
        /// </summary>
        public PropertyValidator<T, TProperty> Custom(Func<TProperty?, string, ValidationResult> validatorFunc)
        {
            _validators.Add(new DelegateValidator((value, propertyName) =>
                   {
                       var typedValue = value is TProperty prop ? prop : default;
                       return validatorFunc(typedValue, propertyName);
                   }));
            return this;
        }

        /// <summary>
        /// Sets a custom error message for the next validator added.
        /// </summary>
        public PropertyValidator<T, TProperty> WithMessage(string message)
        {
            _customMessage = message;

            // Apply message to the last added validator if it supports custom messages
            if (_validators.Count > 0)
            {
                var lastValidator = _validators[^1];

                // Wrap the last validator with a custom message validator
                var wrappedValidator = new DelegateValidator((value, propertyName) =>
                  {
                      var result = lastValidator.Validate(value, propertyName);
                      if (!result.IsValid)
                      {
                          return ValidationResult.Fail(propertyName, message, result.AttemptedValue, result.ErrorCode);
                      }
                      return result;
                  });

                _validators[^1] = wrappedValidator;
            }

            return this;
        }

        /// <summary>
        /// Validates the property value extracted from the instance.
        /// </summary>
        internal List<ValidationResult> Validate(T instance)
        {
            var results = new List<ValidationResult>();
            var compiled = _propertyExpression.Compile();
            var value = compiled(instance);

            foreach (var validator in _validators)
            {
                var result = validator.Validate(value, _propertyName);
                if (!result.IsValid)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        internal string PropertyName => _propertyName;
    }
}
