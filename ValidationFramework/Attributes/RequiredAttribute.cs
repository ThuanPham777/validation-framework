using ValidationFramework.Validator;

namespace ValidationFramework.Attributes;

public sealed class RequiredAttribute : ValidationAttribute
{
	public override IValidator CreateValidator() => new RequiredValidator();
}
