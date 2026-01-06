using System;
using ValidationFramework.Result;

namespace ValidationFramework.Validator
{
 /// <summary>
 /// Validator implemented by a delegate. Useful for defining rules purely by code.
 /// </summary>
 public sealed class DelegateValidator : IValidator
 {
 private readonly Func<object?, string, ValidationResult> _validate;

 public DelegateValidator(Func<object?, string, ValidationResult> validate)
 {
 _validate = validate ?? throw new ArgumentNullException(nameof(validate));
 }

 public ValidationResult Validate(object value, string propertyName)
 {
 return _validate(value, propertyName);
 }
 }
}
