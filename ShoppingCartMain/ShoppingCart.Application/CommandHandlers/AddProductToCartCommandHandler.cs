using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShoppingCart.Domain.Commands;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.CommandHandlers
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand>
    {

        private readonly ICartProductAdder _cartAdder;

        public AddProductToCartCommandHandler(ICartProductAdder cartAdder)
        {
            _cartAdder = cartAdder;
        }

        public async Task Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
        {
            await _cartAdder.AddProductToCartAsync(command.item);
        }
    }
}
