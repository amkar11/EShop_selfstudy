using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Queries
{
    public class GetCartQuery : IRequest<Cart>
    {
        public int cartId { get; set; }
    }
}
