using System;
using ValidationFramework.Core;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Demo.Validators;
using ValidationFramework.Group;
using ValidationFramework.Notification;
using ValidationFramework.Validator;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates custom validators implementing IValidator interface
    /// </summary>
    public static class Demo4_CustomValidators
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 4: Custom Validators");

            Console.WriteLine("This demo shows how to create and use custom validators.");
            Console.WriteLine("Custom validators: NoDigitValidator, NoSpecialCharValidator, AlphaOnlyValidator\n");

            Console.Write("Enter a username to validate: ");
            var username = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            var engine = new ValidationEngine();

            // Subscribe to notifications
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new ColoredConsoleNotifier(ConsoleColor.Magenta));

            // Add custom validators
            var group = new ValidatorGroup();
            group.Add(new NoDigitValidator());
            group.Add(new NoSpecialCharValidator());
            group.Add(new AlphaOnlyValidator());

            engine.AddValidator("Username", group);

            Console.WriteLine("Custom Validators:");
            Console.WriteLine("  1. NoDigitValidator - No digits allowed");
            Console.WriteLine("  2. NoSpecialCharValidator - No special characters");
            Console.WriteLine("  3. AlphaOnlyValidator - Only letters allowed\n");

            Console.WriteLine($"Testing username: '{username}'\n");

            // Manually validate each rule
            int passCount = 0;
            int totalCount = 0;

            foreach (var validator in new IValidator[]
            {
                new NoDigitValidator(),
                new NoSpecialCharValidator(),
                new AlphaOnlyValidator()
            })
            {
                totalCount++;
                var result = validator.Validate(username, "Username");
                if (!result.IsValid)
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  ✗ {result.Message}");
                    Console.ForegroundColor = oldColor;
                }
                else
                {
                    passCount++;
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  ✓ {validator.GetType().Name} passed");
                    Console.ForegroundColor = oldColor;
                }
            }

            Console.WriteLine();
            if (passCount == totalCount)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ All {totalCount} custom validation rules passed!");
                Console.ForegroundColor = oldColor;
            }
            else
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ {totalCount - passCount} out of {totalCount} rules failed");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
