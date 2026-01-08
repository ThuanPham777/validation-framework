using System;
using ValidationFramework.Core;
using ValidationFramework.Demo.Models;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Extensions;
using ValidationFramework.Fluent;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates using extension methods for common validations
    /// </summary>
    public static class Demo7_ExtensionMethods
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 7: Extension Methods");

            Console.WriteLine("This demo shows built-in extension methods for common validations.");
            Console.WriteLine("20+ extension methods available: AlphaNumeric, EmailDomain, Url, Range, etc.\n");

            var user = new ExtendedUser();

            // Input data
            Console.WriteLine("Please enter user information:");
            Console.WriteLine();

            Console.Write("Username (3-20 alphanumeric characters): ");
            user.Username = Console.ReadLine();

            Console.Write("Email (must be from gmail.com or outlook.com): ");
            user.Email = Console.ReadLine();

            Console.Write("Website (must be valid URL, e.g., https://example.com): ");
            user.Website = Console.ReadLine();

            Console.Write("Age (18-100): ");
            if (int.TryParse(Console.ReadLine(), out int age))
                user.Age = age;

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            var builder = new ValidatorBuilder<ExtendedUser>();

            builder.For(u => u.Username)
                   .Required()
                   .Length(3, 20)
                   .AlphaNumeric()
                   .WithMessage("Username must be 3-20 alphanumeric characters");

            builder.For(u => u.Email)
                   .Required()
                   .Email()
                   .EmailDomain("gmail.com", "outlook.com")
                   .WithMessage("Email must be from Gmail or Outlook");

            builder.For(u => u.Website)
                   .Url()
                   .WithMessage("Website must be a valid URL");

            builder.For(u => u.Age)
                   .Range(18, 100)
                   .WithMessage("Age must be between 18 and 100");

            var validator = builder.Build();

            // Create engine with notifications
            var engine = new ValidationEngine();
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new ColoredConsoleNotifier(ConsoleColor.DarkYellow));
            engine.Publisher.Subscribe(ValidationEventType.Validated, new SuccessNotifier());
            engine.AddFluentValidator(validator);

            Console.WriteLine("Extension Methods Used:");
            Console.WriteLine("  • Username: Required, Length(3-20), AlphaNumeric");
            Console.WriteLine("  • Email: Required, Email, EmailDomain(gmail, outlook)");
            Console.WriteLine("  • Website: Url");
            Console.WriteLine("  • Age: Range(18-100)\n");

            Console.WriteLine("Validating data...\n");
            var results = engine.ValidateWithFluent(user);
            DemoHelpers.PrintResults("Validation Results:", results);

            Console.WriteLine("\nMore Extension Methods Available:");
            Console.WriteLine("  String: NotEmpty, MinLength, MaxLength, AlphaOnly, NoSpecialChars");
            Console.WriteLine("  String: StartsWith, EndsWith, Contains, CreditCard");
            Console.WriteLine("  Comparison: Equal, NotEqual, GreaterThan, LessThan");
        }
    }
}
