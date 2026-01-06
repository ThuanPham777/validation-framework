using System.Collections.Generic;
using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Group
{
    public class ValidatorGroup : IValidator
    {
        private readonly List<IValidator> _validators = new();

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
                    return result;
            }
            return ValidationResult.Ok(propertyName);
        }
    }
}