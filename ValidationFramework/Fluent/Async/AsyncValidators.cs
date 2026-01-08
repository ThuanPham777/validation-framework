using ValidationFramework.Result;

namespace ValidationFramework.Fluent.Async
{
    /// <summary>
    /// Interface for asynchronous fluent validators.
    /// </summary>
    /// <typeparam name="T">The type of object to validate</typeparam>
    public interface IAsyncFluentValidator<T>
    {
        /// <summary>
        /// Validates the specified instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with list of validation results</returns>
        Task<List<ValidationResult>> ValidateAsync(T instance, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for asynchronous validators.
    /// </summary>
    public interface IAsyncValidator
    {
        /// <summary>
        /// Validates a value asynchronously.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with validation result</returns>
        Task<ValidationResult> ValidateAsync(object value, string propertyName, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Async validator implemented by a delegate.
    /// </summary>
    public class AsyncDelegateValidator : IAsyncValidator
    {
        private readonly Func<object?, string, CancellationToken, Task<ValidationResult>> _validateAsync;

        public AsyncDelegateValidator(Func<object?, string, CancellationToken, Task<ValidationResult>> validateAsync)
        {
            _validateAsync = validateAsync ?? throw new ArgumentNullException(nameof(validateAsync));
        }

        public AsyncDelegateValidator(Func<object?, string, Task<ValidationResult>> validateAsync)
        {
            if (validateAsync == null) throw new ArgumentNullException(nameof(validateAsync));
            _validateAsync = (value, propertyName, _) => validateAsync(value, propertyName);
        }

        public Task<ValidationResult> ValidateAsync(object value, string propertyName, CancellationToken cancellationToken = default)
        {
            return _validateAsync(value, propertyName, cancellationToken);
        }
    }

    /// <summary>
    /// Example async validators for common scenarios.
    /// </summary>
    public static class AsyncValidators
    {
        /// <summary>
        /// Creates an async validator that checks if a username is unique (simulated).
        /// In real scenarios, this would query a database.
        /// </summary>
        public static IAsyncValidator UniqueUsernameValidator()
        {
            return new AsyncDelegateValidator(async (value, propertyName, cancellationToken) =>
            {
                if (value is not string username || string.IsNullOrWhiteSpace(username))
                    return ValidationResult.Ok(propertyName);

                // Simulate database call
                await Task.Delay(100, cancellationToken);

                // Simulated check - in real app, query database
                var existingUsernames = new[] { "admin", "root", "system" };

                if (existingUsernames.Contains(username, StringComparer.OrdinalIgnoreCase))
                {
                    return ValidationResult.Fail(propertyName,
              $"Username '{username}' is already taken.",
         value, "USERNAME_TAKEN");
                }

                return ValidationResult.Ok(propertyName);
            });
        }

        /// <summary>
        /// Creates an async validator that checks if an email is unique (simulated).
        /// </summary>
        public static IAsyncValidator UniqueEmailValidator()
        {
            return new AsyncDelegateValidator(async (value, propertyName, cancellationToken) =>
              {
                  if (value is not string email || string.IsNullOrWhiteSpace(email))
                      return ValidationResult.Ok(propertyName);

                  // Simulate database call
                  await Task.Delay(100, cancellationToken);

                  // Simulated check
                  var existingEmails = new[] { "admin@example.com", "test@example.com" };

                  if (existingEmails.Contains(email, StringComparer.OrdinalIgnoreCase))
                  {
                      return ValidationResult.Fail(propertyName,
            $"Email '{email}' is already registered.",
           value, "EMAIL_TAKEN");
                  }

                  return ValidationResult.Ok(propertyName);
              });
        }

        /// <summary>
        /// Creates an async validator that verifies email via external API (simulated).
        /// </summary>
        public static IAsyncValidator EmailVerificationValidator()
        {
            return new AsyncDelegateValidator(async (value, propertyName, cancellationToken) =>
       {
           if (value is not string email || string.IsNullOrWhiteSpace(email))
               return ValidationResult.Ok(propertyName);

           // Simulate API call
           await Task.Delay(200, cancellationToken);

           // Simulated API response
           var disposableEmailDomains = new[] { "tempmail.com", "throwaway.email" };
           var domain = email.Split('@').LastOrDefault();

           if (domain != null && disposableEmailDomains.Contains(domain, StringComparer.OrdinalIgnoreCase))
           {
               return ValidationResult.Fail(propertyName,
                  "Disposable email addresses are not allowed.",
                    value, "DISPOSABLE_EMAIL");
           }

           return ValidationResult.Ok(propertyName);
       });
        }

        /// <summary>
        /// Creates an async validator that checks if a value exists in database (simulated).
        /// </summary>
        public static IAsyncValidator ExistsInDatabaseValidator<T>(Func<T, Task<bool>> existsCheck, string errorMessage)
        {
            return new AsyncDelegateValidator(async (value, propertyName, cancellationToken) =>
           {
               if (value is not T typedValue)
                   return ValidationResult.Ok(propertyName);

               var exists = await existsCheck(typedValue);

               if (!exists)
               {
                   return ValidationResult.Fail(propertyName, errorMessage, value, "NOT_EXISTS");
               }

               return ValidationResult.Ok(propertyName);
           });
        }

        /// <summary>
        /// Creates a custom async validator using a predicate.
        /// </summary>
        public static IAsyncValidator CustomAsync<T>(
            Func<T?, Task<bool>> predicateAsync,
    string errorMessage,
         string? errorCode = null)
        {
            return new AsyncDelegateValidator(async (value, propertyName, cancellationToken) =>
          {
              var typedValue = value is T t ? t : default;

              var isValid = await predicateAsync(typedValue);

              if (isValid)
                  return ValidationResult.Ok(propertyName);

              return ValidationResult.Fail(propertyName, errorMessage, value, errorCode);
          });
        }
    }
}
