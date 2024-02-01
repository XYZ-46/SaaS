using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.Model
{
    [Table("UserLogin")]
    public record UserLoginModel : BaseEntity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }

    }
}
