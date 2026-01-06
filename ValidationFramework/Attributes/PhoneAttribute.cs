using ValidationFramework.Validator;

namespace ValidationFramework.Attributes;

public sealed class PhoneAttribute : ValidationAttribute
{
	public override IValidator CreateValidator() => new Core.PhoneValidator();
}
