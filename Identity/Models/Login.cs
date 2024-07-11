using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Login
    {
        public string? ReturnUrl { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
