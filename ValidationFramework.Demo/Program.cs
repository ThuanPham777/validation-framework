using System;
using ValidationFramework.Attributes;
using ValidationFramework.Core;
using ValidationFramework.Result;
using ValidationFramework.Group;
using ValidationFramework.Validator;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo
{
    // Model with attribute validation
    public class User
    {
        [Required(ErrorMessage = "Username is required")]
        [Length(3, 10, ErrorMessage = "Username must be 3-10 chars")]
        public string Username { get; set; }

        [Email(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
    }

    // Username does not contain digits
    public class NoDigitValidator : IValidator
    {
        public ValidationResult Validate(object value, string propertyName)
        {
            if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, "\\d"))
                return ValidationResult.Fail(propertyName, $"{propertyName} must not contain digits.");
            return ValidationResult.Ok(propertyName);
        }
    }

    // Custom notifier in console with red color
    public class RedConsoleNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    var old = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[RedNotifier] {result.PropertyName}: {result.Message}");
                    Console.ForegroundColor = old;
                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var user = new User { Username = "ab1", Email = "not-an-email" };
            var engine = new ValidationEngine();

            //4. Add custom validator and group validator for Username
            var group = new ValidatorGroup();
            group.Add(new NoDigitValidator());
            group.Add(new AlphaOnlyValidator());
            group.Add(new NoSpecialCharValidator());
            group.Add(new LengthValidator(5,8)); // override length

            // Add a purely code-based rule with DelegateValidator (example: username must start with a letter)
            group.Add(new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.Length >0 && char.IsLetter(s[0]))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, $"{propertyName} must start with a letter.", value, "START_WITH_LETTER");
            }));

            engine.AddValidator(nameof(User.Username), group);

            // Add code-based rules for Email using DelegateValidator
            engine.AddValidator(nameof(User.Email), new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, $"{propertyName} must be a @gmail.com address.", value, "EMAIL_DOMAIN");
            }));

            // Register notifiers via engine's publisher
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new MessageBoxNotifier());
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new SummaryNotifier());
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new RedConsoleNotifier());

            // Validate (auto-notify inside)
            var results = engine.Validate(user);

            // Simple to apply: just use attribute or add validator
            Console.WriteLine("\n==> Change Username to 'abc' and Email to valid to see pass");
            user.Username = "abc";
            user.Email = "abc@gmail.com";
            results = engine.Validate(user);
        }
    }
}
