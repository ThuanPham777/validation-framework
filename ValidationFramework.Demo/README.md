# ValidationFramework.Demo - Console Application

A comprehensive console application demonstrating all features of the ValidationFramework with integrated notification system.

## ?? What This Demo Shows

This console application demonstrates:

1. ? **Attribute-Based Validation** - Using validation attributes with notifications
2. ? **Fluent Validation API** - Type-safe, chainable validation rules with notifications
3. ? **Hybrid Approach** - Combining attributes with fluent validation
4. ? **Custom Validators** - Creating and using custom validation logic
5. ? **Validator Groups** - Grouping multiple validators
6. ? **Delegate Validators** - Quick inline validation rules
7. ? **Extension Methods** - Using built-in extension methods

**Note:** Every demo integrates the notification system to show real-time validation feedback with color-coded console output.

## ?? How to Run

### Using .NET CLI
```bash
cd ValidationFramework.Demo
dotnet run
```

### Using Visual Studio
1. Set `ValidationFramework.Demo` as the startup project
2. Press F5 or click "Start"

## ?? Project Structure

```
ValidationFramework.Demo/
??? Program.cs              # Main entry point (30 lines)
??? README.md          # This file
??? REFACTORING_SUMMARY.md# Refactoring details
?
??? Models/  # Demo models (6 files)
?   ??? AttributeUser.cs        # Demo 1 model (with attributes)
? ??? Product.cs        # Demo 2 model
?   ??? Customer.cs   # Demo 3 model (hybrid)
?   ??? NotificationUser.cs  # For notification examples
?   ??? DelegateUser.cs       # Demo 6 model
?   ??? ExtendedUser.cs    # Demo 7 model
?
??? Validators/         # Validator implementations
?   ??? ProductValidator.cs       # Fluent validator for Product
?   ??? CustomValidators.cs    # 3 custom validators
?       ??? NoDigitValidator
?       ??? NoSpecialCharValidator
?       ??? AlphaOnlyValidator
?
??? Notifiers/        # Notification implementations
?   ??? ConsoleNotifiers.cs       # 3 custom notifiers
?    ??? ColoredConsoleNotifier
?       ??? DetailedNotifier
?       ??? SuccessNotifier
?
??? Demos/       # Demo implementations (8 files)
    ??? DemoHelpers.cs         # Helper methods for output
    ??? Demo1_AttributeBased.cs   # Attribute validation + notifications
    ??? Demo2_FluentValidation.cs # Fluent API + notifications
  ??? Demo3_HybridApproach.cs   # Hybrid approach + detailed notifications
    ??? Demo4_CustomValidators.cs # Custom validators
    ??? Demo5_ValidatorGroups.cs  # Validator groups
    ??? Demo6_DelegateValidators.cs # Delegate validators + notifications
    ??? Demo7_ExtensionMethods.cs # Extension methods + notifications
```

## ?? Demo Breakdown

### Demo 1: Attribute-Based Validation ?
**File:** `Demos/Demo1_AttributeBased.cs`
**Model:** `Models/AttributeUser.cs`
**Notifications:** ColoredConsoleNotifier (Red), SuccessNotifier

**Shows:** How to use validation attributes like `[Required]`, `[Email]`, `[Length]`, etc.

**What It Demonstrates:**
- Declarative validation using attributes
- Built-in validators (Required, Email, Phone, Length)
- Custom error messages
- ValidationEngine usage
- **Integrated notifications with color-coded errors**

---

### Demo 2: Fluent Validation API ?
**File:** `Demos/Demo2_FluentValidation.cs`
**Model:** `Models/Product.cs`
**Validator:** `Validators/ProductValidator.cs`
**Notifications:** ColoredConsoleNotifier (Yellow), SuccessNotifier

**Shows:** Type-safe validation using the fluent API with `AbstractValidator<T>`

**What It Demonstrates:**
- Fluent validation syntax
- AbstractValidator base class
- RuleFor method
- Compile-time type safety
- Custom error messages with WithMessage()
- **Real-time notifications on validation events**

---

### Demo 3: Hybrid Approach ?
**File:** `Demos/Demo3_HybridApproach.cs`
**Model:** `Models/Customer.cs`
**Notifications:** DetailedNotifier, SuccessNotifier

**Shows:** Combining attribute-based and fluent validation

**What It Demonstrates:**
- Best of both worlds
- Simple validations as attributes
- Complex validations with fluent API
- ValidateWithFluent() method
- **Detailed error reporting with property, message, value, and error code**

---

### Demo 4: Custom Validators ?
**File:** `Demos/Demo4_CustomValidators.cs`
**Validators:** `Validators/CustomValidators.cs`
**Notifications:** ColoredConsoleNotifier (Magenta)

**Custom Validators:**
- `NoDigitValidator` - No digits allowed
- `NoSpecialCharValidator` - No special characters
- `AlphaOnlyValidator` - Only letters allowed

**What It Demonstrates:**
- IValidator interface
- Custom validation logic
- ValidationResult.Ok() and ValidationResult.Fail()
- Error codes
- **Color-coded custom validator feedback**

---

### Demo 5: Validator Groups ?
**File:** `Demos/Demo5_ValidatorGroups.cs`

**Shows:** Grouping multiple validators for a single property

**What It Demonstrates:**
- ValidatorGroup class
- Adding multiple validators
- Sequential validation (stops at first failure)
- Organized validation logic
- **Color-coded pass/fail feedback**

---

### Demo 6: Delegate Validators ?
**File:** `Demos/Demo6_DelegateValidators.cs`
**Model:** `Models/DelegateUser.cs`
**Notifications:** ColoredConsoleNotifier (Cyan), SuccessNotifier

**Shows:** Quick inline validators using delegates

**What It Demonstrates:**
- DelegateValidator class
- Inline validation logic
- Quick custom rules without creating classes
- Lambda expressions for validation
- **Integrated notifications for delegate validators**

---

### Demo 7: Extension Methods ?
**File:** `Demos/Demo7_ExtensionMethods.cs`
**Model:** `Models/ExtendedUser.cs`
**Notifications:** ColoredConsoleNotifier (DarkYellow), SuccessNotifier

**Shows:** Using built-in extension methods for common validations

**Extension Methods Used:**
- `AlphaNumeric()` - Alphanumeric characters only
- `EmailDomain()` - Validate email domains
- `Url()` - Valid URL format
- `Range()` - Numeric range validation

**What It Demonstrates:**
- Built-in extension methods (20+)
- String validators
- Comparison validators
- Method chaining
- Clean, readable syntax
- **Real-time feedback with notifications**

---

## ?? Console Output Features

The demo includes:
- ? **Color-coded output** - Different colors for each demo
  - Red - Attribute validation errors
  - Yellow - Fluent validation errors
  - Magenta - Custom validator errors
  - Cyan - Delegate validator errors
  - DarkYellow - Extension method errors
  - Green - Success messages
- ? **Section headers** - Clear separation between demos
- ? **Detailed error messages** - Property name, message, attempted value, error code
- ? **Before/After comparisons** - Invalid data ? Valid data
- ? **Integrated notifications** - Real-time validation feedback

## ?? Notification System Integration

Each demo showcases the notification system:

| Demo | Notifiers Used | Purpose |
|------|---------------|---------|
| Demo 1 | ColoredConsoleNotifier (Red), SuccessNotifier | Show attribute validation errors |
| Demo 2 | ColoredConsoleNotifier (Yellow), SuccessNotifier | Show fluent validation errors |
| Demo 3 | DetailedNotifier, SuccessNotifier | Show detailed error information |
| Demo 4 | ColoredConsoleNotifier (Magenta) | Show custom validator errors |
| Demo 5 | - | Manual color-coded output |
| Demo 6 | ColoredConsoleNotifier (Cyan), SuccessNotifier | Show delegate validator errors |
| Demo 7 | ColoredConsoleNotifier (DarkYellow), SuccessNotifier | Show extension method errors |

**Key Takeaway:** Notifications are not a separate feature but an integral part of every validation approach!

## ?? Key Takeaways

After running this demo, you'll understand:

1. **When to use attributes** - Simple, declarative validation
2. **When to use fluent API** - Complex, type-safe validation
3. **When to use hybrid** - Best of both worlds (recommended)
4. **How to create custom validators** - Implement IValidator
5. **How notifications work** - Integrated with every validation
6. **How to group validators** - Organize multiple rules
7. **How to use delegates** - Quick inline rules
8. **How to use extensions** - Built-in common validators

## ?? Learning Path

1. **Start with Demo 1** - Understand attribute-based validation + notifications
2. **Move to Demo 2** - Learn fluent validation API + notifications
3. **Try Demo 3** - See hybrid approach + detailed notifications
4. **Explore Demo 4** - Create custom validators
5. **Review Demo 5** - Learn validator groups
6. **See Demo 6** - Use delegate validators + notifications
7. **Finish with Demo 7** - Master extension methods + notifications

## ?? Related Documentation

- **Main Documentation**: [../ValidationFramework/DOCUMENTATION.md](../ValidationFramework/DOCUMENTATION.md)
- **Quick Start**: [../ValidationFramework/Fluent/QUICKSTART.md](../ValidationFramework/Fluent/QUICKSTART.md)
- **Fluent API Guide**: [../ValidationFramework/Fluent/README.md](../ValidationFramework/Fluent/README.md)

## ?? Next Steps

After understanding this demo:
1. Try modifying the validation rules
2. Create your own custom validators
3. Implement your own notifiers
4. Experiment with different notification colors
5. Use ValidationFramework in your own projects
6. Check out the WinForms and WinUI demos for UI integration

---

**Happy Learning! ??**
