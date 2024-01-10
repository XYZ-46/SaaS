using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.User
{
    [Table("UserProfile")]
    public class UserProfileModel : BaseEntity
    {
        [ForeignKey("UserLogin")]
        [Required(ErrorMessage = "UserLoginId is required")]
        public int UserLoginId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; }

        public UserLoginModel UserLogin { get; set; }
    }
}
