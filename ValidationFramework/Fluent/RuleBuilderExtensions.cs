
namespace ValidationFramework.Fluent
{
    /// <summary>
    /// Extension methods for fluent validation rules.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Adds a rule that the value must not be empty (for strings).
        /// </summary>
        public static IRuleBuilder<T, string> NotEmpty<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.Must(
                     value => !string.IsNullOrWhiteSpace(value),
                      "must not be empty.",
                   "NOT_EMPTY"
                        );
        }

        /// <summary>
        /// Adds a rule that the value must be equal to another value.
        /// </summary>
        public static IRuleBuilder<T, TProperty> Equal<T, TProperty>(
         this IRuleBuilder<T, TProperty> builder,
            TProperty compareValue)
        {
            return builder.Must(
          value => EqualityComparer<TProperty>.Default.Equals(value, compareValue),
                $"must be equal to '{compareValue}'.",
            "EQUAL"
          );
        }

        /// <summary>
        /// Adds a rule that the value must not be equal to another value.
        /// </summary>
        public static IRuleBuilder<T, TProperty> NotEqual<T, TProperty>(
  this IRuleBuilder<T, TProperty> builder,
      TProperty compareValue)
        {
            return builder.Must(
                value => !EqualityComparer<TProperty>.Default.Equals(value, compareValue),
         $"must not be equal to '{compareValue}'.",
                       "NOT_EQUAL"
             );
        }

        /// <summary>
        /// Adds a rule that the numeric value must be greater than a specified value.
        /// </summary>
        public static IRuleBuilder<T, int> GreaterThan<T>(this IRuleBuilder<T, int> builder, int value)
        {
            return builder.Must(
        v => v > value,
        $"must be greater than {value}.",
               "GREATER_THAN"
         );
        }

        /// <summary>
        /// Adds a rule that the numeric value must be less than a specified value.
        /// </summary>
        public static IRuleBuilder<T, int> LessThan<T>(this IRuleBuilder<T, int> builder, int value)
        {
            return builder.Must(
                v => v < value,
            $"must be less than {value}.",
                           "LESS_THAN"
                   );
        }

        /// <summary>
        /// Adds a rule that the numeric value must be between two values (inclusive).
        /// </summary>
        public static IRuleBuilder<T, int> InclusiveBetween<T>(this IRuleBuilder<T, int> builder, int min, int max)
        {
            return builder.Must(
 v => v >= min && v <= max,
 $"must be between {min} and {max}.",
 "BETWEEN"
            );
        }

        /// <summary>
        /// Adds a rule that the string value must contain a substring.
        /// </summary>
        public static IRuleBuilder<T, string> Contains<T>(this IRuleBuilder<T, string> builder, string substring)
        {
            return builder.Must(
                value => value?.Contains(substring, StringComparison.OrdinalIgnoreCase) == true,
              $"must contain '{substring}'.",
             "CONTAINS"
       );
        }

        /// <summary>
        /// Adds a rule that the string value must start with a prefix.
        /// </summary>
        public static IRuleBuilder<T, string> StartsWith<T>(this IRuleBuilder<T, string> builder, string prefix)
        {
            return builder.Must(
          value => value?.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) == true,
                   $"must start with '{prefix}'.",
              "STARTS_WITH"
            );
        }

        /// <summary>
        /// Adds a rule that the string value must end with a suffix.
        /// </summary>
        public static IRuleBuilder<T, string> EndsWith<T>(this IRuleBuilder<T, string> builder, string suffix)
        {
            return builder.Must(
              value => value?.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) == true,
            $"must end with '{suffix}'.",
               "ENDS_WITH"
                  );
        }

        /// <summary>
        /// Adds a credit card number validation rule.
        /// </summary>
        public static IRuleBuilder<T, string> CreditCard<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.Must(
       value => IsValidCreditCard(value),
                "must be a valid credit card number.",
           "CREDIT_CARD"
            );
        }

        private static bool IsValidCreditCard(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Remove spaces and dashes
            var digits = value.Replace(" ", "").Replace("-", "");

            if (!digits.All(char.IsDigit) || digits.Length < 13 || digits.Length > 19)
                return false;

            // Luhn algorithm
            int sum = 0;
            bool alternate = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(digits[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }
    }
}
