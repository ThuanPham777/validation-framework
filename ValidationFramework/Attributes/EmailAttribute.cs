using ValidationFramework.Validator;

namespace ValidationFramework.Attributes;

public sealed class EmailAttribute : ValidationAttribute
{
	public override IValidator CreateValidator() => new Core.EmailValidator();
}
