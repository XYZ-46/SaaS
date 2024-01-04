using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.User
{
    [Table("UserProfile")]
    public class UserProfileModel
    {
        public int UserLoginId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;

    }
}
