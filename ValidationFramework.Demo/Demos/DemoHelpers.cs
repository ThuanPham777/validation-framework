using System;
using System.Collections.Generic;
using System.Linq;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Helper methods for demo console output
    /// </summary>
    public static class DemoHelpers
    {
        public static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();
        }

        public static void PrintSectionHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine();
        }

        public static void PrintResults(string message, List<ValidationResult> results)
        {
            Console.WriteLine($"\n{message}");

            var errors = results.Where(r => !r.IsValid).ToList();

            if (errors.Count == 0)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  ✓ All validations passed!");
                Console.ForegroundColor = oldColor;
            }
            else
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ✗ Found {errors.Count} validation error(s):");
                foreach (var error in errors)
                {
                    Console.WriteLine($"    • {error.PropertyName}: {error.Message}");
                }
                Console.ForegroundColor = oldColor;
            }

            Console.WriteLine();
        }
    }
}
