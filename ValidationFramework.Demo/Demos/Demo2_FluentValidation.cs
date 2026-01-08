using System;
using ValidationFramework.Core;
using ValidationFramework.Demo.Models;
using ValidationFramework.Demo.Notifiers;
using ValidationFramework.Demo.Validators;
using ValidationFramework.Extensions;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo.Demos
{
    /// <summary>
    /// Demonstrates fluent validation API using AbstractValidator
    /// </summary>
    public static class Demo2_FluentValidation
    {
        public static void Run()
        {
            DemoHelpers.PrintSectionHeader("Demo 2: Fluent Validation API");

            Console.WriteLine("This demo shows type-safe fluent validation using AbstractValidator<T>.");
            Console.WriteLine("Model: Product with fluent validation rules\n");

            var product = new Product();

            // Input data
            Console.WriteLine("Please enter product information:");
            Console.WriteLine();

            Console.Write("Product Name (3-100 characters): ");
            product.Name = Console.ReadLine();

            Console.Write("SKU (Format: ABC123 - 3 letters + 3 digits): ");
            product.SKU = Console.ReadLine();

            Console.Write("Price: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
                product.Price = price;

            Console.Write("Stock (0-10000): ");
            if (int.TryParse(Console.ReadLine(), out int stock))
                product.Stock = stock;

            Console.WriteLine();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();

            // Setup validation engine with notifications
            var engine = new ValidationEngine();
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new ColoredConsoleNotifier(ConsoleColor.Yellow));
            engine.Publisher.Subscribe(ValidationEventType.Validated, new SuccessNotifier());

            // Register fluent validator
            var productValidator = new ProductValidator();
            engine.AddFluentValidator(productValidator);

            Console.WriteLine("Validating product data...\n");
            var results = engine.ValidateWithFluent(product);
            DemoHelpers.PrintResults("Validation Results:", results);

            Console.WriteLine("\nFluent Validation Rules Applied:");
            Console.WriteLine("  • Name: Required, Length(3-100)");
            Console.WriteLine("  • SKU: Required, Regex(^[A-Z]{3}\\d{3}$)");
            Console.WriteLine("  • Price: GreaterThan(0)");
            Console.WriteLine("  • Stock: Range(0-10000)");
        }
    }
}
