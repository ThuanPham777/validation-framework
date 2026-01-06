namespace ValidationFramework.Attributes;

public sealed class CustomValidationAttribute : ValidationAttribute
{
    public Type ValidatorType { get; }

    public CustomValidationAttribute(Type validatorType)
    {
        ValidatorType = validatorType ?? throw new ArgumentNullException(nameof(validatorType));
    }

    public override Validator.IValidator CreateValidator()
    {
        if (!typeof(Validator.IValidator).IsAssignableFrom(ValidatorType))
            throw new InvalidOperationException($"{ValidatorType.Name} must implement IValidator");
        return (Validator.IValidator)Activator.CreateInstance(ValidatorType)!;
    }
}
