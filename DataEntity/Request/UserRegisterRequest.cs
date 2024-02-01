using System.ComponentModel.DataAnnotations;

namespace DataEntity.Request
{
    public record UserRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Fullname is required")]
        public string? Fullname { get; set; }

    }
}
