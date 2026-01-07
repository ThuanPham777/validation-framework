using System;
using ValidationFramework.Core;
using ValidationFramework.Fluent;
using ValidationFramework.Result;
using ValidationFramework.Notification;

namespace ValidationFramework.Sample
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public static class Program
    {
        public static void Main()
        {
            var user = new User { Username = "", Email = "not-an-email" };
            var engine = new ValidationEngine();

            // Register a fluent validator via builder
            engine.AddFluentValidator<User>(b =>
            {
                b.For(u => u.Username).Required().Length(3,10).WithMessage("Username must be3-10 chars");
                b.For(u => u.Email).Required().Email().WithMessage("Email is invalid");
            });

            engine.Publisher.Subscribe(ValidationEventType.Invalid, new SummaryNotifier());

            var results = engine.Validate(user);
            Console.WriteLine($"Errors: {results.Count}");
        }
    }
}