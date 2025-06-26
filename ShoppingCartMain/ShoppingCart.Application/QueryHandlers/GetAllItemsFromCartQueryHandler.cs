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
    public class GetAllItemsFromCartQueryHandler : IRequestHandler<GetAllItemsFromCartQuery, List<ShopCartItem>>
    {
        private readonly ICartProductReader _reader;
        public GetAllItemsFromCartQueryHandler(ICartProductReader reader)
        {
            _reader = reader;
        }

        public async Task<List<ShopCartItem>> Handle(GetAllItemsFromCartQuery query, CancellationToken cancellationToken)
        {
            return await _reader.GetAllItemsFromCartAsync(query.cartId);
        }
    }
}
