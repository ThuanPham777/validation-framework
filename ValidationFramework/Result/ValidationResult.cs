namespace ValidationFramework.Core;

public sealed class ValidationResult
{
    public bool IsValid { get; private set; }
    public string Message { get; private set; }
    public string PropertyName { get; private set; }
    public object? AttemptedValue { get; private set; }
    public string? ErrorCode { get; private set; }

    private ValidationResult(bool isValid, string propertyName, string message, object? attemptedValue, string? errorCode)
    {
        IsValid = isValid;
        PropertyName = propertyName;
        Message = message;
        AttemptedValue = attemptedValue;
        ErrorCode = errorCode;
    }

    public static ValidationResult Ok(string propertyName) =>
        new(true, propertyName, string.Empty, null, null);

    public static ValidationResult Fail(string propertyName, string message, object? value = null, string? errorCode = null) =>
        new(false, propertyName, message, value, errorCode);

    public override string ToString() => IsValid
        ? $"{PropertyName}: OK"
        : $"{PropertyName}: {Message}";
}
