using System.Text.RegularExpressions;
using ValidationFramework.Result;

namespace ValidationFramework.Fluent.Rules
{
    /// <summary>
    /// Base class for fluent validation rules.
    /// </summary>
    public abstract class FluentRuleBase<T, TProperty> : IFluentRule<T, TProperty>
    {
        public string? ErrorMessage { get; set; }
        public abstract string ErrorCode { get; }

        public abstract ValidationResult Validate(TProperty value, string propertyName);

        protected ValidationResult Fail(string propertyName, string defaultMessage, object? value)
        {
            return ValidationResult.Fail(propertyName, ErrorMessage ?? defaultMessage, value, ErrorCode);
        }

        protected ValidationResult Ok(string propertyName)
        {
            return ValidationResult.Ok(propertyName);
        }
    }

    /// <summary>
    /// Required validation rule.
    /// </summary>
    public sealed class RequiredRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        public override string ErrorCode => "REQUIRED";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value == null)
                return Fail(propertyName, $"{propertyName} is required.", value);

            if (value is string s && string.IsNullOrWhiteSpace(s))
                return Fail(propertyName, $"{propertyName} is required.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Email validation rule.
    /// </summary>
    public sealed class EmailRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public override string ErrorCode => "EMAIL";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return Fail(propertyName, $"{propertyName} must be a valid email.", value);

            if (!EmailRegex.IsMatch(s))
                return Fail(propertyName, $"{propertyName} is not a valid email format.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Length validation rule.
    /// </summary>
    public sealed class LengthRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly int _min;
        private readonly int _max;

        public LengthRule(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override string ErrorCode => "LENGTH";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s)
                return Fail(propertyName, $"{propertyName} must be a string.", value);

            if (s.Length < _min || s.Length > _max)
                return Fail(propertyName, $"{propertyName} length must be between {_min} and {_max}.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Minimum length validation rule.
    /// </summary>
    public sealed class MinLengthRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly int _min;

        public MinLengthRule(int min)
        {
            _min = min;
        }

        public override string ErrorCode => "MIN_LENGTH";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s)
                return Fail(propertyName, $"{propertyName} must be a string.", value);

            if (s.Length < _min)
                return Fail(propertyName, $"{propertyName} must be at least {_min} characters.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Maximum length validation rule.
    /// </summary>
    public sealed class MaxLengthRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly int _max;

        public MaxLengthRule(int max)
        {
            _max = max;
        }

        public override string ErrorCode => "MAX_LENGTH";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s)
                return Fail(propertyName, $"{propertyName} must be a string.", value);

            if (s.Length > _max)
                return Fail(propertyName, $"{propertyName} must be at most {_max} characters.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Regex pattern validation rule.
    /// </summary>
    public sealed class RegexRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly Regex _regex;
        private readonly string _pattern;

        public RegexRule(string pattern)
        {
            _pattern = pattern;
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public override string ErrorCode => "REGEX";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s)
                return Fail(propertyName, $"{propertyName} must be a string.", value);

            if (!_regex.IsMatch(s))
                return Fail(propertyName, $"{propertyName} does not match the required pattern.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Phone number validation rule.
    /// </summary>
    public sealed class PhoneRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{7,15}$", RegexOptions.Compiled);

        public override string ErrorCode => "PHONE";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return Fail(propertyName, $"{propertyName} must be a valid phone number.", value);

            if (!PhoneRegex.IsMatch(s))
                return Fail(propertyName, $"{propertyName} is not a valid phone number format.", value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Predicate-based validation rule.
    /// </summary>
    public sealed class PredicateRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly Func<TProperty, bool> _predicate;
        private readonly string _defaultMessage;
        private readonly string _errorCode;

        public PredicateRule(Func<TProperty, bool> predicate, string errorMessage, string? errorCode = null)
        {
            _predicate = predicate;
            _defaultMessage = errorMessage;
            _errorCode = errorCode ?? "PREDICATE";
        }

        public override string ErrorCode => _errorCode;

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            if (!_predicate(value))
                return Fail(propertyName, _defaultMessage, value);

            return Ok(propertyName);
        }
    }

    /// <summary>
    /// Custom delegate validation rule.
    /// </summary>
    public sealed class DelegateRule<T, TProperty> : FluentRuleBase<T, TProperty>
    {
        private readonly Func<TProperty, string, ValidationResult> _validator;

        public DelegateRule(Func<TProperty, string, ValidationResult> validator)
        {
            _validator = validator;
        }

        public override string ErrorCode => "CUSTOM";

        public override ValidationResult Validate(TProperty value, string propertyName)
        {
            return _validator(value, propertyName);
        }
    }
}
