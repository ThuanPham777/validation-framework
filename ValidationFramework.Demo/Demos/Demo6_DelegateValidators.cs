using System;
using ValidationFramework.Core;
using ValidationFramework.Demo.Models;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Notification;
using ValidationFramework.Result;
using ValidationFramework.Validator;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates quick inline validators using DelegateValidator
    /// </summary>
    public static class Demo6_DelegateValidators
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 6: Delegate Validators (Quick Inline Rules)");

            Console.WriteLine("This demo shows quick inline validators using lambda expressions.");
            Console.WriteLine("No need to create separate validator classes!\n");

            var user = new DelegateUser();

            // Input data
            Console.WriteLine("Please enter user information:");
            Console.WriteLine();

            Console.Write("Username (must start with a letter): ");
            user.Username = Console.ReadLine();

            Console.Write("Email (must be from @company.com or @example.com): ");
            user.Email = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            var engine = new ValidationEngine();

            // Subscribe to notifications
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new ColoredConsoleNotifier(ConsoleColor.Cyan));
            engine.Publisher.Subscribe(ValidationEventType.Validated, new SuccessNotifier());

            // Delegate validator for username - must start with a letter
            engine.AddValidator("Username", new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.Length > 0 && char.IsLetter(s[0]))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, "Username must start with a letter", value, "START_WITH_LETTER");
            }));

            // Delegate validator for email - must be from specific domain
            engine.AddValidator("Email", new DelegateValidator((value, propertyName) =>
            {
                if (value is string email &&
                    (email.EndsWith("@company.com", StringComparison.OrdinalIgnoreCase) ||
                     email.EndsWith("@example.com", StringComparison.OrdinalIgnoreCase)))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, "Email must be from @company.com or @example.com", value, "EMAIL_DOMAIN");
            }));

            Console.WriteLine("Delegate Validators:");
            Console.WriteLine("  • Username: Must start with a letter (inline lambda)");
            Console.WriteLine("  • Email: Must be from @company.com or @example.com (inline lambda)\n");

            Console.WriteLine("Validating data...\n");

            // Manually test each validator
            var usernameValidator = new DelegateValidator((value, propertyName) =>
            {
                if (value is string s && s.Length > 0 && char.IsLetter(s[0]))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, "Username must start with a letter", value, "START_WITH_LETTER");
            });

            var emailValidator = new DelegateValidator((value, propertyName) =>
            {
                if (value is string email &&
                    (email.EndsWith("@company.com", StringComparison.OrdinalIgnoreCase) ||
                     email.EndsWith("@example.com", StringComparison.OrdinalIgnoreCase)))
                    return ValidationResult.Ok(propertyName);
                return ValidationResult.Fail(propertyName, "Email must be from @company.com or @example.com", value, "EMAIL_DOMAIN");
            });

            var usernameResult = usernameValidator.Validate(user.Username, "Username");
            var emailResult = emailValidator.Validate(user.Email, "Email");

            int passCount = 0;

            if (!usernameResult.IsValid)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Username: {usernameResult.Message}");
                Console.ForegroundColor = oldColor;
            }
            else
            {
                passCount++;
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Username validation passed");
                Console.ForegroundColor = oldColor;
            }

            if (!emailResult.IsValid)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Email: {emailResult.Message}");
                Console.ForegroundColor = oldColor;
            }
            else
            {
                passCount++;
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Email validation passed");
                Console.ForegroundColor = oldColor;
            }

            Console.WriteLine();
            if (passCount == 2)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ All delegate validations passed!");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
