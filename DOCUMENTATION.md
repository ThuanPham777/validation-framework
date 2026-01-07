# ValidationFramework - Complete Documentation

## Table of Contents
1. [Introduction](#introduction)
2. [Architecture Overview](#architecture-overview)
3. [Getting Started](#getting-started)
4. [Validation Approaches](#validation-approaches)
5. [Core Components](#core-components)
6. [Attribute-Based Validation](#attribute-based-validation)
7. [Fluent Validation API](#fluent-validation-api)
8. [Validation Engine](#validation-engine)
9. [Notification System](#notification-system)
10. [Advanced Features](#advanced-features)
11. [Best Practices](#best-practices)
12. [API Reference](#api-reference)
13. [Examples & Demos](#examples--demos)

---

## Introduction

**ValidationFramework** is a comprehensive, extensible validation framework for .NET applications that supports multiple validation strategies and UI frameworks.

### Key Features
- ✅ **Multiple Validation Strategies** - Attribute-based, Fluent API, or both
- ✅ **Type-Safe** - Compile-time checking with fluent API
- ✅ **Extensible** - Easy to add custom validators and rules
- ✅ **UI Framework Agnostic** - Works with Console, WinForms, WinUI, WPF, etc.
- ✅ **Notification System** - Flexible notification publishing/subscription
- ✅ **Composable** - Combine validators with logical operators
- ✅ **Async Support** - Handle database and API validations
- ✅ **.NET 8 & .NET 9** - Supports modern .NET versions

### Supported Platforms
- Console Applications
- Windows Forms (WinForms)
- Windows UI Library 3 (WinUI 3)
- WPF (Windows Presentation Foundation)
- ASP.NET Core (with appropriate notifiers)

---

## Architecture Overview

```
ValidationFramework/
├── Attributes/              # Attribute-based validation
│   ├── ValidationAttribute.cs
│   ├── RequiredAttribute.cs
│   ├── EmailAttribute.cs
│   ├── LengthAttribute.cs
│   ├── PhoneAttribute.cs
│   ├── RegexAttribute.cs
│   └── CustomValidationAttribute.cs
├── Validator/      # Core validators
│   ├── IValidator.cs
│   ├── RequiredValidator.cs
│   ├── EmailValidator.cs
│   ├── LengthValidator.cs
│   ├── PhoneValidator.cs
│   ├── RegexValidator.cs
│   └── DelegateValidator.cs
├── Fluent/    # Fluent validation API
│   ├── IFluentValidator.cs
│   ├── ValidatorBuilder.cs
│   ├── PropertyValidator.cs
│ ├── AbstractValidator.cs
│   ├── Rules/
│   │   └── CommonRules.cs
│   ├── Composite/
│   │   └── CompositeValidators.cs
│   └── Async/
│       └── AsyncValidators.cs
├── Core/         # Core engine
│   └── ValidationEngine.cs
├── Result/        # Validation results
│   └── ValidationResult.cs
├── Notification/       # Notification system
│   ├── IValidationNotifierSubscriber.cs
│   ├── NotificationPublisher.cs
│   ├── ValidationEventType.cs
│   └── Notifiers.cs
├── Extensions/        # Extension methods
│   ├── ValidationEngineExtensions.cs
│   └── PropertyValidatorExtensions.cs
├── Group/   # Validator grouping
│   └── ValidatorGroup.cs
└── Factory/   # Validator factory
    └── ValidatorFactory.cs
```

---

## Getting Started

### Installation

Add reference to the ValidationFramework project in your solution.

### Quick Start - Attribute-Based Validation

```csharp
using ValidationFramework.Attributes;
using ValidationFramework.Core;

// 1. Define your model with validation attributes
public class User
{
    [Required(ErrorMessage = "Username is required")]
    [Length(3, 20, ErrorMessage = "Username must be 3-20 characters")]
    public string Username { get; set; }

    [Required]
    [Email(ErrorMessage = "Invalid email format")]
 public string Email { get; set; }
}

// 2. Create validation engine
var engine = new ValidationEngine();

// 3. Validate
var user = new User { Username = "jo", Email = "invalid-email" };
var results = engine.Validate(user);

// 4. Check results
foreach (var result in results.Where(r => !r.IsValid))
{
    Console.WriteLine($"{result.PropertyName}: {result.Message}");
}
```

### Quick Start - Fluent Validation

```csharp
using ValidationFramework.Fluent;
using ValidationFramework.Extensions;

// 1. Create a validator using fluent API
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
    }
}

// 2. Validate
var validator = new UserValidator();
var results = validator.Validate(user);
```

---

## Validation Approaches

ValidationFramework supports three validation approaches that can be used independently or together:

### 1. Attribute-Based Validation

**Best for:** Simple, declarative validation rules on model properties.

```csharp
public class Product
{
    [Required]
    [Length(3, 100)]
    public string Name { get; set; }

    [Required]
    [Regex(@"^[A-Z]{3}\d{3}$")]
    public string SKU { get; set; }
}
```

**Pros:**
- ✅ Declarative and easy to read
- ✅ Validation rules visible on the model
- ✅ Quick to implement

**Cons:**
- ❌ Limited flexibility for complex rules
- ❌ Hard to unit test in isolation
- ❌ No compile-time type checking

### 2. Fluent Validation API

**Best for:** Complex validation logic, reusable validators, type-safe rules.

```csharp
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .Required()
 .Length(3, 100);

        RuleFor(p => p.SKU)
            .Required()
          .Regex(@"^[A-Z]{3}\d{3}$");
    }
}
```

**Pros:**
- ✅ Type-safe with IntelliSense
- ✅ Highly flexible and composable
- ✅ Easy to unit test
- ✅ Reusable validator classes

**Cons:**
- ❌ More verbose than attributes
- ❌ Requires separate validator classes

### 3. Hybrid Approach (Recommended)

**Best for:** Most real-world applications - simple rules as attributes, complex rules as fluent.

```csharp
// Simple validations as attributes
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

// Complex validations with fluent API
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

---

## Core Components

### 1. IValidator Interface

The foundation of all validators:

```csharp
public interface IValidator
{
    ValidationResult Validate(object value, string propertyName);
}
```

### 2. ValidationResult

Represents the result of a validation:

```csharp
public class ValidationResult
{
    public bool IsValid { get; }
    public string PropertyName { get; }
    public string Message { get; }
    public object? AttemptedValue { get; }
    public string? ErrorCode { get; }

    public static ValidationResult Ok(string propertyName);
  public static ValidationResult Fail(string propertyName, string message, 
        object? attemptedValue = null, string? errorCode = null);
}
```

### 3. ValidationEngine

The central orchestrator for validation:

```csharp
public class ValidationEngine
{
// Add custom validator for a property
    public void AddValidator(string property, IValidator validator);
    
// Validate an object
    public List<ValidationResult> Validate(object model);
    
    // Access notification publisher
  public NotificationPublisher Publisher { get; }
}
```

### 4. ValidatorGroup

Groups multiple validators for a single property:

```csharp
var group = new ValidatorGroup();
group.Add(new RequiredValidator());
group.Add(new LengthValidator(3, 20));
group.Add(new AlphaNumericValidator());

engine.AddValidator("Username", group);
```

---

## Attribute-Based Validation

### Available Attributes

#### Required Attribute
```csharp
[Required(ErrorMessage = "Field is required")]
public string Username { get; set; }
```

#### Length Attribute
```csharp
[Length(3, 20, ErrorMessage = "Must be 3-20 characters")]
public string Username { get; set; }
```

#### Email Attribute
```csharp
[Email(ErrorMessage = "Invalid email format")]
public string Email { get; set; }
```

#### Phone Attribute
```csharp
[Phone(ErrorMessage = "Invalid phone number")]
public string PhoneNumber { get; set; }
```

#### Regex Attribute
```csharp
[Regex(@"^[A-Z]{3}\d{3}$", ErrorMessage = "Invalid format")]
public string SKU { get; set; }
```

#### Custom Validation Attribute
```csharp
// Define custom validator
public class StrongPasswordValidator : IValidator
{
    public ValidationResult Validate(object value, string propertyName)
    {
        // Validation logic
 }
}

// Use on property
[CustomValidation(typeof(StrongPasswordValidator))]
public string Password { get; set; }
```

### Custom Attributes

Create your own validation attributes:

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

---

## Fluent Validation API

### ValidatorBuilder<T>

Create validators using the builder pattern:

```csharp
var builder = new ValidatorBuilder<User>();

builder.For(u => u.Username)
    .Required()
    .Length(3, 20)
    .AlphaNumeric();

builder.For(u => u.Email)
    .Required()
    .Email()
    .EmailDomain("company.com");

var validator = builder.Build();
var results = validator.Validate(user);
```

### AbstractValidator<T>

Create reusable validator classes:

```csharp
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username)
       .Required()
       .Length(3, 20)
            .NoSpecialChars();

        RuleFor(u => u.Email)
        .Required()
            .Email()
            .EmailDomain("gmail.com", "outlook.com");

      RuleFor(u => u.Age)
    .Range(18, 120);
    }
}
```

### Built-in Fluent Validators

#### String Validators
```csharp
builder.For(u => u.Username)
    .Required()       // Not null or empty
    .NotEmpty()      // Not whitespace
    .Length(3, 20)          // Length range
    .MinLength(3)     // Minimum length
    .MaxLength(20)          // Maximum length
    .Email()             // Valid email
.Phone()    // Valid phone
    .Regex(@"pattern")   // Regex match
    .AlphaOnly()      // Only letters
    .AlphaNumeric()         // Letters & digits
    .NoSpecialChars()       // No special chars
    .NoDigits()      // No digits
    .StartsWith("prefix")   // Starts with
    .EndsWith("suffix")     // Ends with
    .Contains("text")       // Contains
    .Url()    // Valid URL
  .CreditCard()     // Valid credit card
    .EmailDomain("gmail.com"); // Email domain
```

#### Numeric Validators
```csharp
builder.For(u => u.Age)
    .Range(18, 100)         // Between values
    .GreaterThan(0)         // Greater than
    .LessThan(150)    // Less than
    .Equal(25)   // Equals
    .NotEqual(0);           // Not equals
```

#### Custom Validators
```csharp
// Simple predicate
builder.For(u => u.Password)
.Custom(p => p.Any(char.IsUpper), "Must contain uppercase");

// Full control
builder.For(u => u.Password)
    .Custom((password, propertyName) =>
    {
        if (string.IsNullOrEmpty(password))
   return ValidationResult.Fail(propertyName, "Required");
  return ValidationResult.Ok(propertyName);
    });
```

### Custom Error Messages

```csharp
builder.For(u => u.Username)
    .Required()
    .WithMessage("Please enter a username")
    .Length(3, 20)
    .WithMessage("Username must be between 3 and 20 characters");
```

---

## Validation Engine

### Basic Usage

```csharp
var engine = new ValidationEngine();

// Validate model with attributes
var results = engine.Validate(user);

// Add custom validators
engine.AddValidator("Username", new CustomValidator());

// Validate again
results = engine.Validate(user);
```

### Adding Custom Validators

```csharp
// Single validator
engine.AddValidator("Email", new EmailValidator());

// Multiple validators using ValidatorGroup
var group = new ValidatorGroup();
group.Add(new RequiredValidator());
group.Add(new LengthValidator(3, 20));
group.Add(new AlphaNumericValidator());
engine.AddValidator("Username", group);

// Delegate validator
engine.AddValidator("Password", new DelegateValidator((value, propertyName) =>
{
    if (value is string s && s.Length >= 8)
 return ValidationResult.Ok(propertyName);
    return ValidationResult.Fail(propertyName, "Password must be at least 8 characters");
}));
```

### Integration with Fluent Validators

```csharp
using ValidationFramework.Extensions;

var engine = new ValidationEngine();

// Option 1: Register validator instance
var userValidator = new UserValidator();
engine.AddFluentValidator(userValidator);

// Option 2: Configure inline
engine.AddFluentValidator<User>(builder =>
{
    builder.For(u => u.Username).Required().Length(3, 20);
    builder.For(u => u.Email).Required().Email();
});

// Validate using both attribute and fluent rules
var results = engine.ValidateWithFluent(user);
```

---

## Notification System

The notification system allows you to react to validation events and notify users through various channels.

### Architecture

```
ValidationEngine
    └─> NotificationPublisher
     └─> Subscribers (Notifiers)
  ├─> MessageBoxNotifier
        ├─> TooltipNotifier
            ├─> LabelNotifier
         └─> Custom Notifiers
```

### Validation Event Types

```csharp
public enum ValidationEventType
{
    Validated,  // Validation completed (may have errors)
    Invalid     // Validation found errors
}
```

### Built-in Notifiers

```csharp
using ValidationFramework.Notification;

var engine = new ValidationEngine();

// Subscribe to invalid events
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
 new MessageBoxNotifier());

engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new TooltipNotifier());

engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new LabelNotifier());

engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new SummaryNotifier());

// Validation automatically triggers notifications
var results = engine.Validate(user);
```

### Creating Custom Notifiers

```csharp
public class CustomNotifier : IValidationNotifierSubscriber
{
    public void Notify(List<ValidationResult> results)
    {
    foreach (var result in results.Where(r => !r.IsValid))
   {
            // Custom notification logic
            Console.WriteLine($"Error: {result.PropertyName} - {result.Message}");
        }
    }
}

// Usage
engine.Publisher.Subscribe(ValidationEventType.Invalid, new CustomNotifier());
```

### UI Framework-Specific Notifiers

#### Windows Forms Notifiers

```csharp
using ValidationFramework.Demo.Winforms.Notifiers;

// MessageBox notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new WinFormsMessageBoxNotifier());

// Label notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new WinFormsLabelNotifier(lblError));

// TextBox highlight notifier
var textBoxes = new Dictionary<string, TextBox>
{
    { "Username", txtUsername },
    { "Email", txtEmail }
};
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new TextBoxHighlightNotifier(textBoxes));

// ErrorProvider notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new ErrorProviderNotifier(errorProvider, controls));
```

#### WinUI Notifiers

```csharp
using ValidationFramework.Demo.WinUI.Notifiers;

// ContentDialog notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new ContentDialogNotifier(this.Content.XamlRoot));

// TextBlock notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new TextBlockNotifier(txtValidationSummary));

// TextBox highlight notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new TextBoxHighlightNotifier(textBoxes, errorTextBlocks));

// InfoBar notifier
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new InfoBarNotifier(infoBar));
```

---

## Advanced Features

### 1. Composite Validators

Combine validators using logical operators:

```csharp
using ValidationFramework.Fluent.Composite;

// AND - all must pass
var andValidator = new AndValidator(
    new RequiredValidator(),
    new LengthValidator(3, 20)
);

// OR - at least one must pass
var orValidator = new OrValidator(
    new EmailValidator(),
    new PhoneValidator()
);

// NOT - invert result
var notValidator = new NotValidator(
    new RegexValidator(@"\d"),
    "Must not contain digits"
);

// WHEN - conditional
var whenValidator = new WhenValidator(
    value => value != null,
    new EmailValidator()
);

// Extension methods
var validator = new RequiredValidator()
    .And(new LengthValidator(3, 20))
    .Or(new EmailValidator());
```

### 2. Async Validation

For database checks and API calls:

```csharp
using ValidationFramework.Fluent.Async;

// Built-in async validators
var uniqueUsername = AsyncValidators.UniqueUsernameValidator();
var uniqueEmail = AsyncValidators.UniqueEmailValidator();

// Custom async validator
var customAsync = AsyncValidators.CustomAsync<string>(
    async username =>
 {
        await Task.Delay(100); // Simulate DB call
        return !existingUsernames.Contains(username);
 },
    "Username is already taken"
);

// Usage
var result = await uniqueUsername.ValidateAsync("john", "Username");
```

### 3. Reusable Rules

Create and reuse validation rules:

```csharp
using ValidationFramework.Fluent.Rules;

// Use predefined rules
var notEmpty = CommonRules.NotEmptyRule();
var minLength = CommonRules.MinLengthRule(3);
var alphaOnly = CommonRules.AlphaOnlyRule();

// Create custom reusable rules
public static class MyRules
{
    public static IValidator StrongPasswordRule()
    {
        return new ChainValidator(
    CommonRules.MinLengthRule(8),
   CommonRules.PredicateRule<string>(
      p => p.Any(char.IsUpper),
 "Must contain uppercase"
            ),
            CommonRules.PredicateRule<string>(
          p => p.Any(char.IsDigit),
     "Must contain digit"
            )
        );
    }
}
```

### 4. Validator Inheritance

```csharp
// Base validator
public abstract class PersonValidator<T> : AbstractValidator<T>
    where T : Person
{
    protected PersonValidator()
    {
        RuleFor(p => p.FirstName)
    .Required()
 .AlphaOnly();

     RuleFor(p => p.LastName)
            .Required()
      .AlphaOnly();
    }
}

// Derived validator
public class EmployeeValidator : PersonValidator<Employee>
{
    public EmployeeValidator()
    {
        // Inherits FirstName and LastName rules
        
        RuleFor(e => e.EmployeeId)
      .Required()
            .Regex(@"^EMP\d{5}$");
    }
}
```

### 5. DelegateValidator

Quick inline validators:

```csharp
var validator = new DelegateValidator((value, propertyName) =>
{
    if (value is string s && s.Length > 0 && char.IsLetter(s[0]))
  return ValidationResult.Ok(propertyName);
    return ValidationResult.Fail(propertyName, "Must start with a letter");
});

engine.AddValidator("Username", validator);
```

---

## Best Practices

### 1. Choose the Right Approach

**Use Attributes for:**
- ✅ Simple validations (Required, Email, Length)
- ✅ Quick prototyping
- ✅ When rules are unlikely to change

**Use Fluent API for:**
- ✅ Complex validation logic
- ✅ Multiple conditions per property
- ✅ Cross-property validation
- ✅ Reusable validator classes
- ✅ Unit testable validators

**Use Hybrid for:**
- ✅ Best of both worlds
- ✅ Simple rules as attributes
- ✅ Complex rules as fluent

### 2. Separation of Concerns

```csharp
// ✅ Good - Validation logic separate
public class UserValidator : AbstractValidator<User>
{
// Validation rules here
}

public class UserService
{
    private readonly IFluentValidator<User> _validator;
    
    public void CreateUser(User user)
    {
        var results = _validator.Validate(user);
        if (results.Any(r => !r.IsValid))
            throw new ValidationException(results);
      
        // Business logic here
    }
}

// ❌ Avoid - Mixing validation with business logic
public class UserService
{
public void CreateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Username))
   throw new Exception("Invalid");
    
     // Business logic
    }
}
```

### 3. Use Meaningful Error Messages

```csharp
// ✅ Good
builder.For(u => u.Password)
    .MinLength(8)
    .WithMessage("Password must be at least 8 characters for security");

// ❌ Avoid
builder.For(u => u.Password)
    .MinLength(8)
    .WithMessage("Invalid");
```

### 4. Group Related Validators

```csharp
// ✅ Good
var usernameValidators = new ValidatorGroup();
usernameValidators.Add(new RequiredValidator());
usernameValidators.Add(new LengthValidator(3, 20));
usernameValidators.Add(new AlphaNumericValidator());
engine.AddValidator("Username", usernameValidators);

// ❌ Avoid
engine.AddValidator("Username", new RequiredValidator());
engine.AddValidator("Username", new LengthValidator(3, 20));
engine.AddValidator("Username", new AlphaNumericValidator());
```

### 5. Use NotificationPublisher for UI Updates

```csharp
// ✅ Good - Centralized notification
var engine = new ValidationEngine();
engine.Publisher.Subscribe(ValidationEventType.Invalid, 
    new TextBoxHighlightNotifier(textBoxes));

var results = engine.Validate(user);
// Notifiers automatically triggered

// ❌ Avoid - Manual UI updates everywhere
var results = engine.Validate(user);
foreach (var error in results.Where(r => !r.IsValid))
{
    // Manually update UI
}
```

### 6. Test Your Validators

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

---

## API Reference

### Core Interfaces

#### IValidator
```csharp
public interface IValidator
{
    ValidationResult Validate(object value, string propertyName);
}
```

#### IFluentValidator<T>
```csharp
public interface IFluentValidator<T>
{
    List<ValidationResult> Validate(T instance);
}
```

#### IValidationNotifierSubscriber
```csharp
public interface IValidationNotifierSubscriber
{
    void Notify(List<ValidationResult> results);
}
```

### ValidationEngine

| Method | Description |
|--------|-------------|
| `AddValidator(string property, IValidator validator)` | Add custom validator for property |
| `Validate(object model)` | Validate object using attributes and custom validators |
| `Publisher` | Get NotificationPublisher instance |

### ValidationEngineExtensions

| Method | Description |
|--------|-------------|
| `AddFluentValidator<T>(IFluentValidator<T>)` | Register fluent validator |
| `AddFluentValidator<T>(Action<ValidatorBuilder<T>>)` | Configure fluent validator |
| `ValidateWithFluent<T>(T)` | Validate with both attribute and fluent rules |

### ValidatorBuilder<T>

| Method | Description |
|--------|-------------|
| `For<TProperty>(expression)` | Specify property to validate |
| `Build()` | Build the validator |

### PropertyValidator<T, TProperty>

| Method | Description |
|--------|-------------|
| `Required()` | Value must not be null/empty |
| `Email()` | Valid email format |
| `Phone()` | Valid phone number |
| `Length(min, max)` | String length range |
| `Regex(pattern)` | Regex pattern match |
| `Custom(predicate, message)` | Custom validation |
| `WithMessage(message)` | Override error message |

### PropertyValidatorExtensions

**String Validators:**
- `NotEmpty()`, `MinLength(int)`, `MaxLength(int)`
- `AlphaOnly()`, `AlphaNumeric()`, `NoSpecialChars()`, `NoDigits()`
- `StartsWith(string)`, `EndsWith(string)`, `Contains(string)`
- `EmailDomain(params string[])`, `Url()`, `CreditCard()`

**Comparison Validators:**
- `Equal(value)`, `NotEqual(value)`
- `GreaterThan(value)`, `LessThan(value)`, `Range(min, max)`

### Composite Validators

| Class | Description |
|-------|-------------|
| `AndValidator` | All validators must pass |
| `OrValidator` | At least one must pass |
| `NotValidator` | Invert validator result |
| `WhenValidator` | Conditional validation |
| `UnlessValidator` | Unless condition |
| `ChainValidator` | Collect all errors |

---

## Examples & Demos

### Console Application

See `ValidationFramework.Demo\Program.cs` for a complete console application example demonstrating:
- Attribute-based validation
- Fluent validation
- Custom validators
- Notification system

### Windows Forms Application

See `ValidationFramework.Demo.Winforms\` for a WinForms example with:
- User registration form
- Real-time validation
- Multiple notifiers (MessageBox, Label, TextBox highlighting, ErrorProvider)
- Custom validators

### WinUI 3 Application

See `ValidationFramework.Demo.WinUI\` for a WinUI 3 example with:
- Modern UI validation
- ContentDialog, InfoBar, TextBlock notifiers
- TextBox highlighting
- Fluent and attribute validation

### Sample Code

See `ValidationFramework\Sample\FluentValidationSample.cs` for 5 comprehensive examples:
1. ValidatorBuilder usage
2. AbstractValidator usage
3. Engine integration
4. Custom validation rules
5. Chaining multiple rules

### Unit Tests

See `ValidationFramework\Tests\FluentValidationTests.cs` for test examples.

---

## Support & Contributing

### Target Frameworks
- .NET 8.0
- .NET 9.0

### License
Part of ValidationFramework project.

### Version
2.0.0 - Fluent API Release

---

**Happy Validating! 🎉**
