using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.User
{
    [Table("UserProfile")]
    public class UserProfileModel : BaseEntity
    {
        [Required(ErrorMessage = "UserLoginId is required")]
        public int UserLoginId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; }

        [ForeignKey("UserLoginId")]
        public UserLoginModel UserLogin { get; set; }
    }
}
