using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Fluent.Rules
{
    /// <summary>
    /// Collection of reusable validation rules that can be used with fluent validators.
    /// </summary>
    public static class CommonRules
    {
        /// <summary>
        /// Creates a validator that checks if a string is not null or whitespace.
        /// </summary>
        public static IValidator NotEmptyRule()
        {
            return new DelegateValidator((value, propertyName) =>
    {
        if (value is string s && !string.IsNullOrWhiteSpace(s))
            return ValidationResult.Ok(propertyName);

        return ValidationResult.Fail(propertyName, $"{propertyName} must not be empty.", value, "NOT_EMPTY");
    });
        }

        /// <summary>
        /// Creates a validator that checks minimum string length.
        /// </summary>
        public static IValidator MinLengthRule(int minLength)
        {
            return new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.Length >= minLength)
                    return ValidationResult.Ok(propertyName);

                return ValidationResult.Fail(propertyName,
                       $"{propertyName} must be at least {minLength} characters.",
                   value, "MIN_LENGTH");
            });
        }

        /// <summary>
        /// Creates a validator that checks maximum string length.
        /// </summary>
        public static IValidator MaxLengthRule(int maxLength)
        {
            return new DelegateValidator((value, propertyName) =>
                    {
                        if (value is not string s || s.Length <= maxLength)
                            return ValidationResult.Ok(propertyName);

                        return ValidationResult.Fail(propertyName,
                $"{propertyName} must not exceed {maxLength} characters.",
             value, "MAX_LENGTH");
                    });
        }

        /// <summary>
        /// Creates a validator that checks if value is within a range.
        /// </summary>
        public static IValidator RangeRule<T>(T min, T max) where T : IComparable<T>
        {
            return new DelegateValidator((value, propertyName) =>
                     {
                         if (value is T comparableValue)
                         {
                             if (comparableValue.CompareTo(min) >= 0 && comparableValue.CompareTo(max) <= 0)
                                 return ValidationResult.Ok(propertyName);
                         }

                         return ValidationResult.Fail(propertyName,
             $"{propertyName} must be between {min} and {max}.",
                  value, "RANGE");
                     });
        }

        /// <summary>
        /// Creates a validator that checks if value equals a specific value.
        /// </summary>
        public static IValidator EqualRule<T>(T comparisonValue)
        {
            return new DelegateValidator((value, propertyName) =>
      {
          if (value?.Equals(comparisonValue) ?? comparisonValue == null)
              return ValidationResult.Ok(propertyName);

          return ValidationResult.Fail(propertyName,
       $"{propertyName} must be equal to {comparisonValue}.",
             value, "EQUAL");
      });
        }

        /// <summary>
        /// Creates a validator that checks if value does not equal a specific value.
        /// </summary>
        public static IValidator NotEqualRule<T>(T comparisonValue)
        {
            return new DelegateValidator((value, propertyName) =>
        {
            if (!value?.Equals(comparisonValue) ?? comparisonValue != null)
                return ValidationResult.Ok(propertyName);

            return ValidationResult.Fail(propertyName,
      $"{propertyName} must not be equal to {comparisonValue}.",
     value, "NOT_EQUAL");
        });
        }

        /// <summary>
        /// Creates a validator that checks if string contains only alphabetic characters.
        /// </summary>
        public static IValidator AlphaOnlyRule()
        {
            return new DelegateValidator((value, propertyName) =>
              {
                  if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z]+$"))
                      return ValidationResult.Ok(propertyName);

                  return ValidationResult.Fail(propertyName,
                   $"{propertyName} must contain only letters.",
                    value, "ALPHA_ONLY");
              });
        }

        /// <summary>
        /// Creates a validator that checks if string contains only alphanumeric characters.
        /// </summary>
        public static IValidator AlphaNumericRule()
        {
            return new DelegateValidator((value, propertyName) =>
     {
         if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"^[a-zA-Z0-9]+$"))
             return ValidationResult.Ok(propertyName);

         return ValidationResult.Fail(propertyName,
       $"{propertyName} must contain only letters and digits.",
      value, "ALPHA_NUMERIC");
     });
        }

        /// <summary>
        /// Creates a validator that checks if string does not contain digits.
        /// </summary>
        public static IValidator NoDigitsRule()
        {
            return new DelegateValidator((value, propertyName) =>
              {
                  if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"\d"))
                      return ValidationResult.Fail(propertyName,
               $"{propertyName} must not contain digits.",
                   value, "NO_DIGITS");

                  return ValidationResult.Ok(propertyName);
              });
        }

        /// <summary>
        /// Creates a validator that checks if string does not contain special characters.
        /// </summary>
        public static IValidator NoSpecialCharsRule()
        {
            return new DelegateValidator((value, propertyName) =>
                    {
                        if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"[^a-zA-Z0-9]"))
                            return ValidationResult.Fail(propertyName,
                       $"{propertyName} must not contain special characters.",
                               value, "NO_SPECIAL_CHARS");

                        return ValidationResult.Ok(propertyName);
                    });
        }

        /// <summary>
        /// Creates a validator that checks if string starts with a specific prefix.
        /// </summary>
        public static IValidator StartsWithRule(string prefix, StringComparison comparison = StringComparison.Ordinal)
        {
            return new DelegateValidator((value, propertyName) =>
                 {
                     if (value is string s && s.StartsWith(prefix, comparison))
                         return ValidationResult.Ok(propertyName);

                     return ValidationResult.Fail(propertyName,
             $"{propertyName} must start with '{prefix}'.",
        value, "STARTS_WITH");
                 });
        }

        /// <summary>
        /// Creates a validator that checks if string ends with a specific suffix.
        /// </summary>
        public static IValidator EndsWithRule(string suffix, StringComparison comparison = StringComparison.Ordinal)
        {
            return new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.EndsWith(suffix, comparison))
                    return ValidationResult.Ok(propertyName);

                return ValidationResult.Fail(propertyName,
                $"{propertyName} must end with '{suffix}'.",
                  value, "ENDS_WITH");
            });
        }

        /// <summary>
        /// Creates a validator that checks if string contains a specific substring.
        /// </summary>
        public static IValidator ContainsRule(string substring, StringComparison comparison = StringComparison.Ordinal)
        {
            return new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.Contains(substring, comparison))
                    return ValidationResult.Ok(propertyName);

                return ValidationResult.Fail(propertyName,
              $"{propertyName} must contain '{substring}'.",
             value, "CONTAINS");
            });
        }

        /// <summary>
        /// Creates a validator that checks if email is from allowed domains.
        /// </summary>
        public static IValidator EmailDomainRule(params string[] allowedDomains)
        {
            return new DelegateValidator((value, propertyName) =>
                  {
                      if (value is not string email || string.IsNullOrWhiteSpace(email))
                          return ValidationResult.Ok(propertyName);

                      var isValid = allowedDomains.Any(domain =>
                email.EndsWith($"@{domain}", StringComparison.OrdinalIgnoreCase));

                      if (isValid)
                          return ValidationResult.Ok(propertyName);

                      var domainList = string.Join(", ", allowedDomains.Select(d => "@" + d));
                      return ValidationResult.Fail(propertyName,
                          $"{propertyName} must be from domains: {domainList}",
               value, "EMAIL_DOMAIN");
                  });
        }

        /// <summary>
        /// Creates a validator that checks if value is null.
        /// </summary>
        public static IValidator NullRule()
        {
            return new DelegateValidator((value, propertyName) =>
        {
            if (value == null)
                return ValidationResult.Ok(propertyName);

            return ValidationResult.Fail(propertyName,
$"{propertyName} must be null.",
 value, "NULL");
        });
        }

        /// <summary>
        /// Creates a validator that checks if value is not null.
        /// </summary>
        public static IValidator NotNullRule()
        {
            return new DelegateValidator((value, propertyName) =>
             {
                 if (value != null)
                     return ValidationResult.Ok(propertyName);

                 return ValidationResult.Fail(propertyName,
        $"{propertyName} must not be null.",
            value, "NOT_NULL");
             });
        }

        /// <summary>
        /// Creates a validator that checks if a string is a valid URL.
        /// </summary>
        public static IValidator UrlRule()
        {
            return new DelegateValidator((value, propertyName) =>
     {
         if (value is not string url || string.IsNullOrWhiteSpace(url))
             return ValidationResult.Ok(propertyName);

         if (Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
             return ValidationResult.Ok(propertyName);

         return ValidationResult.Fail(propertyName,
   $"{propertyName} must be a valid URL.",
         value, "URL");
     });
        }

        /// <summary>
        /// Creates a validator that checks if a string is a valid credit card number using Luhn algorithm.
        /// </summary>
        public static IValidator CreditCardRule()
        {
            return new DelegateValidator((value, propertyName) =>
{
    if (value is not string cardNumber || string.IsNullOrWhiteSpace(cardNumber))
        return ValidationResult.Ok(propertyName);

    var cleaned = cardNumber.Replace(" ", "").Replace("-", "");

    if (!System.Text.RegularExpressions.Regex.IsMatch(cleaned, @"^\d{13,19}$"))
        return ValidationResult.Fail(propertyName,
                 $"{propertyName} must be a valid credit card number.",
                    value, "CREDIT_CARD");

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

    if (sum % 10 == 0)
        return ValidationResult.Ok(propertyName);

    return ValidationResult.Fail(propertyName,
             $"{propertyName} must be a valid credit card number.",
          value, "CREDIT_CARD");
});
        }

        /// <summary>
        /// Creates a validator using a custom predicate.
        /// </summary>
        public static IValidator PredicateRule<T>(Func<T?, bool> predicate, string errorMessage, string? errorCode = null)
        {
            return new DelegateValidator((value, propertyName) =>
                {
                    var typedValue = value is T t ? t : default;

                    if (predicate(typedValue))
                        return ValidationResult.Ok(propertyName);

                    return ValidationResult.Fail(propertyName, errorMessage, value, errorCode);
                });
        }
    }
}
