using ValidationFramework.Fluent;
using ValidationFramework.Extensions;
using ValidationFramework.Core;
using ValidationFramework.Result;

namespace ValidationFramework.Sample.Fluent
{
    /// <summary>
    /// Sample demonstrating the Fluent Validation API
    /// </summary>
    public class FluentValidationSample
    {
        public class Product
        {
            public string Name { get; set; } = string.Empty;
            public string SKU { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string? Description { get; set; }
        }

        public class Customer
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public int Age { get; set; }
            public string? Website { get; set; }
        }

        // =========================
        // Example 1: ValidatorBuilder
        // =========================
        public static void Example1_ValidatorBuilder()
        {
            Console.WriteLine("=== Example 1: Using ValidatorBuilder ===\n");

            var builder = new ValidatorBuilder<Product>();

            builder.For(p => p.Name)
                   .Required()
                   .Length(3, 100)
                   .WithMessage("Product name must be 3-100 characters");

            builder.For(p => p.SKU)
                   .Required()
                   .AlphaNumeric()
                   .Length(5, 20);

            builder.For(p => p.Price)
                   .GreaterThan(0m)
                   .WithMessage("Price must be greater than 0");

            builder.For(p => p.Stock)
                   .Range(0, 10000);

            var validator = builder.Build();

            var product = new Product
            {
                Name = "AB",
                SKU = "ABC-123",
                Price = -10,
                Stock = 50
            };

            var results = validator.Validate(product);
            PrintResults(results);

            Console.WriteLine("\nFixed product:");

            product.Name = "Valid Product";
            product.SKU = "ABC123";
            product.Price = 99.99m;

            results = validator.Validate(product);
            PrintResults(results);
        }

        // =========================
        // Example 2: AbstractValidator
        // =========================
        public class CustomerValidator : AbstractValidator<Customer>
        {
            public CustomerValidator()
            {
                RuleFor(c => c.FirstName)
                    .Required()
                    .Length(2, 50)
                    .AlphaOnly()
                    .WithMessage("First name must be 2-50 letters only");

                RuleFor(c => c.LastName)
                    .Required()
                    .Length(2, 50)
                    .AlphaOnly();

                RuleFor(c => c.Email)
                    .Required()
                    .Email()
                    .EmailDomain("gmail.com", "outlook.com", "yahoo.com");

                RuleFor(c => c.Phone)
                    .Required()
                    .Phone();

                RuleFor(c => c.Age)
                    .Range(18, 120)
                    .WithMessage("Age must be between 18 and 120");

                RuleFor(c => c.Website)
                    .Url()
                    .WithMessage("Website must be a valid URL");
            }
        }

        public static void Example2_AbstractValidator()
        {
            Console.WriteLine("\n=== Example 2: Using AbstractValidator ===\n");

            var validator = new CustomerValidator();

            var customer = new Customer
            {
                FirstName = "John123",
                LastName = "Doe",
                Email = "john@company.com",
                Phone = "123-456-7890",
                Age = 15,
                Website = "not-a-url"
            };

            var results = validator.Validate(customer);
            PrintResults(results);

            Console.WriteLine("\nFixed customer:");

            customer.FirstName = "John";
            customer.Email = "john@gmail.com";
            customer.Age = 25;
            customer.Website = "https://example.com";

            results = validator.Validate(customer);
            PrintResults(results);
        }

        // =========================
        // Example 3: ValidationEngine
        // =========================
        public static void Example3_EngineIntegration()
        {
            Console.WriteLine("\n=== Example 3: Integration with ValidationEngine ===\n");

            var engine = new ValidationEngine();

            engine.AddFluentValidator<Product>(builder =>
            {
                builder.For(p => p.Name)
                       .Required()
                       .NotEmpty()
                       .MinLength(3);

                builder.For(p => p.SKU)
                       .Required()
                       .Regex(@"^[A-Z]{3}\d{3}$")
                       .WithMessage("SKU must be 3 uppercase letters followed by 3 digits");

                builder.For(p => p.Price)
                       .GreaterThan(0m);
            });

            var product = new Product
            {
                Name = "Test Product",
                SKU = "ABC123",
                Price = 50m,
                Stock = 100
            };

            var results = engine.ValidateWithFluent(product);
            PrintResults(results);
        }

        // =========================
        // Example 4: Custom Rules
        // =========================
        public static void Example4_CustomRules()
        {
            Console.WriteLine("\n=== Example 4: Custom Validation Rules ===\n");

            var builder = new ValidatorBuilder<Product>();

            builder.For(p => p.Name)
                   .Required()
                   .Custom(
                       name => !name.Contains("invalid", StringComparison.OrdinalIgnoreCase),
                       "Product name cannot contain 'invalid'"
                   );

            builder.For(p => p.Price)
                   .Custom((price, propertyName) =>
                   {
                       if (price < 0)
                       {
                           return ValidationResult.Fail(
                               propertyName,
                               "Price cannot be negative",
                               price,
                               "PRICE_NEGATIVE"
                           );
                       }

                       if (price > 1_000_000)
                       {
                           return ValidationResult.Fail(
                               propertyName,
                               "Price seems unrealistically high",
                               price,
                               "PRICE_TOO_HIGH"
                           );
                       }

                       return ValidationResult.Ok(propertyName);
                   });

            builder.For(p => p.Description)
                   .MaxLength(500)
                   .NoSpecialChars();

            var validator = builder.Build();

            var product = new Product
            {
                Name = "Invalid Product",
                SKU = "TEST",
                Price = 2_000_000m,
                Stock = 50,
                Description = "This has special chars: @#$%"
            };

            var results = validator.Validate(product);
            PrintResults(results);
        }

        // =========================
        // Example 5: Chaining Rules
        // =========================
        public static void Example5_ChainingRules()
        {
            Console.WriteLine("\n=== Example 5: Chaining Multiple Rules ===\n");

            var builder = new ValidatorBuilder<Customer>();

            builder.For(c => c.Email)
                   .Required()
                   .WithMessage("Email is required")
                   .Email()
                   .WithMessage("Email format is invalid")
                   .EmailDomain("gmail.com", "outlook.com")
                   .WithMessage("Only Gmail and Outlook emails are allowed");

            builder.For(c => c.Phone)
                   .Required()
                   .Phone()
                   .Custom(
                       phone => phone.StartsWith("+1") || phone.StartsWith("1"),
                       "Phone must be a US number starting with +1 or 1"
                   );

            var validator = builder.Build();

            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@yahoo.com",
                Phone = "+44123456789"
            };

            var results = validator.Validate(customer);
            PrintResults(results);
        }

        // =========================
        // Helpers
        // =========================
        private static void PrintResults(List<ValidationResult> results)
        {
            if (results.Count == 0 || results.All(r => r.IsValid))
            {
                Console.WriteLine("✓ All validations passed!");
                return;
            }

            Console.WriteLine("✗ Validation errors:");

            foreach (var result in results.Where(r => !r.IsValid))
            {
                Console.WriteLine($"  - {result.PropertyName}: {result.Message}");

                if (result.ErrorCode != null)
                {
                    Console.WriteLine($"    Error Code: {result.ErrorCode}");
                }
            }
        }

        public static void RunAllExamples()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ Fluent Validation API - Sample Demonstrations          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            Example1_ValidatorBuilder();
            Example2_AbstractValidator();
            Example3_EngineIntegration();
            Example4_CustomRules();
            Example5_ChainingRules();

            Console.WriteLine("\n╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            All Examples Completed!                     ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        }
    }
}
