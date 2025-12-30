namespace ValidationFramework.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public abstract class ValidationAttribute : Attribute
{
    public string ErrorMessage { get; init; } = "Invalid";
}
