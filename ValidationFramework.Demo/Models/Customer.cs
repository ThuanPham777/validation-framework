using ValidationFramework.Attributes;

namespace ValidationFramework.Demo.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "First name is required")]
        [Length(2, 50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Length(2, 50)]
        public string LastName { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
