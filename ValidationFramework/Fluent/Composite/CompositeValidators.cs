using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Fluent.Composite
{
    /// <summary>
    /// Combines multiple validators into a single validator with AND logic.
    /// All validators must pass for the validation to succeed.
    /// </summary>
    public class AndValidator : IValidator
    {
        private readonly List<IValidator> _validators = new();

        public AndValidator(params IValidator[] validators)
        {
            _validators.AddRange(validators);
        }

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            foreach (var validator in _validators)
            {
                var result = validator.Validate(value, propertyName);
                if (!result.IsValid)
                    return result; // Return first failure
            }
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Combines multiple validators into a single validator with OR logic.
    /// At least one validator must pass for the validation to succeed.
    /// </summary>
    public class OrValidator : IValidator
    {
        private readonly List<IValidator> _validators = new();

        public OrValidator(params IValidator[] validators)
        {
            _validators.AddRange(validators);
        }

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            if (_validators.Count == 0)
                return ValidationResult.Ok(propertyName);

            var errors = new List<string>();

            foreach (var validator in _validators)
            {
                var result = validator.Validate(value, propertyName);
                if (result.IsValid)
                    return result; // Return first success

                errors.Add(result.Message);
            }

            // All failed
            var combinedMessage = $"{propertyName} failed all validation rules: {string.Join("; ", errors)}";
            return ValidationResult.Fail(propertyName, combinedMessage, value, "OR_VALIDATOR_FAILED");
        }
    }

    /// <summary>
    /// Inverts the result of another validator (NOT logic).
    /// </summary>
    public class NotValidator : IValidator
    {
        private readonly IValidator _innerValidator;
        private readonly string? _customMessage;

        public NotValidator(IValidator innerValidator, string? customMessage = null)
        {
            _innerValidator = innerValidator ?? throw new ArgumentNullException(nameof(innerValidator));
            _customMessage = customMessage;
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            var result = _innerValidator.Validate(value, propertyName);

            if (result.IsValid)
            {
                // Inner validator passed, but we want it to fail
                var message = _customMessage ?? $"{propertyName} must not pass the validation.";
                return ValidationResult.Fail(propertyName, message, value, "NOT_VALIDATOR");
            }

            // Inner validator failed, which is what we want
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Conditional validator that only runs if a condition is met.
    /// </summary>
    public class WhenValidator : IValidator
    {
        private readonly Func<object?, bool> _condition;
        private readonly IValidator _validator;

        public WhenValidator(Func<object?, bool> condition, IValidator validator)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            if (_condition(value))
            {
                return _validator.Validate(value, propertyName);
            }

            // Condition not met, skip validation
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Validator that runs unless a condition is met (opposite of When).
    /// </summary>
    public class UnlessValidator : IValidator
    {
        private readonly Func<object?, bool> _condition;
        private readonly IValidator _validator;

        public UnlessValidator(Func<object?, bool> condition, IValidator validator)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            if (!_condition(value))
            {
                return _validator.Validate(value, propertyName);
            }

            // Condition met, skip validation
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Validator that chains multiple validators and collects all errors.
    /// Unlike AND validator which stops at first failure, this collects all failures.
    /// </summary>
    public class ChainValidator : IValidator
    {
        private readonly List<IValidator> _validators = new();

        public ChainValidator(params IValidator[] validators)
        {
            _validators.AddRange(validators);
        }

        public void Add(IValidator validator)
        {
            _validators.Add(validator);
        }

        public ValidationResult Validate(object value, string propertyName)
        {
            var errors = new List<string>();
            var errorCodes = new List<string>();

            foreach (var validator in _validators)
            {
                var result = validator.Validate(value, propertyName);
                if (!result.IsValid)
                {
                    errors.Add(result.Message);
                    if (!string.IsNullOrEmpty(result.ErrorCode))
                        errorCodes.Add(result.ErrorCode);
                }
            }

            if (errors.Count == 0)
                return ValidationResult.Ok(propertyName);

            // Combine all errors
            var combinedMessage = string.Join("; ", errors);
            var combinedErrorCode = errorCodes.Count > 0 ? string.Join(",", errorCodes) : null;
            return ValidationResult.Fail(propertyName, combinedMessage, value, combinedErrorCode);
        }
    }

    /// <summary>
    /// Extension methods for creating composite validators.
    /// </summary>
    public static class CompositeValidatorExtensions
    {
        /// <summary>
        /// Combines this validator with another using AND logic.
        /// </summary>
        public static IValidator And(this IValidator first, IValidator second)
        {
            return new AndValidator(first, second);
        }

        /// <summary>
        /// Combines this validator with another using OR logic.
        /// </summary>
        public static IValidator Or(this IValidator first, IValidator second)
        {
            return new OrValidator(first, second);
        }

        /// <summary>
        /// Inverts the result of this validator.
        /// </summary>
        public static IValidator Not(this IValidator validator, string? customMessage = null)
        {
            return new NotValidator(validator, customMessage);
        }

        /// <summary>
        /// Makes this validator conditional.
        /// </summary>
        public static IValidator When(this IValidator validator, Func<object?, bool> condition)
        {
            return new WhenValidator(condition, validator);
        }

        /// <summary>
        /// Makes this validator run unless a condition is met.
        /// </summary>
        public static IValidator Unless(this IValidator validator, Func<object?, bool> condition)
        {
            return new UnlessValidator(condition, validator);
        }
    }
}
