using ValidationFramework.Attributes;

namespace ValidationFramework.Demo.Models
{
    public class NotificationUser
    {
        [Required]
        [Length(3, 20)]
        public string Username { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }
    }
}
