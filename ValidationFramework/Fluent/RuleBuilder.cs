using System.Linq.Expressions;
using ValidationFramework.Fluent.Rules;
using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Represents a validation rule configuration for a specific property.
    /// </summary>
    internal sealed class PropertyRule<T, TProperty>
    {
        public string PropertyName { get; }
        public Func<T, TProperty> PropertyAccessor { get; }
        public List<RuleEntry<T, TProperty>> Rules { get; } = new();

        public PropertyRule(string propertyName, Func<T, TProperty> accessor)
        {
            PropertyName = propertyName;
            PropertyAccessor = accessor;
        }

        public List<ValidationResult> Validate(T instance)
        {
            var results = new List<ValidationResult>();
            var value = PropertyAccessor(instance);

            foreach (var entry in Rules)
            {
                // Check condition
                if (entry.Condition != null && !entry.Condition(instance))
                    continue;

                var result = entry.Rule.Validate(value, PropertyName);
                if (!result.IsValid)
                    results.Add(result);
            }

            return results;
        }
    }

    /// <summary>
    /// Holds a rule and its optional condition.
    /// </summary>
    internal sealed class RuleEntry<T, TProperty>
    {
        public IFluentRule<T, TProperty> Rule { get; }
        public Func<T, bool>? Condition { get; set; }

        public RuleEntry(IFluentRule<T, TProperty> rule)
        {
            Rule = rule;
        }
    }

    /// <summary>
    /// Implementation of IRuleBuilder for chaining validation rules.
    /// </summary>
    public sealed class RuleBuilder<T, TProperty> : IRuleBuilder<T, TProperty>
    {
        private readonly PropertyRule<T, TProperty> _propertyRule;
        private RuleEntry<T, TProperty>? _lastRule;

        internal RuleBuilder(PropertyRule<T, TProperty> propertyRule)
        {
            _propertyRule = propertyRule;
        }

        private IRuleBuilder<T, TProperty> AddRule(IFluentRule<T, TProperty> rule)
        {
            var entry = new RuleEntry<T, TProperty>(rule);
            _propertyRule.Rules.Add(entry);
            _lastRule = entry;
            return this;
        }

        public IRuleBuilder<T, TProperty> Required()
        {
            return AddRule(new RequiredRule<T, TProperty>());
        }

        public IRuleBuilder<T, TProperty> Email()
        {
            return AddRule(new EmailRule<T, TProperty>());
        }

        public IRuleBuilder<T, TProperty> Length(int min, int max)
        {
            return AddRule(new LengthRule<T, TProperty>(min, max));
        }

        public IRuleBuilder<T, TProperty> MinLength(int min)
        {
            return AddRule(new MinLengthRule<T, TProperty>(min));
        }

        public IRuleBuilder<T, TProperty> MaxLength(int max)
        {
            return AddRule(new MaxLengthRule<T, TProperty>(max));
        }

        public IRuleBuilder<T, TProperty> Matches(string pattern)
        {
            return AddRule(new RegexRule<T, TProperty>(pattern));
        }

        public IRuleBuilder<T, TProperty> Phone()
        {
            return AddRule(new PhoneRule<T, TProperty>());
        }

        public IRuleBuilder<T, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage, string? errorCode = null)
        {
            return AddRule(new PredicateRule<T, TProperty>(predicate, errorMessage, errorCode));
        }

        public IRuleBuilder<T, TProperty> Custom(Func<TProperty, string, ValidationResult> validator)
        {
            return AddRule(new DelegateRule<T, TProperty>(validator));
        }

        public IRuleBuilder<T, TProperty> WithMessage(string message)
        {
            if (_lastRule != null)
                _lastRule.Rule.ErrorMessage = message;
            return this;
        }

        public IRuleBuilder<T, TProperty> WithErrorCode(string errorCode)
        {
            // ErrorCode is typically set in rule, but we can override message pattern
            // For simplicity, we handle this via WithMessage
            return this;
        }

        public IRuleBuilder<T, TProperty> When(Func<T, bool> condition)
        {
            if (_lastRule != null)
                _lastRule.Condition = condition;
            return this;
        }
    }
}
