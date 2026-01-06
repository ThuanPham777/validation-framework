using ValidationFramework.Validator;

namespace ValidationFramework.Attributes;

public sealed class LengthAttribute : ValidationAttribute
{
    public int Min { get; init; }
    public int Max { get; init; }

    public LengthAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public override IValidator CreateValidator() => new Core.LengthValidator(Min, Max);
}
