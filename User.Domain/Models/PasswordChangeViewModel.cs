using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.ValidationAttributes;

namespace User.Domain.Models
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "You have to enter your new password!")]
        [PasswordContains]
        [Length(8, 30)]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "You have to confirm your new password!")]
        [Compare("Password", ErrorMessage = "Passwords don`t match!")]
        public string PasswordConfirmation { get; set; } = default!;
    }
}
