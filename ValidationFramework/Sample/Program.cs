using ValidationFramework.Attributes;
using ValidationFramework.Core;
using ValidationFramework.Notification;

namespace ValidationFramework.Sample
{
    public class User
    {
        [Required(ErrorMessage = "Username is required")] 
        [Length(3, 10, ErrorMessage = "Username must be 3-10 chars")] 
        public string Username { get; set; }

        [Email(ErrorMessage = "Email is invalid")] 
        public string Email { get; set; }
    }

    public static class Program
    {
        public static void Main()
        {
            var user = new User { Username = "", Email = "not-an-email" };
            var engine = new ValidationEngine();

            // Register notifiers via engine's publisher
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new MessageBoxNotifier());
            engine.Publisher.Subscribe(ValidationEventType.Invalid, new SummaryNotifier());

            // Validate (auto-notify inside)
            var results = engine.Validate(user);
        }
    }
}