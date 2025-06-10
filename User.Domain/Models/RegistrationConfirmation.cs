using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public class RegistrationConfirmation
    {
        [DisplayName("Confirmation code")]
        [Required]
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
