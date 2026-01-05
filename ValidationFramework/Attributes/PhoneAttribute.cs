namespace ValidationFramework.Attributes;

public sealed class PhoneAttribute : ValidationAttribute
{
	public override Core.IValidator CreateValidator() => new Core.PhoneValidator();
}
