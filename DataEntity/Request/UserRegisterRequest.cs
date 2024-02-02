using System.ComponentModel.DataAnnotations;

namespace DataEntity.Request
{
    public record UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }

    }
}
