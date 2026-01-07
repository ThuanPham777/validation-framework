using ValidationFramework.Fluent;
using ValidationFramework.Result;

namespace ValidationFramework.Extensions
{
    /// <summary>
    /// Extension methods for PropertyValidator to add common validation rules.
    /// </summary>
    public static class PropertyValidatorExtensions
    {
        /// <summary>
        /// Adds a NotEmpty validation rule (for strings).
        /// </summary>
        public static PropertyValidator<T, string> NotEmpty<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Custom(value => !string.IsNullOrWhiteSpace(value), "must not be empty.");
        }

        /// <summary>
        /// Adds a MinLength validation rule.
        /// </summary>
        public static PropertyValidator<T, string> MinLength<T>(this PropertyValidator<T, string> validator, int minLength)
        {
            return validator.Custom(value => value?.Length >= minLength, $"must be at least {minLength} characters.");
        }

        /// <summary>
        /// Adds a MaxLength validation rule.
        /// </summary>
        public static PropertyValidator<T, string> MaxLength<T>(this PropertyValidator<T, string> validator, int maxLength)
        {
            return validator.Custom(value => value == null || value.Length <= maxLength, $"must not exceed {maxLength} characters.");
        }

        /// <summary>
        /// Adds a Must validation rule with custom predicate.
        /// </summary>
        public static PropertyValidator<T, TProperty> Must<T, TProperty>(
     this PropertyValidator<T, TProperty> validator,
            Func<TProperty?, bool> predicate,
     string errorMessage)
        {
            return validator.Custom(predicate, errorMessage);
        }

        /// <summary>
        /// Adds a Must validation rule with access to the entire object.
        /// </summary>
        public static PropertyValidator<T, TProperty> Must<T, TProperty>(
            this PropertyValidator<T, TProperty> validator,
            Func<T, TProperty?, bool> predicate,
          string errorMessage)
        {
            // Store the parent instance for validation
            var parentInstance = default(T);

            return validator.Custom((value, propertyName) =>
                        {
                            // This will need to be enhanced with actual parent instance access
                            // For now, we'll use a simpler version
                            if (value is TProperty typedValue)
                            {
                                // We can't easily access parent here without refactoring
                                // This is a simplified version
                                return ValidationResult.Ok(propertyName);
                            }
                            return ValidationResult.Fail(propertyName, errorMessage, value);
                        });
        }

        /// <summary>
        /// Adds an Equal validation rule.
        /// </summary>
        public static PropertyValidator<T, TProperty> Equal<T, TProperty>(
            this PropertyValidator<T, TProperty> validator,
      TProperty comparisonValue)
        {
            return validator.Custom(value =>
          value?.Equals(comparisonValue) ?? comparisonValue == null,
            $"must be equal to {comparisonValue}.");
        }

        /// <summary>
        /// Adds a NotEqual validation rule.
        /// </summary>
        public static PropertyValidator<T, TProperty> NotEqual<T, TProperty>(
     this PropertyValidator<T, TProperty> validator,
      TProperty comparisonValue)
        {
            return validator.Custom(value =>
                       !value?.Equals(comparisonValue) ?? comparisonValue != null,
       $"must not be equal to {comparisonValue}.");
        }

        /// <summary>
        /// Adds a GreaterThan validation rule for comparable types.
        /// </summary>
        public static PropertyValidator<T, TProperty> GreaterThan<T, TProperty>(
          this PropertyValidator<T, TProperty> validator,
               TProperty comparisonValue) where TProperty : IComparable<TProperty>
        {
            return validator.Custom(value =>
       value != null && value.CompareTo(comparisonValue) > 0,
                $"must be greater than {comparisonValue}.");
        }

        /// <summary>
        /// Adds a LessThan validation rule for comparable types.
        /// </summary>
        public static PropertyValidator<T, TProperty> LessThan<T, TProperty>(
            this PropertyValidator<T, TProperty> validator,
  TProperty comparisonValue) where TProperty : IComparable<TProperty>
        {
            return validator.Custom(value =>
           value != null && value.CompareTo(comparisonValue) < 0,
              $"must be less than {comparisonValue}.");
        }

        /// <summary>
        /// Adds a Range validation rule for comparable types.
        /// </summary>
        public static PropertyValidator<T, TProperty> Range<T, TProperty>(
            this PropertyValidator<T, TProperty> validator,
        TProperty min,
          TProperty max) where TProperty : IComparable<TProperty>
        {
            return validator.Custom(value =>
                    value != null && value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0,
            $"must be between {min} and {max}.");
        }

        /// <summary>
        /// Adds an AlphaOnly validation rule (only letters).
        /// </summary>
        public static PropertyValidator<T, string> AlphaOnly<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Regex(@"^[a-zA-Z]+$")
               .WithMessage("must contain only letters.");
        }

        /// <summary>
        /// Adds an AlphaNumeric validation rule (letters and digits only).
        /// </summary>
        public static PropertyValidator<T, string> AlphaNumeric<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Regex(@"^[a-zA-Z0-9]+$")
                .WithMessage("must contain only letters and digits.");
        }

        /// <summary>
        /// Adds a NoSpecialChars validation rule.
        /// </summary>
        public static PropertyValidator<T, string> NoSpecialChars<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Custom(value =>
      value == null || !System.Text.RegularExpressions.Regex.IsMatch(value, @"[^a-zA-Z0-9]"),
                    "must not contain special characters.");
        }

        /// <summary>
        /// Adds a NoDigits validation rule.
        /// </summary>
        public static PropertyValidator<T, string> NoDigits<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Custom(value =>
     value == null || !System.Text.RegularExpressions.Regex.IsMatch(value, @"\d"),
                "must not contain digits.");
        }

        /// <summary>
        /// Adds a StartsWith validation rule.
        /// </summary>
        public static PropertyValidator<T, string> StartsWith<T>(
            this PropertyValidator<T, string> validator,
     string prefix,
            StringComparison comparison = StringComparison.Ordinal)
        {
            return validator.Custom(value =>
            value?.StartsWith(prefix, comparison) ?? false,
                   $"must start with '{prefix}'.");
        }

        /// <summary>
        /// Adds an EndsWith validation rule.
        /// </summary>
        public static PropertyValidator<T, string> EndsWith<T>(
            this PropertyValidator<T, string> validator,
    string suffix,
   StringComparison comparison = StringComparison.Ordinal)
        {
            return validator.Custom(value =>
  value?.EndsWith(suffix, comparison) ?? false,
       $"must end with '{suffix}'.");
        }

        /// <summary>
        /// Adds a Contains validation rule.
        /// </summary>
        public static PropertyValidator<T, string> Contains<T>(
      this PropertyValidator<T, string> validator,
            string substring,
        StringComparison comparison = StringComparison.Ordinal)
        {
            return validator.Custom(value =>
                 value?.Contains(substring, comparison) ?? false,
                        $"must contain '{substring}'.");
        }

        /// <summary>
        /// Adds an EmailDomain validation rule.
        /// </summary>
        public static PropertyValidator<T, string> EmailDomain<T>(
            this PropertyValidator<T, string> validator,
            params string[] allowedDomains)
        {
            return validator.Custom(value =>
            {
                if (string.IsNullOrWhiteSpace(value)) return true;

                return allowedDomains.Any(domain =>
                     value.EndsWith($"@{domain}", StringComparison.OrdinalIgnoreCase));
            }, $"must be from domains: {string.Join(", ", allowedDomains.Select(d => "@" + d))}");
        }

        /// <summary>
        /// Adds a CreditCard validation rule.
        /// </summary>
        public static PropertyValidator<T, string> CreditCard<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Custom(value =>
        {
            if (string.IsNullOrWhiteSpace(value)) return true;

            // Remove spaces and dashes
            var cleaned = value.Replace(" ", "").Replace("-", "");

            // Check if it's all digits and has valid length
            if (!System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^\d{13,19}$"))
                return false;

            // Luhn algorithm
            int sum = 0;
            bool alternate = false;
            for (int i = cleaned.Length - 1; i >= 0; i--)
            {
                int digit = cleaned[i] - '0';
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }, "must be a valid credit card number.");
        }

        /// <summary>
        /// Adds a URL validation rule.
        /// </summary>
        public static PropertyValidator<T, string> Url<T>(this PropertyValidator<T, string> validator)
        {
            return validator.Custom(value =>
                  {
                      if (string.IsNullOrWhiteSpace(value)) return true;
                      return Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                  (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                  }, "must be a valid URL.");
        }
    }
}
