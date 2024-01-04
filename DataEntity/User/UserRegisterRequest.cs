using System.ComponentModel.DataAnnotations;

namespace DataEntity.User
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public static string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; } = string.Empty;

        public static UserLoginModel MapToUserLoginModel()
        {
            return new UserLoginModel()
            {
                Username = UserRegisterRequest.Username,
                PasswordHash = string.Empty,
            };
        }
    }
}
