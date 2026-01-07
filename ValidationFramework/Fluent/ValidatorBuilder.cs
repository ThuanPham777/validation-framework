using System.Linq.Expressions;
using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Fluent validator builder for creating validation rules for a model type.
    /// </summary>
    /// <typeparam name="T">The type of the model to validate.</typeparam>
    public class ValidatorBuilder<T> where T : class
    {
        private readonly List<object> _propertyRules = new();

        /// <summary>
        /// Defines validation rules for a specific property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">Expression selecting the property.</param>
        /// <returns>A rule builder to chain validation rules.</returns>
        public IRuleBuilder<T, TProperty> For<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            var accessor = propertyExpression.Compile();

            var propertyRule = new PropertyRule<T, TProperty>(propertyName, accessor);
            _propertyRules.Add(propertyRule);

            return new RuleBuilder<T, TProperty>(propertyRule);
        }

        /// <summary>
        /// Builds and returns a FluentValidator instance.
        /// </summary>
        public FluentValidator<T> Build()
        {
            return new FluentValidator<T>(_propertyRules);
        }

        private static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                return memberExpression.Member.Name;

            if (expression.Body is UnaryExpression unaryExpression &&
      unaryExpression.Operand is MemberExpression operandMember)
                return operandMember.Member.Name;

            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        }
    }

    /// <summary>
    /// A fluent validator that validates models using configured rules.
    /// </summary>
    /// <typeparam name="T">The type of the model to validate.</typeparam>
    public sealed class FluentValidator<T> where T : class
    {
        private readonly List<object> _propertyRules;

        internal FluentValidator(List<object> propertyRules)
        {
            _propertyRules = propertyRules;
        }

        /// <summary>
        /// Validates the model and returns all validation results.
        /// </summary>
        public List<ValidationResult> Validate(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var results = new List<ValidationResult>();

            foreach (var rule in _propertyRules)
            {
                // Use reflection to call Validate on the generic PropertyRule
                var validateMethod = rule.GetType().GetMethod("Validate");
                if (validateMethod != null)
                {
                    var ruleResults = validateMethod.Invoke(rule, new object[] { instance }) as List<ValidationResult>;
                    if (ruleResults != null)
                        results.AddRange(ruleResults);
                }
            }

            return results;
        }

        /// <summary>
        /// Validates the model and returns true if valid.
        /// </summary>
        public bool IsValid(T instance)
        {
            return Validate(instance).Count == 0;
        }

        /// <summary>
        /// Validates the model and throws ValidationException if invalid.
        /// </summary>
        public void ValidateAndThrow(T instance)
        {
            var results = Validate(instance);
            if (results.Count > 0)
                throw new ValidationException(results);
        }
    }

    /// <summary>
    /// Exception thrown when validation fails.
    /// </summary>
    public sealed class ValidationException : Exception
    {
        public List<ValidationResult> Errors { get; }

        public ValidationException(List<ValidationResult> errors)
       : base($"Validation failed with {errors.Count} error(s).")
        {
            Errors = errors;
        }

        public override string ToString()
        {
            var errorMessages = string.Join(Environment.NewLine, Errors.Select(e => $"- {e.PropertyName}: {e.Message}"));
            return $"Validation failed:{Environment.NewLine}{errorMessages}";
        }
    }
}
