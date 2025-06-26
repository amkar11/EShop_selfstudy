using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Commands
{
    public class AddProductToCartCommand : IRequest
    {
        public ShopCartItem item { get; set; } = default!;
    }
}
