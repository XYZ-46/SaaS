using System.ComponentModel.DataAnnotations;

namespace DataEntity.Request
{
    public record LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
