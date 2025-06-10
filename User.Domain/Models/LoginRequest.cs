using System.ComponentModel.DataAnnotations;

namespace User.Domain.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "You need to enter username!")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "You need to enter password")]
        public string Password { get; set; } = default!;

        public bool RememberMe { get; set; }
    }
}
