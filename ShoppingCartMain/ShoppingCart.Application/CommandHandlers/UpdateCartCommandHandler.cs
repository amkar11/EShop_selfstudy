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
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand>
    {
        private readonly ICartUpdater _updater;
        public UpdateCartCommandHandler(ICartUpdater updater)
        {
            _updater = updater;
        }

        public async Task Handle(UpdateCartCommand command, CancellationToken cancellationToken)
        {
            await _updater.UpdateCartAsync(command.cart);
        }
    }
}
