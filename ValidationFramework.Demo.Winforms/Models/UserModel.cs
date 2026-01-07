using ValidationFramework.Attributes;

namespace ValidationFramework.Demo.Winforms.Models
{
    // Model class with validation attributes for demo
    public class UserModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Length(3, 20, ErrorMessage = "Username must be 3-20 characters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [Email(ErrorMessage = "Email format is invalid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Phone format is invalid")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [Length(6, 50, ErrorMessage = "Password must be 6-50 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
