namespace ValidationFramework.Attributes;

public sealed class RegexAttribute : ValidationAttribute
{
    public string Pattern { get; }
    public RegexAttribute(string pattern)
    {
        Pattern = pattern;
    }
}
