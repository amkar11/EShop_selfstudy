using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.ValidationAttributes;

namespace User.Domain.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "You have to enter your username!")]
        [NoAtSymbol]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "You have to enter your email address!")]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "You have to enter your password!")]
        [PasswordContains]
        [Length(8, 30)]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "You have to confirm your password!")]
        [Compare("Password", ErrorMessage = "Passwords don`t match!")]
        public string PasswordConfirmation { get; set; } = default!;

        [MaxLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } = default!;

        [MaxLength(30)]
        public string Country { get; set; } = default!;

        [MaxLength(60)]
        public string Region { get; set; } = default!;

        [MaxLength(100)]
        public string Settlement { get; set; } = default!;

        [MaxLength(15)]
        public string PostalCode { get; set; } = default!;
    }
}
