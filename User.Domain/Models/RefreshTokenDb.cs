using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public class RefreshTokenDb
    {
        [Key]
        public string RefreshToken { get; set; } = default!;

        public int userId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiredAt { get; set; } = DateTime.UtcNow.AddDays(14);

        public bool isRevoked { get; set; } = false;

        public UserDb? UserDb { get; set; }
    }
}
