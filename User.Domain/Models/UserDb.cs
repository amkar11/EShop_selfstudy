using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public class UserDb
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = default!;

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

        [Required]
        public string PasswordHash { get; set; } = default!;

        public ICollection<Role>? Roles { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<RefreshTokenDb>? RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailConfirmed { get; set; } = false;

    }
}
