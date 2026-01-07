using ValidationFramework.Result;

namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Adapter that exposes a FluentValidator as a simple wrapper object.
    /// Kept for backward compatibility if external code expects an adapter.
    /// </summary>
    public sealed class FluentValidatorAdapter<T> where T : class
    {
        private readonly FluentValidator<T> _fluentValidator;

        public FluentValidatorAdapter(FluentValidator<T> fluentValidator)
        {
            _fluentValidator = fluentValidator ?? throw new ArgumentNullException(nameof(fluentValidator));
        }

        public List<ValidationResult> ValidateAll(T model) => _fluentValidator.Validate(model);

        public bool IsValid(T model) => _fluentValidator.IsValid(model);
    }
}
