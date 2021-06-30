using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.Request
{
    public class UserRegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}