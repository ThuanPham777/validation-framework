using ValidationFramework.Attributes;

namespace ValidationFramework.Demo.Models
{
    public class AttributeUser
    {
        [Required(ErrorMessage = "Username is required")]
        [Length(3, 20, ErrorMessage = "Username must be 3-20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Email(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
