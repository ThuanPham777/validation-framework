# ValidationFramework

A comprehensive, extensible validation framework for .NET applications supporting multiple validation strategies and UI frameworks.

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## ?? Overview

ValidationFramework provides a flexible, powerful validation system that supports:
- **Attribute-Based Validation** - Declarative validation using attributes
- **Fluent Validation API** - Type-safe, chainable validation rules
- **Hybrid Approach** - Combine both strategies
- **Multi-Platform** - Console, WinForms, WinUI, WPF
- **Extensible Notification System** - Flexible UI feedback

## ? Key Features

- ? **Multiple Validation Strategies** - Choose attribute-based, fluent API, or both
- ? **Type-Safe Fluent API** - Compile-time checking with IntelliSense support
- ? **Extensible Architecture** - Easy to add custom validators and rules
- ? **UI Framework Agnostic** - Works with any .NET UI framework
- ? **Notification System** - Publish/subscribe pattern for validation results
- ? **Composable Validators** - Combine validators with AND, OR, NOT logic
- ? **Async Support** - Handle database and API validations
- ? **Modern .NET** - Supports .NET 8 and .NET 9

## ?? Quick Start

### Attribute-Based Validation

```csharp
using ValidationFramework.Attributes;
using ValidationFramework.Core;

public class User
{
    [Required(ErrorMessage = "Username is required")]
    [Length(3, 20, ErrorMessage = "Username must be 3-20 characters")]
    public string Username { get; set; }

    [Required]
    [Email(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
}

// Validate
var engine = new ValidationEngine();
var results = engine.Validate(user);
```

### Fluent Validation API

```csharp
using ValidationFramework.Fluent;
using ValidationFramework.Extensions;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username)
       .Required()
            .Length(3, 20)
       .AlphaNumeric();

    RuleFor(u => u.Email)
   .Required()
      .Email();

        RuleFor(u => u.Age)
   .Range(18, 120);
    }
}

// Validate
var validator = new UserValidator();
var results = validator.Validate(user);
```

### Hybrid Approach (Recommended)

```csharp
// Simple rules as attributes
public class User
{
    [Required]
    [Length(3, 20)]
    public string Username { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }
    
    public string Password { get; set; }
}

// Complex rules with fluent API
var engine = new ValidationEngine();
engine.AddFluentValidator<User>(builder =>
{
    builder.For(u => u.Password)
        .Required()
        .MinLength(8)
        .Custom(p => p.Any(char.IsUpper), "Must contain uppercase")
        .Custom(p => p.Any(char.IsDigit), "Must contain digit");
});

// Validates both attribute and fluent rules
var results = engine.ValidateWithFluent(user);
```

## ?? Components

### Core Components

| Component | Description |
|-----------|-------------|
| **ValidationEngine** | Central orchestrator for validation |
| **IValidator** | Base interface for all validators |
| **ValidationResult** | Represents validation outcome |
| **ValidatorGroup** | Groups multiple validators |

### Attribute-Based Validation

| Attribute | Description |
|-----------|-------------|
| `[Required]` | Value must not be null/empty |
| `[Email]` | Valid email format |
| `[Phone]` | Valid phone number |
| `[Length(min, max)]` | String length validation |
| `[Regex(pattern)]` | Regex pattern matching |
| `[CustomValidation(type)]` | Use custom validator class |

### Fluent Validation API

| Component | Description |
|-----------|-------------|
| **ValidatorBuilder<T>** | Builder for creating fluent validators |
| **AbstractValidator<T>** | Base class for reusable validators |
| **PropertyValidator<T, TProperty>** | Property-level validation |
| **IFluentValidator<T>** | Interface for fluent validators |

### Extension Methods (20+)

**String Validators:**
- `NotEmpty()`, `MinLength()`, `MaxLength()`
- `AlphaOnly()`, `AlphaNumeric()`, `NoSpecialChars()`, `NoDigits()`
- `StartsWith()`, `EndsWith()`, `Contains()`
- `EmailDomain()`, `Url()`, `CreditCard()`

**Comparison Validators:**
- `Equal()`, `NotEqual()`, `GreaterThan()`, `LessThan()`, `Range()`

### Notification System

| Component | Description |
|-----------|-------------|
| **NotificationPublisher** | Publishes validation events |
| **IValidationNotifierSubscriber** | Interface for notifiers |
| **ValidationEventType** | Validated, Invalid events |

**Built-in Notifiers:**
- MessageBox, Tooltip, Label, Summary (Console)
- WinForms: MessageBox, Label, TextBox Highlighting, ErrorProvider
- WinUI: ContentDialog, InfoBar, TextBlock, TextBox Highlighting

### Advanced Features

| Feature | Description |
|---------|-------------|
| **Composite Validators** | AND, OR, NOT, WHEN, UNLESS logic |
| **Async Validators** | Database/API validation support |
| **Common Rules** | Reusable validation rule factories |
| **Delegate Validators** | Quick inline validators |

## ?? Documentation

- **[Complete Documentation](ValidationFramework/DOCUMENTATION.md)** - Comprehensive guide
- **[Quick Start Guide](ValidationFramework/Fluent/QUICKSTART.md)** - 5-minute introduction
- **[Migration Guide](ValidationFramework/Fluent/MIGRATION_GUIDE.md)** - Migrate from attributes to fluent
- **[API Reference](ValidationFramework/DOCUMENTATION.md#api-reference)** - Complete API documentation

## ?? Examples

### Example 1: ValidatorBuilder

```csharp
var builder = new ValidatorBuilder<Product>();

builder.For(p => p.Name)
    .Required()
    .Length(3, 100)
    .WithMessage("Product name must be 3-100 characters");

builder.For(p => p.SKU)
    .Required()
    .Regex(@"^[A-Z]{3}\d{3}$")
    .WithMessage("SKU must be 3 letters followed by 3 digits");

builder.For(p => p.Price)
    .GreaterThan(0m)
    .WithMessage("Price must be positive");

var validator = builder.Build();
var results = validator.Validate(product);
```

### Example 2: Custom Validators

```csharp
// Define custom validator
public class NoDigitValidator : IValidator
{
    public ValidationResult Validate(object value, string propertyName)
    {
        if (value is string s && Regex.IsMatch(s, @"\d"))
            return ValidationResult.Fail(propertyName, "Must not contain digits");
        return ValidationResult.Ok(propertyName);
    }
}

// Use in validator group
var group = new ValidatorGroup();
group.Add(new RequiredValidator());
group.Add(new NoDigitValidator());
group.Add(new LengthValidator(3, 20));

engine.AddValidator("Username", group);
```

### Example 3: Notification System

```csharp
var engine = new ValidationEngine();

// Subscribe to validation events
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new MessageBoxNotifier());

engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new SummaryNotifier());

// Validation automatically triggers notifications
var results = engine.Validate(user);
```

### Example 4: Composite Validators

```csharp
using ValidationFramework.Fluent.Composite;

// Combine validators with logical operators
var validator = new RequiredValidator()
    .And(new LengthValidator(3, 20))
    .And(new AlphaOnlyValidator());

// Conditional validation
var conditionalValidator = new EmailValidator()
    .When(value => value is string s && !string.IsNullOrEmpty(s));
```

### Example 5: Async Validation

```csharp
using ValidationFramework.Fluent.Async;

// Check username uniqueness
var uniqueUsername = AsyncValidators.UniqueUsernameValidator();
var result = await uniqueUsername.ValidateAsync("john", "Username");

// Custom async validator
var customAsync = AsyncValidators.CustomAsync<string>(
    async username =>
    {
        await Task.Delay(100); // Simulate DB call
        return !existingUsernames.Contains(username);
    },
    "Username is already taken"
);
```

## ?? Demo Applications

### Console Demo
**Location:** `ValidationFramework.Demo`

Demonstrates:
- Attribute-based validation
- Fluent validation
- Custom validators
- Delegate validators
- Notification system

**Run:**
```bash
cd ValidationFramework.Demo
dotnet run
```

### Windows Forms Demo
**Location:** `ValidationFramework.Demo.Winforms`

Features:
- User registration form
- Real-time validation
- Multiple notifiers (MessageBox, Label, TextBox highlighting, ErrorProvider)
- Custom validators

**Run:**
```bash
cd ValidationFramework.Demo.Winforms
dotnet run
```

### WinUI 3 Demo
**Location:** `ValidationFramework.Demo.WinUI`

Features:
- Modern UI validation
- ContentDialog, InfoBar, TextBlock notifiers
- TextBox highlighting
- Fluent and attribute validation

**Run:**
```bash
cd ValidationFramework.Demo.WinUI
dotnet run
```

## ??? Architecture

```
ValidationFramework/
??? Attributes/            # Attribute-based validation
?   ??? RequiredAttribute
? ??? EmailAttribute
?   ??? LengthAttribute
?   ??? PhoneAttribute
?   ??? RegexAttribute
?   ??? CustomValidationAttribute
??? Validator/               # Core validators
?   ??? IValidator
?   ??? RequiredValidator
?   ??? EmailValidator
?   ??? LengthValidator
?   ??? PhoneValidator
?   ??? RegexValidator
?   ??? DelegateValidator
??? Fluent/                # Fluent validation API
?   ??? IFluentValidator
?   ??? ValidatorBuilder
?   ??? PropertyValidator
?   ??? AbstractValidator
?   ??? Rules/
?   ?   ??? CommonRules
?   ??? Composite/
?   ?   ??? CompositeValidators
?   ??? Async/
?       ??? AsyncValidators
??? Core/   # Core engine
?   ??? ValidationEngine
??? Result/ # Validation results
?   ??? ValidationResult
??? Notification/            # Notification system
?   ??? IValidationNotifierSubscriber
?   ??? NotificationPublisher
?   ??? ValidationEventType
?   ??? Notifiers
??? Extensions/   # Extension methods
?   ??? ValidationEngineExtensions
?   ??? PropertyValidatorExtensions
??? Group/    # Validator grouping
?   ??? ValidatorGroup
??? Factory/               # Validator factory
    ??? ValidatorFactory
```

## ?? Testing

Example test structure:

```csharp
[Fact]
public void UserValidator_InvalidEmail_ShouldFail()
{
    // Arrange
    var validator = new UserValidator();
    var user = new User { Email = "invalid-email" };
    
 // Act
    var results = validator.Validate(user);
    
    // Assert
    Assert.NotEmpty(results.Where(r => 
 !r.IsValid && r.PropertyName == "Email"));
}
```

See `ValidationFramework\Tests\FluentValidationTests.cs` for more examples.

## ?? Use Cases

### When to Use Attribute-Based Validation

? Simple validations (Required, Email, Length)
? Quick prototyping
? Declarative approach preferred
? Rules unlikely to change

### When to Use Fluent Validation

? Complex validation logic
? Multiple conditions per property
? Cross-property validation
? Reusable validator classes
? Unit testing validators
? Type-safe validation

### When to Use Hybrid Approach

? Most real-world applications
? Simple rules as attributes
? Complex rules as fluent
? Best of both worlds

## ??? Extensibility

### Create Custom Attribute

```csharp
public sealed class RangeAttribute : ValidationAttribute
{
    public int Min { get; init; }
  public int Max { get; init; }

    public RangeAttribute(int min, int max)
    {
    Min = min;
        Max = max;
    }

    public override IValidator CreateValidator() 
        => new RangeValidator(Min, Max);
}
```

### Create Custom Validator

```csharp
public class StrongPasswordValidator : IValidator
{
    public ValidationResult Validate(object value, string propertyName)
    {
        if (value is not string password)
            return ValidationResult.Ok(propertyName);

if (password.Length < 8)
            return ValidationResult.Fail(propertyName, "Min 8 characters");

        if (!password.Any(char.IsUpper))
            return ValidationResult.Fail(propertyName, "Need uppercase");

  if (!password.Any(char.IsDigit))
          return ValidationResult.Fail(propertyName, "Need digit");

    return ValidationResult.Ok(propertyName);
    }
}
```

### Create Custom Extension Method

```csharp
public static class MyValidatorExtensions
{
    public static PropertyValidator<T, string> MustContainDomain<T>(
        this PropertyValidator<T, string> validator, 
        string domain)
    {
        return validator.Custom(
          email => email.Contains($"@{domain}"),
         $"Must be from {domain} domain"
   );
    }
}

// Usage
builder.For(u => u.Email).MustContainDomain("company.com");
```

### Create Custom Notifier

```csharp
public class CustomNotifier : IValidationNotifierSubscriber
{
    public void Notify(List<ValidationResult> results)
    {
  foreach (var result in results.Where(r => !r.IsValid))
     {
            // Your custom notification logic
      LogError(result);
            SendEmail(result);
            UpdateUI(result);
 }
    }
}
```

## ?? Requirements

- .NET 8.0 or higher
- .NET 9.0 supported
- C# 13.0 (for latest features)

## ?? Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch
3. Write tests for new functionality
4. Ensure all tests pass
5. Submit a pull request

## ?? License

This project is licensed under the MIT License - see the LICENSE file for details.

## ?? Acknowledgments

- Inspired by FluentValidation
- Built with modern .NET practices
- Designed for real-world applications

## ?? Support

- **Documentation**: See [DOCUMENTATION.md](ValidationFramework/DOCUMENTATION.md)
- **Quick Start**: See [QUICKSTART.md](ValidationFramework/Fluent/QUICKSTART.md)
- **Migration**: See [MIGRATION_GUIDE.md](ValidationFramework/Fluent/MIGRATION_GUIDE.md)
- **Examples**: Check demo applications in solution

## ??? Roadmap

### Version 2.0.0 (Current)
- ? Fluent Validation API
- ? Composite Validators
- ? Async Validation Support
- ? 20+ Extension Methods
- ? Complete Documentation

### Future Plans
- ?? More built-in validators (DateTime, GUID, etc.)
- ?? Localization support for error messages
- ?? Integration with popular DI containers
- ?? Performance optimizations
- ?? More UI framework notifiers

## ?? Version History

### 2.0.0 - Fluent API Release
- Added complete Fluent Validation API
- Added composite validators (AND, OR, NOT, WHEN)
- Added async validation support
- Added 20+ extension methods
- Added comprehensive documentation
- 100% backward compatible

### 1.0.0 - Initial Release
- Attribute-based validation
- ValidationEngine
- Notification system
- Basic validators (Required, Email, Length, Phone, Regex)
- Demo applications

---

**Made with ?? for the .NET Community**

**Happy Validating! ??**
