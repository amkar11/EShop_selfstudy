using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.ValidationAttributes;

namespace User.Domain.Models
{
    public class PasswordRequestViewModel
    {
        [DisplayName("Your email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        
    }
}
