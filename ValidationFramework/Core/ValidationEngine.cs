using ValidationFramework.Fluent;
using ValidationFramework.Result;
using ValidationFramework.Notification;

namespace ValidationFramework.Core
{
    /// <summary>
    /// Core validation engine that only supports Fluent validators.
    /// </summary>
    public class ValidationEngine
    {
        private readonly List<object> _fluentValidators = new();
        private readonly NotificationPublisher _publisher = new();

        /// <summary>
        /// Gets the notification publisher for subscribing to validation events.
        /// </summary>
        public NotificationPublisher Publisher => _publisher;

        /// <summary>
        /// Removes all registered fluent validators.
        /// </summary>
        public void ClearFluentValidators()
        {
            _fluentValidators.Clear();
        }

        /// <summary>
        /// Registers a FluentValidator to be used during validation.
        /// </summary>
        public void AddFluentValidator<T>(FluentValidator<T> fluentValidator) where T : class
        {
            _fluentValidators.Add(fluentValidator ?? throw new ArgumentNullException(nameof(fluentValidator)));
        }

        /// <summary>
        /// Registers an AbstractFluentValidator to be used during validation.
        /// </summary>
        public void AddFluentValidator<T>(AbstractFluentValidator<T> fluentValidator) where T : class
        {
            _fluentValidators.Add(fluentValidator ?? throw new ArgumentNullException(nameof(fluentValidator)));
        }

        /// <summary>
        /// Registers a FluentValidator using a builder action.
        /// </summary>
        public void AddFluentValidator<T>(Action<ValidatorBuilder<T>> configure) where T : class
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            var builder = new ValidatorBuilder<T>();
            configure(builder);
            _fluentValidators.Add(builder.Build());
        }

        /// <summary>
        /// Validates the model using registered fluent validators.
        /// </summary>
        public List<ValidationResult> Validate(object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var results = new List<ValidationResult>();

            foreach (var fluentValidator in _fluentValidators)
            {
                var fluentResults = InvokeFluentValidate(fluentValidator, model);
                if (fluentResults != null && fluentResults.Count > 0)
                    results.AddRange(fluentResults);
            }

            // Auto-notify after validation
            if (results.Count > 0)
                _publisher.Notify(ValidationEventType.Invalid, results);
            else
                _publisher.Notify(ValidationEventType.Validated, results);

            return results;
        }

        private List<ValidationResult> InvokeFluentValidate(object fluentValidator, object model)
        {
            var validatorType = fluentValidator.GetType();

            // Handle AbstractFluentValidator<T>
            if (IsDerivedFromGeneric(validatorType, typeof(AbstractFluentValidator<>)))
            {
                var modelType = validatorType.BaseType!.GetGenericArguments().FirstOrDefault();
                if (modelType != null && modelType.IsInstanceOfType(model))
                {
                    var validateMethod = validatorType.GetMethod("Validate", new[] { modelType });
                    if (validateMethod != null)
                    {
                        var result = validateMethod.Invoke(fluentValidator, new[] { model }) as List<ValidationResult>;
                        return result ?? new List<ValidationResult>();
                    }
                }

                return new List<ValidationResult>();
            }

            // Handle FluentValidator<T>
            if (validatorType.IsGenericType && validatorType.GetGenericTypeDefinition() == typeof(FluentValidator<>))
            {
                var modelType = validatorType.GetGenericArguments().FirstOrDefault();
                if (modelType != null && modelType.IsInstanceOfType(model))
                {
                    var validateMethod = validatorType.GetMethod("Validate", new[] { modelType });
                    if (validateMethod != null)
                    {
                        var result = validateMethod.Invoke(fluentValidator, new[] { model }) as List<ValidationResult>;
                        return result ?? new List<ValidationResult>();
                    }
                }

                return new List<ValidationResult>();
            }

            return new List<ValidationResult>();
        }

        private static bool IsDerivedFromGeneric(Type type, Type genericBaseType)
        {
            var current = type.BaseType;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == genericBaseType)
                    return true;
                current = current.BaseType;
            }
            return false;
        }
    }
}