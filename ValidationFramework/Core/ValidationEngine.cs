using System;
using System.Collections.Generic;
using System.Reflection;
using ValidationFramework.Attributes;
using ValidationFramework.Factory;
using ValidationFramework.Group;
using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Core
{
    public class ValidationEngine
    {
        private readonly ValidatorFactory _factory = new();
        private readonly Dictionary<string, ValidatorGroup> _validators = new();

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
            return results;
        }
    }
}