using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Dto
{
    public class CreateCartDto
    {
        public int userId { get; set; }
        public string? is_checkout { get; set; }
    }
}
