using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public class Order
    {
        [Key]
        public int orderId {  get; set; }
        public int userId { get; set; }

        [ForeignKey("userId")]
        public UserDb user { get; set; } = default!;
    }
}
