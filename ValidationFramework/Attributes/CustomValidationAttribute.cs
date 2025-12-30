namespace ValidationFramework.Attributes;

public sealed class CustomValidationAttribute : ValidationAttribute
{
    public Type ValidatorType { get; }

    public CustomValidationAttribute(Type validatorType)
    {
        ValidatorType = validatorType ?? throw new ArgumentNullException(nameof(validatorType));
    }
}
