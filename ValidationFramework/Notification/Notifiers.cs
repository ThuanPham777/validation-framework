using System;
using System.Collections.Generic;
using ValidationFramework.Result;

namespace ValidationFramework.Notification
{
    public class MessageBoxNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    Console.WriteLine($"[MessageBox] {result.PropertyName}: {result.Message}");
                }
            }
        }
    }

    public class TooltipNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    Console.WriteLine($"[Tooltip] {result.PropertyName}: {result.Message}");
                }
            }
        }
    }

    public class LabelNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    Console.WriteLine($"[Label] {result.PropertyName}: {result.Message}");
                }
            }
        }
    }

    public class SummaryNotifier : IValidationNotifierSubscriber
    {
        public void Notify(List<ValidationResult> results)
        {
            Console.WriteLine("[Summary]");
            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    Console.WriteLine($"- {result.PropertyName}: {result.Message}");
                }
            }
        }
    }
}