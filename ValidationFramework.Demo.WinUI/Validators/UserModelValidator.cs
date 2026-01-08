using System.Linq;
using ValidationFramework.Demo.WinUI.Models;
using ValidationFramework.Extensions;
using ValidationFramework.Fluent;

namespace ValidationFramework.Demo.WinUI.Validators
{
    /// <summary>
    /// Fluent validator for UserModel with complex validation rules
    /// </summary>
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            // Username: alphanumeric, no special characters
            RuleFor(u => u.Username)
                .Required()
                .Length(3, 20)
                .AlphaNumeric()
                .NoSpecialChars()
                .WithMessage(
                    "Username must be 3-20 alphanumeric characters without special characters"
                );

            // Email: must be from specific domains
            RuleFor(u => u.Email)
                .Required()
                .Email()
                .EmailDomain("gmail.com", "outlook.com", "yahoo.com")
                .WithMessage(
                    "Email must be from gmail.com, outlook.com, or yahoo.com"
                );

            // Phone: valid phone format
            RuleFor(u => u.Phone)
                .Required()
                .Phone()
                .WithMessage("Phone must be a valid phone number");

            // Password: strong password requirements
            RuleFor(u => u.Password)
                .Required()
                .MinLength(8)
                .Custom(
                    p => p.Any(char.IsUpper),
                    "Password must contain at least one uppercase letter"
                )
                .Custom(
                    p => p.Any(char.IsLower),
                    "Password must contain at least one lowercase letter"
                )
                .Custom(
                    p => p.Any(char.IsDigit),
                    "Password must contain at least one digit"
                )
                .WithMessage(
                    "Password must be at least 8 characters with uppercase, lowercase, and digit"
                );

            // ConfirmPassword: required
            RuleFor(u => u.ConfirmPassword)
                .Required()
                .WithMessage("Confirm Password is required");
        }
    }
}
