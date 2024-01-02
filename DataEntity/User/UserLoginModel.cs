using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.User
{
    [Table("UserLogin")]
    public class UserLoginModel : ABaseModel
    {
        public required string Username { get; set; }

    }
}
