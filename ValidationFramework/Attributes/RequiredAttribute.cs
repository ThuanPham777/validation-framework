namespace ValidationFramework.Attributes;

public sealed class RequiredAttribute : ValidationAttribute
{
	public override Core.IValidator CreateValidator() => new Core.RequiredValidator();
}
