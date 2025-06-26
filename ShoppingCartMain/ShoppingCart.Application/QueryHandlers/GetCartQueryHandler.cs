using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.Queries;

namespace ShoppingCart.Application.QueryHandlers
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Cart?>
    {
        private readonly ICartReader _cartReader;

        public GetCartQueryHandler(ICartReader cartReader)
        {
            _cartReader = cartReader;
        }

        public async Task<Cart?> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            return await _cartReader.GetCartAsync(query.cartId);
        }
    }
}
