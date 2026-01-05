using ValidationFramework.Validator;

namespace ValidationFramework.Attributes;

public sealed class RegexAttribute : ValidationAttribute
{
    public string Pattern { get; }
    public RegexAttribute(string pattern)
    {
        Pattern = pattern;
    }

    public override IValidator CreateValidator() => new Core.RegexValidator(Pattern);
}
