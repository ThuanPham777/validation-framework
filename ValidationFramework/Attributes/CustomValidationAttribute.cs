namespace ValidationFramework.Attributes;

public sealed class CustomValidationAttribute : ValidationAttribute
{
    public Type ValidatorType { get; }

    public CustomValidationAttribute(Type validatorType)
    {
        ValidatorType = validatorType ?? throw new ArgumentNullException(nameof(validatorType));
    }

    public override Core.IValidator CreateValidator()
    {
        if (!typeof(Core.IValidator).IsAssignableFrom(ValidatorType))
            throw new InvalidOperationException($"{ValidatorType.Name} must implement IValidator");
        return (Core.IValidator)Activator.CreateInstance(ValidatorType)!;
    }
}
