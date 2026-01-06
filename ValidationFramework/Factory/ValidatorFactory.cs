using ValidationFramework.Attributes;
using ValidationFramework.Validator;

namespace ValidationFramework.Factory
{
    public class ValidatorFactory
    {
        public IValidator Create(ValidationAttribute attr)
        {
            return attr.CreateValidator();
        }
    }
}