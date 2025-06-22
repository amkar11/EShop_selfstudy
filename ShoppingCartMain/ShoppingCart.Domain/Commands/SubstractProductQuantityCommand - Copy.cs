using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ShoppingCart.Domain.Commands
{
    public class SubstractProductQuantityCommand : IRequest
    {
        public int cartId { get; set; }
        public int productId {  get; set; }
    }
}
