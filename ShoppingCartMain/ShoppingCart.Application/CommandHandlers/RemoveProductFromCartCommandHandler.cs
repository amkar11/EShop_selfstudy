using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.CommandHandlers
{
    public class RemoveProductFromCartCommandHandler : IRequestHandler<RemoveProductFromCartCommand>
    {
        private readonly ICartProductRemover _cartRemover;
        public RemoveProductFromCartCommandHandler(ICartProductRemover cartRemover)
        {
            _cartRemover = cartRemover;
        }

        public async Task Handle(RemoveProductFromCartCommand command, CancellationToken cancellationToken)
        {
            await _cartRemover.RemoveProductFromCartAsync(command.cartId, command.productId);
        }
    }
}
