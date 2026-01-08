using System;
using ValidationFramework.Demo.Validators;
using ValidationFramework.Group;
using ValidationFramework.Validator;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates grouping multiple validators together
    /// </summary>
    public static class Demo5_ValidatorGroups
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 5: Validator Groups");

            Console.WriteLine("This demo shows how to group multiple validators together.");
            Console.WriteLine("ValidatorGroup stops at the first validation failure.\n");

            Console.Write("Enter a username to validate (try: 'user@123' or 'johnsmith'): ");
            var username = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            var group = new ValidatorGroup();
            group.Add(new RequiredValidator());
            group.Add(new LengthValidator(5, 15));
            group.Add(new NoSpecialCharValidator());
            group.Add(new NoDigitValidator());

            Console.WriteLine("Validator Group Rules:");
            Console.WriteLine("  1. Required - Must have a value");
            Console.WriteLine("  2. Length (5-15) - Must be 5-15 characters");
            Console.WriteLine("  3. No special characters - Only letters and digits");
            Console.WriteLine("  4. No digits - Only letters allowed\n");

            Console.WriteLine($"Validating username: '{username}'\n");

            var result = group.Validate(username, "Username");

            if (!result.IsValid)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"? Validation failed: {result.Message}");
                Console.WriteLine($"   (Stopped at first failure - this is ValidatorGroup behavior)");
                Console.ForegroundColor = oldColor;
            }
            else
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("? All rules in group passed!");
                Console.ForegroundColor = oldColor;
            }

            // Show suggestion
            Console.WriteLine("\nTry different inputs:");
            Console.WriteLine("  • 'ab' - Too short (fails at rule 2)");
            Console.WriteLine("  • 'user@123' - Has special char (fails at rule 3)");
            Console.WriteLine("  • 'user123' - Has digits (fails at rule 4)");
            Console.WriteLine("  • 'johnsmith' - Valid (passes all rules)");
        }
    }
}
