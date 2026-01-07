using System;
using ValidationFramework.Core;
using ValidationFramework.Fluent;
using ValidationFramework.Notification;

namespace ValidationFramework.Demo
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    class Program
    {
        static void Main()
        {
            var engine = new ValidationEngine();

            // Register fluent validator using builder
            engine.AddFluentValidator<User>(b =>
            {
                b.For(u => u.Username).Required().Length(3,10).WithMessage("Username must be3-10 chars");
                b.For(u => u.Email).Required().Email().WithMessage("Email is invalid");
                b.For(u => u.Phone).Phone().WithMessage("Phone is invalid");
            });

            engine.Publisher.Subscribe(ValidationEventType.Invalid, new SummaryNotifier());

            var user = new User { Username = "ab1", Email = "not-an-email", Phone = "123" };
            var results = engine.Validate(user);

            Console.WriteLine($"Found {results.Count} error(s) in demo.");
        }
    }
}
