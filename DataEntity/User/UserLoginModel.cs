﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.User
{
    [Table("UserLogin")]
    public class UserLoginModel : BaseEntity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }

    }
}
