namespace ValidationFramework.Attributes;

public sealed class EmailAttribute : ValidationAttribute
{
	public override Core.IValidator CreateValidator() => new Core.EmailValidator();
}
