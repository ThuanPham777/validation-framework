using System;
using ValidationFramework.Core;
using ValidationFramework.Demo.Models;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates attribute-based validation using ValidationEngine
    /// </summary>
    public static class Demo1_AttributeBased
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 1: Attribute-Based Validation");

            Console.WriteLine("This demo shows validation using attributes on model properties.");
            Console.WriteLine("Model: AttributeUser with [Required], [Email], [Phone], [Length] attributes\n");

            var user = new AttributeUser();

            // Input data
            Console.WriteLine("Please enter user information:");
            Console.WriteLine();

            Console.Write("Username (3-20 characters): ");
            user.Username = Console.ReadLine();

            Console.Write("Email: ");
            user.Email = Console.ReadLine();

            Console.Write("Phone: ");
            user.Phone = Console.ReadLine();

            Console.Write("Age: ");
            if (int.TryParse(Console.ReadLine(), out int age))
                user.Age = age;

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            // Setup validation engine with notifications
            var engine = new ValidationEngine();
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new ColoredConsoleNotifier(ConsoleColor.Red));
            engine.Publisher.Subscribe(ValidationEventType.Validated, new SuccessNotifier());

            Console.WriteLine("Validating user data...\n");
            var results = engine.Validate(user);
            DemoHelpers.PrintResults("Validation Results:", results);

            // Show what was validated
            Console.WriteLine("\nValidation Rules Applied:");
            Console.WriteLine("  • Username: Required, Length(3-20)");
            Console.WriteLine("  • Email: Required, Valid email format");
            Console.WriteLine("  • Phone: Valid phone format");
            Console.WriteLine("  • Age: Required");
        }
    }
}
