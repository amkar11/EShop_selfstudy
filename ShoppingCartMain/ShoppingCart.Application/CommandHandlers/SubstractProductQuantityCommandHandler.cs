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
    public class SubstractProductQuantityCommandHandler : IRequestHandler<SubstractProductQuantityCommand>
    {
        private readonly IProductQuantityChanger _changer;

        public SubstractProductQuantityCommandHandler(IProductQuantityChanger changer)
        {
            _changer = changer;
        }

        public async Task Handle(SubstractProductQuantityCommand command, CancellationToken cancellationToken)
        {
            await _changer.SubstractProductQuantityAsync(command.cartId, command.productId);
        }
    }
}
