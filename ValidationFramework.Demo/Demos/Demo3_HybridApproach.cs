using System;
using System.Linq;
using ValidationFramework.Core;
using ValidationFramework.Demo.Models;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Extensions;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates hybrid approach combining attributes and fluent validation
    /// </summary>
  public static class Demo3_HybridApproach
  {
        public static void Run()
   {
   DemoHelpers.PrintSectionHeader("Demo 3: Hybrid Approach (Attributes + Fluent)");

     Console.WriteLine("This demo combines attribute-based and fluent validation.");
        Console.WriteLine("Attributes for simple rules, Fluent API for complex rules.\n");

     var customer = new Customer();

       // Input data
Console.WriteLine("Please enter customer information:");
       Console.WriteLine();

  Console.Write("First Name (2-50 characters, letters only): ");
      customer.FirstName = Console.ReadLine();

        Console.Write("Last Name (2-50 characters, letters only): ");
   customer.LastName = Console.ReadLine();

            Console.Write("Email (must be from gmail.com, outlook.com, or yahoo.com): ");
     customer.Email = Console.ReadLine();

        Console.Write("Password (min 8 chars, must have uppercase, lowercase, digit): ");
  customer.Password = ReadPassword();

     Console.WriteLine();
       Console.WriteLine(new string('-', 60));
Console.WriteLine();

       var engine = new ValidationEngine();

      // Subscribe to notifications
          engine.Publisher.Subscribe(ValidationEventType.Invalid, new DetailedNotifier());
engine.Publisher.Subscribe(ValidationEventType.Validated, new SuccessNotifier());

// Add fluent validators for complex rules
   engine.AddFluentValidator<Customer>(builder =>
    {
       builder.For(c => c.FirstName)
     .AlphaOnly()
  .WithMessage("First name must contain only letters");

builder.For(c => c.LastName)
      .AlphaOnly()
    .WithMessage("Last name must contain only letters");

            builder.For(c => c.Email)
  .EmailDomain("gmail.com", "outlook.com", "yahoo.com")
   .WithMessage("Email must be from Gmail, Outlook, or Yahoo");

builder.For(c => c.Password)
         .MinLength(8)
 .Custom(p => p.Any(char.IsUpper), "Password must contain at least one uppercase letter")
      .Custom(p => p.Any(char.IsLower), "Password must contain at least one lowercase letter")
  .Custom(p => p.Any(char.IsDigit), "Password must contain at least one digit");
      });

            Console.WriteLine("Validating customer data...\n");
       var results = engine.ValidateWithFluent(customer);
DemoHelpers.PrintResults("Validation Results:", results);

        Console.WriteLine("\nHybrid Validation Rules:");
Console.WriteLine("Attribute Rules:");
   Console.WriteLine("  • FirstName: Required, Length(2-50)");
Console.WriteLine("  • LastName: Required, Length(2-50)");
   Console.WriteLine("  • Email: Required, Valid email format");
Console.WriteLine("  • Password: Required");
     Console.WriteLine("\nFluent Rules:");
 Console.WriteLine("  • FirstName: AlphaOnly");
      Console.WriteLine("  • LastName: AlphaOnly");
   Console.WriteLine("  • Email: EmailDomain(gmail, outlook, yahoo)");
     Console.WriteLine("  • Password: MinLength(8), Uppercase, Lowercase, Digit");
        }

        private static string ReadPassword()
 {
       string password = "";
      ConsoleKeyInfo key;
            do
     {
    key = Console.ReadKey(true);
       if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
       {
       password += key.KeyChar;
    Console.Write("*");
    }
       else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
     {
         password = password.Substring(0, password.Length - 1);
    Console.Write("\b \b");
 }
    } while (key.Key != ConsoleKey.Enter);
   Console.WriteLine();
            return password;
        }
    }
}
