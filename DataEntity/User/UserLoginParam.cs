using System.ComponentModel.DataAnnotations;

namespace DataEntity.User
{
    public class UserLoginParam
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;


    }
}
