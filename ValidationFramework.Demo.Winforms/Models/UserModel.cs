namespace ValidationFramework.Demo.Winforms.Models
{
    // Model class for demo (Fluent-only validation)
    public class UserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
