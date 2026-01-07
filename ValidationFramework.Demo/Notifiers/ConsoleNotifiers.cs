using System;
using System.Collections.Generic;
using System.Linq;
using ValidationFramework.Notification;
using ValidationFramework.Result;

namespace ValidationFramework.Demo.Notifiers
{
    /// <summary>
    /// Notifier that displays errors in colored console output
    /// </summary>
    public class ColoredConsoleNotifier : IValidationNotifierSubscriber
    {
        private readonly ConsoleColor _color;

        public ColoredConsoleNotifier(ConsoleColor color)
        {
            _color = color;
        }

        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0) return;

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = _color;
            Console.WriteLine($"[{_color} Notifier]");
            foreach (var error in errors)
            {
                Console.WriteLine($"  • {error.PropertyName}: {error.Message}");
            }
            Console.ForegroundColor = oldColor;
        }
    }

    /// <summary>
    /// Notifier that displays detailed error information
    /// </summary>
    public class DetailedNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            var errors = results.Where(r => !r.IsValid).ToList();
            if (errors.Count == 0) return;

            Console.WriteLine("\n[Detailed Error Report]");
            foreach (var error in errors)
            {
                Console.WriteLine($"  Property: {error.PropertyName}");
                Console.WriteLine($"  Message:  {error.Message}");
                Console.WriteLine($"  Value:    {error.AttemptedValue ?? "(null)"}");
                Console.WriteLine($"  Code:     {error.ErrorCode ?? "N/A"}");
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Notifier that displays success message when all validations pass
    /// </summary>
    public class SuccessNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            if (results.All(r => r.IsValid))
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ All validations passed successfully!");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
