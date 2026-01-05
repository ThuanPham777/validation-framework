
using System;
using ValidationFramework.Attributes;
using ValidationFramework.Core;
using ValidationFramework.Notification;
using System.Collections.Generic;
using ValidationFramework.Demo;

namespace ValidationFramework.Demo
{
	// Model with attribute validation
	public class User
	{
		[Required(ErrorMessage = "Username is required")] 
		[Length(3, 10, ErrorMessage = "Username must be 3-10 chars")] 
		public string Username { get; set; }

		[Email(ErrorMessage = "Email is invalid")] 
		public string Email { get; set; }
	}

	// Username does not contain digits
	public class NoDigitValidator : IValidator
	{
		public ValidationResult Validate(object value, string propertyName)
		{
			if (value is string s && System.Text.RegularExpressions.Regex.IsMatch(s, @"\d"))
				return ValidationResult.Fail(propertyName, $"{propertyName} must not contain digits.");
			return ValidationResult.Ok(propertyName);
		}
	}

	// Custom notifier in console with red color
	public class RedConsoleNotifier : IValidationNotifierSubscriber
	{
		public void Notify(List<ValidationResult> results)
		{
			foreach (var result in results)
			{
				if (!result.IsValid)
				{
					var old = Console.ForegroundColor;
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"[RedNotifier] {result.PropertyName}: {result.Message}");
					Console.ForegroundColor = old;
				}
			}
		}
	}

	class Program
	{
		static void Main()
		{
			var user = new User { Username = "ab1", Email = "not-an-email" };
			var engine = new ValidationEngine();

			// 4. Add custom validator and group validator for Username
			var group = new ValidatorGroup();
			group.Add(new NoDigitValidator());
			group.Add(new AlphaOnlyValidator());
			group.Add(new NoSpecialCharValidator());
			group.Add(new LengthValidator(5, 8)); // override length
			engine.AddValidator(nameof(User.Username), group);

			// Validate
			var results = engine.Validate(user);

			// Register notifiers
			var publisher = new NotificationPublisher();
			publisher.Subscribe(ValidationEventType.Invalid, new MessageBoxNotifier());
			publisher.Subscribe(ValidationEventType.Invalid, new SummaryNotifier());
			publisher.Subscribe(ValidationEventType.Invalid, new RedConsoleNotifier());
			publisher.Notify(ValidationEventType.Invalid, results);

			// Simple to apply: just use attribute or add validator
			Console.WriteLine("\n==> Change Username to 'abc' and Email to valid to see pass");
			user.Username = "abc";
			user.Email = "abc@gmail.com";
			results = engine.Validate(user);
			publisher.Notify(ValidationEventType.Invalid, results);
		}
	}
}
