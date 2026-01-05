using ValidationFramework.Attributes;
using ValidationFramework.Validator;

namespace ValidationFramework.Core
{
    public class ValidatorFactory
    {
        public IValidator Create(ValidationAttribute attr)
        {
            return attr.CreateValidator();
        }
    }
}