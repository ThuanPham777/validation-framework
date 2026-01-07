using System.Reflection;
using ValidationFramework.Attributes;
using ValidationFramework.Factory;
using ValidationFramework.Group;
using ValidationFramework.Result;
using ValidationFramework.Validator;
using ValidationFramework.Notification;

namespace ValidationFramework.Core
{
    public class ValidationEngine
    {
        private readonly ValidatorFactory _factory = new();
        private readonly Dictionary<string, ValidatorGroup> _validators = new();
        private readonly NotificationPublisher _publisher = new();

        public NotificationPublisher Publisher => _publisher;

        public void AddValidator(string property, IValidator validator)
        {
            if (!_validators.ContainsKey(property))
                _validators[property] = new ValidatorGroup();
            _validators[property].Add(validator);
        }

        public List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var type = model.GetType();
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(model);
                var attrs = prop.GetCustomAttributes<ValidationAttribute>(true);
                foreach (var attr in attrs)
                {
                    var validator = _factory.Create(attr);
                    var result = validator.Validate(value, prop.Name);
                    if (!result.IsValid)
                        results.Add(result);
                }
                if (_validators.TryGetValue(prop.Name, out var group))
                {
                    var result = group.Validate(value, prop.Name);
                    if (!result.IsValid)
                        results.Add(result);
                }
            }

            // Auto-notify after validation
            if (results.Count > 0)
                _publisher.Notify(ValidationEventType.Invalid, results);
            else
                _publisher.Notify(ValidationEventType.Validated, results);

            return results;
        }
    }
}