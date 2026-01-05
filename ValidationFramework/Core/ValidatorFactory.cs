using ValidationFramework.Attributes;
using ValidationFramework.Core;

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