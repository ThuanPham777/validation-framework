using ValidationFramework.Fluent;
using ValidationFramework.Extensions;

namespace ValidationFramework.Tests.Fluent
{
    /// <summary>
    /// Example unit tests for Fluent Validation API
    /// Note: This is a sample test file. To actually run these tests,
    /// you need to add a test project with xUnit, NUnit, or MSTest.
    /// </summary>
    public class FluentValidationTests
    {
        public class TestUser
        {
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int Age { get; set; }
        }

        public class TestUserValidator : AbstractValidator<TestUser>
        {
            public TestUserValidator()
            {
                RuleFor(u => u.Username)
                    .Required()
                    .Length(3, 20)
                    .AlphaNumeric();

                RuleFor(u => u.Email)
                    .Required()
                    .Email();

                RuleFor(u => u.Age)
                    .Range(18, 100);
            }
        }

        // =========================
        // Unit test examples
        // =========================

        public void ValidUser_ShouldPassValidation()
        {
            // Arrange
            var validator = new TestUserValidator();
            var user = new TestUser
            {
                Username = "john123",
                Email = "john@example.com",
                Age = 25
            };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.Empty(results.Where(r => !r.IsValid));
            // or Assert.True(results.All(r => r.IsValid));
        }

        public void InvalidUsername_ShouldFailValidation()
        {
            // Arrange
            var validator = new TestUserValidator();
            var user = new TestUser
            {
                Username = "ab", // Too short
                Email = "john@example.com",
                Age = 25
            };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.NotEmpty(results.Where(r => !r.IsValid && r.PropertyName == "Username"));
        }

        public void InvalidEmail_ShouldFailValidation()
        {
            // Arrange
            var validator = new TestUserValidator();
            var user = new TestUser
            {
                Username = "john123",
                Email = "not-an-email", // Invalid email
                Age = 25
            };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.NotEmpty(results.Where(r => !r.IsValid && r.PropertyName == "Email"));
        }

        public void AgeOutOfRange_ShouldFailValidation()
        {
            // Arrange
            var validator = new TestUserValidator();
            var user = new TestUser
            {
                Username = "john123",
                Email = "john@example.com",
                Age = 15 // Too young
            };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.NotEmpty(results.Where(r => !r.IsValid && r.PropertyName == "Age"));
        }

        public void CustomValidationRule_ShouldWork()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();

            builder.For(u => u.Username)
                   .Custom(
                       name => !name.Contains("admin", StringComparison.OrdinalIgnoreCase),
                       "Username cannot contain 'admin'"
                   );

            var validator = builder.Build();
            var user = new TestUser { Username = "admin123" };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.NotEmpty(results.Where(r => !r.IsValid));
        }

        public void ChainedRules_ShouldAllBeEvaluated()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();

            builder.For(u => u.Email)
                   .Required()
                   .Email()
                   .EmailDomain("company.com");

            var validator = builder.Build();
            var user = new TestUser { Email = "test@gmail.com" };

            // Act
            var results = validator.Validate(user);

            // Assert
            // Should fail on EmailDomain check
            // Assert.NotEmpty(results.Where(r => !r.IsValid && r.PropertyName == "Email"));
        }

        public void WithMessage_ShouldOverrideDefaultMessage()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();

            builder.For(u => u.Username)
                   .Required()
                   .WithMessage("Custom: Username is mandatory");

            var validator = builder.Build();
            var user = new TestUser { Username = string.Empty };

            // Act
            var results = validator.Validate(user);

            // Assert
            // var error = results.FirstOrDefault(r => !r.IsValid && r.PropertyName == "Username");
            // Assert.NotNull(error);
            // Assert.Contains("Custom: Username is mandatory", error.Message);
        }

        public void ExtensionMethods_ShouldWork()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();

            builder.For(u => u.Username)
                   .NotEmpty()
                   .MinLength(3)
                   .MaxLength(20)
                   .AlphaNumeric();

            var validator = builder.Build();
            var user = new TestUser { Username = "user@123" }; // Has special char

            // Act
            var results = validator.Validate(user);

            // Assert
            // Assert.NotEmpty(results.Where(r => !r.IsValid));
        }

        public void MultipleProperties_ShouldValidateAll()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();

            builder.For(u => u.Username).Required();
            builder.For(u => u.Email).Required();
            builder.For(u => u.Age).GreaterThan(0);

            var validator = builder.Build();
            var user = new TestUser(); // All empty/default

            // Act
            var results = validator.Validate(user);

            // Assert
            // Should have errors for Username, Email, and Age
            // Assert.True(results.Count(r => !r.IsValid) >= 3);
        }

        public void ValidatorBuilder_Build_ShouldReturnValidator()
        {
            // Arrange
            var builder = new ValidatorBuilder<TestUser>();
            builder.For(u => u.Username).Required();

            // Act
            var validator = builder.Build();

            // Assert
            // Assert.NotNull(validator);
            // Assert.IsAssignableFrom<IFluentValidator<TestUser>>(validator);
        }
    }

    // =========================
    // Integration tests
    // =========================
    public class FluentValidationIntegrationTests
    {
        public class Product
        {
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        public void ValidationEngine_WithFluentValidator_ShouldCombineRules()
        {
            // Arrange
            var engine = new ValidationFramework.Core.ValidationEngine();

            engine.AddFluentValidator<Product>(builder =>
            {
                builder.For(p => p.Name)
                       .Required()
                       .MinLength(3);

                builder.For(p => p.Price)
                       .GreaterThan(0m);
            });

            var product = new Product
            {
                Name = "AB",   // Too short
                Price = -10   // Negative
            };

            // Act
            var results = engine.ValidateWithFluent(product);

            // Assert
            // Should have errors for both Name and Price
            // Assert.NotEmpty(results.Where(r => !r.IsValid));
        }
    }

    // =========================
    // Performance tests
    // =========================
    public class FluentValidationPerformanceTests
    {
        public class SimpleModel
        {
            public string Field1 { get; set; } = string.Empty;
            public string Field2 { get; set; } = string.Empty;
            public int Field3 { get; set; }
        }

        public void ValidateThousandObjects_ShouldBePerformant()
        {
            // Arrange
            var builder = new ValidatorBuilder<SimpleModel>();

            builder.For(m => m.Field1).Required().Length(3, 50);
            builder.For(m => m.Field2).Email();
            builder.For(m => m.Field3).Range(1, 100);

            var validator = builder.Build();

            var models = Enumerable.Range(1, 1000)
                .Select(i => new SimpleModel
                {
                    Field1 = $"Value{i}",
                    Field2 = $"test{i}@example.com",
                    Field3 = i % 100
                })
                .ToList();

            // Act
            var startTime = DateTime.Now;

            foreach (var model in models)
            {
                var results = validator.Validate(model);
            }

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            // Assert
            // Assert.True(duration.TotalSeconds < 1, "Should validate 1000 objects in less than 1 second");
            Console.WriteLine($"Validated 1000 objects in {duration.TotalMilliseconds}ms");
        }
    }
}
